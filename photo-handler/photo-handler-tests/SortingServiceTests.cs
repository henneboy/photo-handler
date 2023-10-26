using FluentAssertions;
using photo_handler;

namespace photo_handler_tests;

public class SortingServiceTests
{
	private readonly string _testDataPath = "../../../SortingServiceTestData";

	[Fact]
	public void SortingService_Tests()
	{
		ISortingService ss = new SortingService();
		State state = new()
		{
			Criteria = new HashSet<Criteria>() { Criteria.filename },
			FromDir = new DirectoryInfo(Path.Combine(_testDataPath, "EqualData")),
			ToDir = new DirectoryInfo(Path.Combine(_testDataPath, "ToDirEqualData"))
		};
		ss.PerformSorting(true, state);
		File.Exists(Path.Combine(_testDataPath, "EqualData", "f1.txt")).Should().BeTrue();
		File.Exists(Path.Combine(_testDataPath, "EqualData", "f2.txt")).Should().BeTrue();
	}
}
