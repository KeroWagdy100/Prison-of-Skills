using System.Diagnostics;
using Spectre.Console;

namespace prison_of_skills;

public static class Console
{
    private readonly static string _normalColor = "white";
    private readonly static string _storyColor = "cyan";
    private readonly static string _errorColor = "red";
    private readonly static string _warningColor = "darkorange";
    private readonly static string _confirmationColor = "green";

    private readonly static Dictionary<string, string> _staticText = [];


    private static void Print(string str, string color)
        => AnsiConsole.MarkupLine($"[{color}]{str}[/]");

    public static void Clear()
        => AnsiConsole.Clear();

    public static void Error(string err)
        => Print($":cross_mark: {err}", _errorColor);

    public static void Warning(string warning)
        => Print($":warning:  {warning}", _warningColor);

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

    public static void AddStaticText(string name, string text)
        => _staticText.Add(name, text);

    public static string GetStaticText(string name)
    {
        if (!_staticText.TryGetValue(name, out string? text))
            throw new ArgumentException($"Static Text {name} doesn't exist.");
        return text;
    }

    // returns CANCEL if user cancels.
    public static List<string> MultiSelectionPrompt(string prompt, string[] choices)
    {
        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title(prompt)
                .AddChoices(choices)
                .AddCancelResult("CANCEL")
        );
    }

    // returns CANCEL if user cancels.
    public static string SelectionPrompt(string prompt, string[] choices)
        => AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(prompt)
                .AddChoices(choices)
                .AddCancelResult("CANCEL")
                .EnableSearch()
                .SearchPlaceholderText("Type to search...")
        );

    public static void SlowPrint(string text)
    {
        AnsiConsole.MarkupLine("[dim italic on black]Press Space/Escape/Enter to skip[/]");
        text = AnsiMarkup.Remove(text);
        Random random = new();
        bool skip = false;
        for (int i = 0; i < text.Length; ++i)
        {
            if (System.Console.KeyAvailable)
            {
                var keyInfo = System.Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Escape:
                        skip = true;
                        break;
                }
            }

            if (skip) break;

            AnsiConsole.Markup($"{text[i]}");

            int delayInMilliSeconds = random.Next(20, 80);
            if (text[i] == '\n')
                delayInMilliSeconds += 200;
            Thread.Sleep(delayInMilliSeconds);
            // await Task.Delay(delayInMilliSeconds);
        }

        if (skip)
            AnsiConsole.MarkupLine("[italic]...Skipped[/]");
        AnsiConsole.WriteLine();
    }

}