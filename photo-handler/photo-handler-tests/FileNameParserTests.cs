using photo_handler;

namespace photo_handler_tests;

public class FileNameParserTests
{
	private readonly string _testDataPath = "../../../FileNameParserData";

	[Theory]
	[InlineData("20191215_020656.jpg", "2019")]
	[InlineData("59a93f3c-3bed-45bf-8de3-00d048fca041.jpg", "2024")]
	[InlineData("HENRIK - WIN_20150608_121616.jpg", "2015")]
	[InlineData("JPEG_20200507_225305.jpg", "2020")]
	[InlineData("Screenshot_20190910-064953_Chrome.jpg", "2019")]

	public void GetCreationTime_ReturnsCorrectYear(string filename, string year)
	{
		FileInfo f = new(Path.Combine(_testDataPath, filename));
		string result = FileNameParser.GetCreationYear(f);
		Assert.Equal(year, result);
	}
}
