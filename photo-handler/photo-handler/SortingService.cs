﻿namespace photo_handler;

public class SortingService : ISortingService
{
	public SortingResults PerformSorting(bool allowDuplicates, State state)
	{
		DateTimeOffset ExecutionStartTime = DateTimeOffset.UtcNow;
		long FromByteSize = state.FromDir!.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
		// Perform sorting
		long NumberOfDuplicateFiles = SortingAlgo(allowDuplicates, state);

		long ToByteSize = state.ToDir!.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
		return new SortingResults(FromByteSize, ToByteSize, NumberOfDuplicateFiles, ExecutionStartTime - DateTimeOffset.UtcNow);
	}

	/// <summary>
	/// Performs the sorting.
	/// </summary>
	/// <param name="allowDuplicates"></param>
	/// <param name="state"></param>
	/// <returns>The amount of dupliace files found.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	private static long SortingAlgo(bool allowDuplicates, State state)
	{
		long numberOfDuplicateFiles = 0;
		// NOTE: maybe use array and null for deleted items ->
		// This allows for iterating using for loop+null checks.
		// OR use immutable list and auxilary hashset with index of deleted items.
		List<string> filesToSort = state.FromDir!.EnumerateFiles("*", SearchOption.AllDirectories).Select(file => file.FullName).ToList();

		while (filesToSort.Count >= 2)
		{
			string pathToCurrentFile = filesToSort.First();
            filesToSort.Remove(pathToCurrentFile);
			FileInfo currentFile = new(pathToCurrentFile);
            Console.WriteLine($"{filesToSort.Count} left, current file: {currentFile.Name}");
			//Sort this file wrt. the other files.
			if (!allowDuplicates)
			{
				List<string> filesToRemove = new();
				foreach (string filePath in filesToSort)
				{
					FileInfo file = new(filePath);
					if (FileComparer.CompareFiles(currentFile, file, state.Criteria!))
					{
						filesToRemove.Add(filePath);
						numberOfDuplicateFiles++;
					}
				}
				int filesRemoved = filesToSort.RemoveAll(f => filesToRemove.Contains(f));
				if (filesRemoved != filesToRemove.Count)
				{
					throw new InvalidOperationException("Did not remove all duplicates from work-list");
				}
				filesToRemove.Clear();
			}
			MoveCurrentFileToTargetDir(currentFile, state);
		}
		if (filesToSort.Count == 1)
		{
			MoveCurrentFileToTargetDir(new FileInfo(filesToSort.Single()), state);
		}
		return numberOfDuplicateFiles;
	}

	private static void MoveCurrentFileToTargetDir(FileInfo file, State state)
	{
		string destDirPath = Path.Join(state.ToDir!.FullName, file.CreationTimeUtc.Year.ToString());
		if (!Directory.Exists(destDirPath))
		{
			Directory.CreateDirectory(destDirPath);
		}
		string destFilePath = Path.Join(destDirPath, file.Name);
		FileInfo destFile = file.CopyTo(destFilePath);
		destFile.CreationTimeUtc = file.CreationTimeUtc;
	}
}
