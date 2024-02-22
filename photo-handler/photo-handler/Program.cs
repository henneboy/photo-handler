namespace photo_handler;

public class Program
{
	static void Main()
	{
		string projectName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name!;
		Console.WriteLine($"Starting application {projectName}");
		CLI cli = new(new SortingService());
		cli.Run();
	}
}