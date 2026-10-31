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

    public static void Note(string note)
    {
        var text = new Text(
            note,
            new Style(Color.Grey).Decoration(Decoration.Dim).Decoration(Decoration.Italic)
        ).RightJustified();
        AnsiConsole.Write(text);
    }

    public static void SlowPrint(string text)
    {
        System.Console.CursorVisible = false;

        Note("Press Space/Enter to fast forward, Escape to skip, Use arrows to change typing speed");
        text = AnsiMarkup.Remove(text);
        Random random = new();
        bool skip = false, fastForward = false;

        int delayInMilliSeconds = 80;
        for (int i = 0; i < text.Length; ++i)
        {
            AnsiConsole.Markup($"{text[i]}");


            if (System.Console.KeyAvailable)
            {
                var keyInfo = System.Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        skip = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        fastForward = true;
                        break;
                    case ConsoleKey.DownArrow:
                        delayInMilliSeconds += 20;
                        break;
                    case ConsoleKey.UpArrow:
                        delayInMilliSeconds -= 20;
                        break;
                }
            }

            delayInMilliSeconds = Math.Clamp(delayInMilliSeconds, 20, 150);

            if (skip) break;
            if (fastForward) delayInMilliSeconds = 0;


            // New paragraph
            if (text[i] == '\n' && i > 0 && text[i-1] == '\n') {
                fastForward = false;
                NormalText("[italic blue]Continue?[/] [grey italic](press any key to continue, Escape to skip)[/]");
                var key = System.Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape) {
                    skip = true;
                    break;
                }

                continue;
            }
            Thread.Sleep(delayInMilliSeconds);
        }

        if (skip) { 
            // AnsiConsole.MarkupLine("[italic]...Skipped[/]");
            AnsiConsole.Clear();
        }
        AnsiConsole.WriteLine();

        System.Console.CursorVisible = true;
    }

    public static void PrintTable(this Inventory<Skill> inv, string? title = null, string? caption = null)
    {
        var skills = inv.GetItems();

        var table = new Table();
        table.AddColumns("Skill", "of type", "costs", "adds");
        table.SimpleHeavyBorder();

        if (!string.IsNullOrEmpty(title))
            table.Title(title);

        int totalCost = 0, totalPoints = 0;
        foreach (var skill in skills)
        {
            table.AddRow(
                $"[bold]{skill.Name}[/]",
                skill.Type.ToString(),
                $"[blue]{skill.TimeCost} months[/]",
                $"[green][bold]{skill.Strength}[/] points[/]"
            );
            totalCost += skill.TimeCost;
            totalPoints += skill.Strength;
        }

        table.Columns[0].Footer = new Text($"Total");
        table.Columns[1].Footer = new Text($"{skills.Count} skills", new Style(foreground: Color.Yellow));
        table.Columns[2].Footer = new Text($"{totalCost} months", new Style(foreground: Color.Blue));
        table.Columns[3].Footer = new Text($"{totalPoints} points", new Style(foreground: Color.Green, decoration: Decoration.Bold));

        if (!string.IsNullOrEmpty(caption))
            table.Caption(caption);

        AnsiConsole.Write(table);
    }
}
