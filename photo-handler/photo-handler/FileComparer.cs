namespace photo_handler;

public static class FileComparer
{

	public static readonly Dictionary<Criteria, Func<FileInfo, FileInfo, bool>> ComparisonFuncs = new()
	{
		{ Criteria.filename, (a, b) => a.Name == b.Name},
		{ Criteria.filesize, (a, b) => a.Length == b.Length},
		{ Criteria.lastmodified, (a, b) => a.LastWriteTime == b.LastWriteTime},
		{ Criteria.creationTime, (a, b) => a.CreationTime == b.CreationTime},
		{ Criteria.filecontent, (a, b) => EqualData(a, b)},
		{ Criteria.filetype, (a, b) => a.Extension == b.Extension}
	};

	private static bool EqualData(FileInfo f1, FileInfo f2)
	{
		FileStream rf1 = f1.OpenRead();
		FileStream rf2 = f2.OpenRead();
		if (f1.Length != f2.Length && f1.Extension != f2.Extension)
		{
			return false;
		}
		// Code snippet from stackoverflow for optimal comparison speed:
		const int BYTES_TO_READ = sizeof(Int64);
		int iterations = (int)Math.Ceiling((double)f1.Length / BYTES_TO_READ);
		byte[] one = new byte[BYTES_TO_READ];
		byte[] two = new byte[BYTES_TO_READ];
		for (int i = 0; i < iterations; i++)
		{
			rf1.Read(one, 0, BYTES_TO_READ);
			rf2.Read(two, 0, BYTES_TO_READ);

			if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
				return false;
		}
		return true;
	}

	/// <summary>
	/// Compares two files.
	/// </summary>
	/// <param name="sourceFile"></param>
	/// <param name="f2"></param>
	/// <param name="criteria"></param>
	/// <returns>Return true if the two files are equal according to the criteria.</returns>
	public static bool CompareFiles(FileInfo sourceFile, FileInfo f2, IEnumerable<Criteria> criteria)
	{
		foreach (Criteria arg in criteria)
		{
			if (ComparisonFuncs.TryGetValue(arg, out Func<FileInfo, FileInfo, bool>? func))
			{
				if (!func(sourceFile, f2))
				{
					return false;
				}
			}
		}
		return true;
	}
}
