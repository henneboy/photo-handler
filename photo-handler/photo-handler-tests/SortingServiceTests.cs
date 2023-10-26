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
		string fromDir = Path.Combine(_testDataPath, "ToDirEqualData");
		string toDir = Path.Combine(_testDataPath, "EqualData");
		HashSet<Criteria> criterias = new()
		{
			Criteria.filename
		};

		// Clean up from previous run
		File.Delete(fromDir);

		// Assert required input data exists
		File.Exists(Path.Combine(toDir, "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(toDir, "f2.txt")).Should().BeTrue();

		// Arrange
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = criterias,
			FromDir = new DirectoryInfo(toDir),
			ToDir = new DirectoryInfo(fromDir)
		};

		// Act
		ss.PerformSorting(true, state);

		// Assert

	}
}
