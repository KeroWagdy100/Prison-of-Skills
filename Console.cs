using Spectre.Console;

namespace zelda_game;

public static class Console
{
    private readonly static string _normalColor = "white";
    private readonly static string _storyColor = "cyan";
    private readonly static string _errorColor = "red";
    private readonly static string _warningColor = "darkorange";
    private readonly static string _confirmationColor = "green";

    private static void Print(string str, string color)
    {
        AnsiConsole.MarkupLine($"[{color}]{str}[/]");
    }

    public static void Error(string err)
        => Print(err, _errorColor);

    public static void Warning(string warning)
        => Print(warning, _warningColor);

    public static void Story(string text)
        => Print(text, _storyColor);

    public static void NormalText(string text)
        => Print(text, _normalColor);

    public static void Confirm(string confirmationMessage)
        => Print(confirmationMessage, _confirmationColor);

    public static string Ask(string prompt = ">")
    {
        return AnsiConsole.Ask<string>(prompt);
    }
}