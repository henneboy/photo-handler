namespace photo_handler;

public static class FileNameParser
{
	public static string GetCreationYear(FileInfo file)
	{
		if (file.Name.StartsWith("JPEG_"))
		{
			return file.Name.Substring("JPEG_".Length, 4);
		}
		if (file.Name.StartsWith("Screenshot_"))
		{
			return file.Name.Substring("Screenshot_".Length, 4);
		}
		if (file.Name.StartsWith("HENRIK - WIN_"))
		{
			return file.Name.Substring("HENRIK - WIN_".Length, 4);
		}
		if (file.Name.Length == "20190812_182049.jpg".Length && file.Name.ElementAt(8) == '_' && int.TryParse(file.Name[..4], out int res))
		{
			return res.ToString();
		}
		return file.CreationTimeUtc.Year.ToString();
	}
}
