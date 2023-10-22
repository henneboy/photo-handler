using photo_handler;
using FluentAssertions;

namespace photo_handler_tests;

public class FileComparerTests
{
	private readonly string _testDataPath = "../../../FileComparerTestData";

	[Theory]
	[InlineData(Criteria.filecontent, true)]
	[InlineData(Criteria.filetype, true)]
	[InlineData(Criteria.created, false)]
	[InlineData(Criteria.filename, false)]
	public void CompareFiles_Tests(Criteria criteria, bool result)
	{
		FileInfo f1 = new(Path.Combine(_testDataPath, "EqualData", "f1.txt"));
		FileInfo f2 = new(Path.Combine(_testDataPath, "EqualData", "f2.txt"));
		List<Criteria> criteriaList = new() { criteria };
		FileComparer.CompareFiles(f1, f2, criteriaList).Should().Be(result);
	}
}