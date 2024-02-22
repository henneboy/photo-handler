namespace photo_handler;

public class Commands
{
	private readonly ISortingService _ss;
	public Commands(ISortingService ss)
	{
		_ss = ss;
	}
	private readonly State _state = new();
	public Dictionary<string, Func<bool>> GetCommands()
	{
		Dictionary<string, Func<bool>> actions = new()
		{
			{ "help-usage", HelpUsage },
			{ "help", Help },
			{ "exit", Exit},
			{ "clear", ClearConsole},
			{ "set-from-dir", SetFromDir},
			{ "set-to-dir", SetToDir},
			{ "set-criteria", SetCriteria},
			{ "view-state", ViewState},
			{ "run-sorting", RunSorting}
		};
		return actions;
	}
	private static bool ClearConsole()
	{
		Console.Clear();
		return false;
	}
	private static bool Exit() => true;
	private bool Help()
	{
		Console.WriteLine("-------------------------");
		Console.WriteLine("Commands:");
		foreach (string key in GetCommands().Keys)
		{
			Console.WriteLine(key);
		}
		return false;
	}
	public static bool HelpUsage()
	{
		Console.WriteLine("Standard procedure for sorting files:");
		Console.WriteLine("1. Ensure all the files (photos) you want to sort are in a single directory (from directory).");
		Console.WriteLine("2. 'set-from-dir' to set the from directory");
		Console.WriteLine("3. Create a destination directory (to directory).");
		Console.WriteLine("4. 'set-to-dir' to set the to directory");
		Console.WriteLine("5. 'set-criteria' to set the criteria use for comparing files (photos).");
		Console.WriteLine("6. 'view-state' to get an overview of the selected dirs and criteria.");
		Console.WriteLine("7. 'run-sorting' to start sorting/duplication elimination. (this might take a while)");
		Console.WriteLine("8. If desired use the 'delete-from-dir' to remove the from directory,\n	since the files have been moved to the to directory.");
		return false;
	}

	private bool SetFromDir()
	{
		DirectoryInfo? fromDir = PromtForDir();
		if (fromDir != null)
		{
			_state.FromDir = fromDir;
			Console.WriteLine($"'from directory' successfully set to {fromDir}");
		}
		return false;
	}

	private bool SetToDir()
	{
		DirectoryInfo? toDir = PromtForDir("to");
		if (toDir != null)
		{
			_state.ToDir = toDir;
			Console.WriteLine($"'to directory' successfully set to {toDir}");
		}
		return false;
	}

	private static DirectoryInfo? PromtForDir(string directoryType = "from")
	{
		DirectoryInfo? dir = null;
		bool exit = false;
		while (dir == null || exit)
		{
			Console.WriteLine($"Please enter path to the desired {directoryType} directory or 'cancel'");
			string? response = Console.ReadLine();
			if (string.IsNullOrEmpty(response))
			{
				Console.WriteLine($"Could not find dir: {response}, please enter valid directory path");
			}
			if (response == "cancel")
			{
				exit = true;
				continue;
			}
			dir = new(response!);
			if (!dir.Exists)
			{
				Console.WriteLine($"Could not find dir: {response}, please enter valid directory path");
				dir = null;
			}
		}
		return dir;
	}

	private bool ViewState()
	{
		Console.WriteLine("Current state:");
		string fromDir = _state.FromDir == null ? "not set" : _state.FromDir.FullName;
		string toDir = _state.ToDir == null ? "not set" : _state.ToDir.FullName;
		Console.WriteLine($"From directory: {fromDir}");
		Console.WriteLine($"To directory: {toDir}");
		PrintSelectedCriteria();
		return false;
	}

	private bool SetCriteria()
	{
		PrintSelectedCriteria();
		Console.WriteLine("Do you wish to add any of these unselected criteria? (y/n)");
		if (CLI.YOrNoPromt())
		{
			Console.WriteLine("Please enter the criteria you wish to add. (separate multiple arguments with a space)");
			string[] args = Console.ReadLine()!.Split(' ');
			foreach (string arg in args)
			{
				if (Enum.TryParse(arg, out Criteria argEnum))
				{
					_state.Criteria!.Add(argEnum);
				}
			}
		}
		else
		{
			Console.WriteLine("Do you wish to remove any of the selected criteria? (y/n)");
			if (CLI.YOrNoPromt())
			{
				Console.WriteLine("Please enter the criteria you wish to remove. (separate multiple arguments with a space)");
				string[] args = Console.ReadLine()!.Split(' ');
				foreach (string arg in args)
				{
					if (Enum.TryParse(arg, out Criteria argEnum))
					{
						_state.Criteria!.Remove(argEnum);
					}
				}
			}
		}
		Console.WriteLine("Criteria have been updated.");
		return false;
	}

	private void PrintSelectedCriteria()
	{
		_state.Criteria ??= new();
		Console.WriteLine("The program is currently set to compare files based on: ");
		if (_state.Criteria.Count == 0)
		{
			Console.WriteLine("No criteria have been set.");
		}
		else
		{
			foreach (Criteria arg in _state.Criteria)
			{
				Console.WriteLine(arg);
			}
		}
		Console.WriteLine("The program does not use the following criteria in the comparison: ");
		foreach (Criteria arg in Enum.GetValues(typeof(Criteria)))
		{
			if (!_state.Criteria.Contains(arg))
			{
				Console.WriteLine(arg);
			}
		}
	}

	private bool RunSorting()
	{
		Console.WriteLine("Do you want to avoid copying duplicates of the same file.");
		var result = _ss.PerformSorting(CLI.YOrNoPromt(), _state);
		Console.WriteLine("Sorting completed, all files can be found in: " + _state.ToDir);
		Console.WriteLine("If desired use the 'delete-from-dir' to remove the from directory: " + _state.FromDir);
		Console.WriteLine(new string('-', 30));
		Console.WriteLine("Stats of the sorting:");
		Console.WriteLine("Execution time: " + result.ExecutionTime);
		Console.WriteLine("Number of duplicate files: " + result.NumberOfDuplicateFiles);
		Console.WriteLine("Size of source directory(from dir): " + result.FromByteSize);
		Console.WriteLine("Size of target directory(to dir): " + result.ToByteSize);
		return false;
	}
}
