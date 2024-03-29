﻿using FluentAssertions;
using photo_handler;

namespace photo_handler_tests;

public class SortingServiceTests
{
	private readonly string currentYear;
	public SortingServiceTests()
	{
		currentYear = DateTime.UtcNow.Year.ToString();
	}
	private readonly string _testDataPath = "../../../SortingServiceTestData";
	private static void ResetResultDir(string toDirPath)
	{
		// Clean up from previous run
		string[] subDirs;
		try
		{
			subDirs = Directory.GetDirectories(toDirPath);
		}
		catch (Exception)
		{
			Directory.CreateDirectory(toDirPath);
			subDirs = Directory.GetDirectories(toDirPath);
		}
		foreach (string subDir in subDirs)
		{
			foreach (var file in Directory.GetFiles(subDir))
			{
				File.Delete(file);
			}
		}
	}

	[Fact]
	public void AllowDuplicates_Keep_All()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir");
		string toDir = Path.Combine(_testDataPath, "1_toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filename
		};

		// Clean up from previous run
		ResetResultDir(toDir);

		// Assert required input data exists
		File.Exists(Path.Combine(fromDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(fromDir, "f2.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(fromDir),
			ToDir = new DirectoryInfo(toDir)
		};

		// Act
		ss.PerformSorting(true, state);

		// Assert
		File.Exists(Path.Combine(toDir, currentYear, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, currentYear, "f2.txt")).Should().BeTrue();
	}

	[Fact]
	public void DisallowDuplicates_Criteria_Filename_Keep_All()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir");
		string toDir = Path.Combine(_testDataPath, "2_toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filename
		};

		// Clean up from previous run
		ResetResultDir(toDir);

		// Assert required input data exists
		File.Exists(Path.Combine(fromDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(fromDir, "f2.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(fromDir),
			ToDir = new DirectoryInfo(toDir)
		};

		// Act
		ss.PerformSorting(false, state);

		// Assert
		File.Exists(Path.Combine(toDir, currentYear, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, currentYear, "f2.txt")).Should().BeTrue();
	}

	[Fact]
	public void DisallowDuplicates_Criteria_Content_Keep_One()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir");
		string toDir = Path.Combine(_testDataPath, "3_toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filecontent
		};

		// Clean up from previous run
		ResetResultDir(toDir);

		// Assert required input data exists
		File.Exists(Path.Combine(fromDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(fromDir, "f2.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(fromDir),
			ToDir = new DirectoryInfo(toDir)
		};

		// Act
		ss.PerformSorting(false, state);

		// Assert
		File.Exists(Path.Combine(toDir, currentYear, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, currentYear, "f2.txt")).Should().BeFalse();
	}

	[Fact]
	public void AllowDuplicates_Criteria_Content_CorrectCreationTime()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir");
		string toDir = Path.Combine(_testDataPath, "4_toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filecontent
		};

		// Clean up from previous run
		ResetResultDir(toDir);

		// Assert required input data exists
		File.Exists(Path.Combine(fromDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(fromDir, "f2.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(fromDir),
			ToDir = new DirectoryInfo(toDir)
		};

		// Act
		ss.PerformSorting(true, state);

		// Assert
		File.Exists(Path.Combine(toDir, currentYear, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, currentYear, "f2.txt")).Should().BeTrue();

		FileInfo f1 = new(Path.Combine(toDir, currentYear, "f1.txt"));
		FileInfo f1_expected = new(Path.Combine(fromDir, "f1.txt"));
		f1.CreationTimeUtc.Should().Be(f1_expected.CreationTimeUtc);

		FileInfo f2 = new(Path.Combine(toDir, currentYear, "f2.txt"));
		FileInfo f2_expected = new(Path.Combine(fromDir, "f2.txt"));
		f2.CreationTimeUtc.Should().Be(f2_expected.CreationTimeUtc);
	}

	[Fact]
	public void Criteria_fileContent_MultipleFilesWithSameName_Keep_All()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir-nameClash");
		string toDir = Path.Combine(_testDataPath, "5_toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filecontent
		};

		// Clean up from previous run
		ResetResultDir(toDir);

		// Assert required input data exists
		File.Exists(Path.Combine(fromDir, "file.txt")).Should().BeTrue();
		File.Exists(Path.Combine(fromDir, "f", "file1.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(fromDir),
			ToDir = new DirectoryInfo(toDir)
		};

		// Act
		var sortingResults = ss.PerformSorting(false, state);

		// Assert
		sortingResults.FromByteSize.Should().Be(sortingResults.ToByteSize);
		File.Exists(Path.Combine(toDir, currentYear, "file.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, currentYear, "file1.txt")).Should().BeTrue();
	}
}
