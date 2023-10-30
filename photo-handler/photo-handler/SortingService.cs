using System.IO;

namespace photo_handler;

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

	private static long SortingAlgo(bool allowDuplicates, State state)
	{
		long numberOfDuplicateFiles = 0;
		HashSet<string> filesToSort = state.FromDir!.EnumerateFiles("*", SearchOption.AllDirectories).Select(file => file.FullName).ToHashSet<string>();
		while (filesToSort.Count >= 2)
		{
			string pathToCurrentFile = filesToSort.First();
			filesToSort.Remove(pathToCurrentFile);
			FileInfo currentFile = new(pathToCurrentFile);
			//Sort this file wrt. the other files.
			if (!allowDuplicates)
			{
				foreach (string filePath in filesToSort)
				{
					FileInfo file = new(filePath);
					if (FileComparer.CompareFiles(currentFile, file, state.Criteria!))
					{
						filesToSort.Remove(filePath);
						numberOfDuplicateFiles++;
					}
				}
			}
			MoveCurrentFileToTargetDir(currentFile, state);
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
		file.CopyTo(Path.Join(destDirPath, file.Name));
	}
}
