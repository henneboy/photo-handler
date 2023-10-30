using FluentAssertions;
using photo_handler;

namespace photo_handler_tests;

public class SortingServiceTests
{
	private readonly string _testDataPath = "../../../SortingServiceTestData";

	[Fact]
	public void SortingService_Tests()
	{
		// Configuration
		string fromDir = Path.Combine(_testDataPath, "fromDir");
		string toDir = Path.Combine(_testDataPath, "toDir");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filename
		};

		// Clean up from previous run
		var subDirs = Directory.GetDirectories(toDir);
		foreach (string subDir in subDirs)
		{
			Directory.Delete(subDir);
		}

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
		File.Exists(Path.Combine(toDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, "f2.txt")).Should().BeTrue();
	}
}
