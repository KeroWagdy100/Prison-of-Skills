namespace prison_of_skills;

public static class Logger
{
    readonly static public string _logfile = "log/Game.log";

    public static void Init()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_logfile)!);
    }

    public static void Log(params string[] strings)
    {
        using StreamWriter writer = new(_logfile, true);
        foreach (string str in strings)
            writer.WriteLine(str);
    }

    public static void Success(params string[] strings)
    {
        using StreamWriter writer = new(_logfile, true);
        writer.Write("[SUCCESS]: ");
        foreach (string str in strings)
            writer.WriteLine($"{str}");
    }

    public static void Error(params string[] errors)
    {
        using StreamWriter writer = new(_logfile, true);
        foreach (string err in errors)
            writer.WriteLine($"[ERROR]: {err}");
    }

    public static void Clear()
    {
        File.WriteAllText(_logfile, null);
    }
}