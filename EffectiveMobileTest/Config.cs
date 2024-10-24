using System.Text.Json;

namespace EffectiveMobileTest;

public class Config
{
	private static Config? instance;

	private Config()
	{
		string path = @"./settings.json";

		try
		{
			var json = File.ReadAllText(path);
			Dictionary<string, string>? settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

			if (settings == null)
			{
				throw new Exception("Settings file is empty or doesn't have all settings");
			}

			if (!settings.TryGetValue("logPath", out var lpath) && lpath != null && !lpath.Equals(""))
			{
				logPath = Path.Combine(logPath, lpath);
			} else
			{
				Console.WriteLine("Log path incorrect and was set to program directory");
			}
			
			if (!settings.TryGetValue("outPath", out var opath) && opath != null && !opath.Equals(""))
			{
				outPath = Path.Combine(outPath, opath);
			} else
			{
				Console.WriteLine("Out path incorrect and was set to program directory");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
	
	public static Config getInstance()
	{
		return instance ??= new Config();
	}

	public string logPath { get; set; } = Directory.GetCurrentDirectory();
	public string outPath { get; set; } = Directory.GetCurrentDirectory();
}