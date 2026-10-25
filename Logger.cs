using System.IO.Pipelines;

namespace zelda_game;

public static class Logger
{
    readonly static public string _logfile = "log.txt";
    public static void Log(params string[] strings)
    {
        using StreamWriter writer = new(_logfile, true);
        foreach (string str in strings)
            writer.WriteLine(str);
    }
    public static void Log(params Exception[] strings)
    {
        using StreamWriter writer = new(_logfile, true);
        foreach (Exception ex in strings)
            writer.WriteLine(ex);
    }
    public static void Clear()
    {
        File.WriteAllText(_logfile, null);
    }
}