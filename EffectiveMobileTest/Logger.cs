namespace EffectiveMobileTest;

public class Logger
{
	private static Logger? instance;

	private Logger()
	{
	}

	private string getLogFilePath()
	{
		return Path.Combine(Config.getInstance().logPath, "L" + DateTime.Now.ToString("MMDDyyyy") + ".txt");
	}

	public async Task Log(string message)
	{
		await using StreamWriter file = new StreamWriter(getLogFilePath(), true);
		await file.WriteLineAsync(DateTime.Now.ToString("F") + ": " + message);
	}

	public static Logger getInstance()
	{
		return instance ??= new Logger();
	}
}