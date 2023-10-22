namespace photo_handler;

public class CLI
{
    private readonly Commands _commandExecutor;
    private readonly Dictionary<string, Func<bool>> commandList;
    public CLI()
    {
        commandList = new();
        _commandExecutor = new Commands();
        var importCommands = _commandExecutor.GetCommands();
        foreach (var cmd in importCommands)
        {
            commandList.Add(cmd.Key, cmd.Value);
        }
    }
    public void Run()
    {
        bool exitProgram = false;
        while (!exitProgram)
        {
            string? line = Console.ReadLine();
            if (line == null)
            {
                continue;
            }

            string[] args = line.Split(' ');
            if (commandList.ContainsKey(args[0]))
            {
                _ = commandList.TryGetValue(args[0], out Func<bool>? f);
                if (f != null)
                {
                    exitProgram = f.Invoke();
                    continue;
                }
            } else
            {
                _ = commandList.TryGetValue("help", out Func<bool>? f);
                if (f != null)
                {
                    bool _ = f.Invoke();
                } else
                {
                    throw new MissingMethodException("Help command not found");
                }
            }
        }
    }
	public static bool YOrNoPromt()
	{
		string? input;
		while (true)
		{
			input = Console.ReadLine();
			if (!String.IsNullOrEmpty(input) && input == "y")
			{
				return true;
			}
			else if (!String.IsNullOrEmpty(input) && input == "n")
			{
				return false;
			}
			else
			{
				Console.WriteLine("Invalid input, please enter 'y' or 'n'");
			}
		}
	}
}
