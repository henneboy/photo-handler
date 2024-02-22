namespace photo_handler;

public class State
{
	public DirectoryInfo? FromDir { get; set; }
	public DirectoryInfo? ToDir { get; set; }
	public HashSet<Criteria>? Criteria { get; set; }
}
