using Spectre.Console;

namespace prison_of_skills;

/* Command Handler */

/* To Add/Remove/Edit commands do the following: */
// 1. Update Command.cs
// 2. Create a new Handle<new_command> method that follows the handleDelegate
// or Edit/Remove the corresponding Handle<command> method for the command you want to edit/remove
// 3. Update LoadCommands method

public class CommandHandler
{
    private readonly Game _game;

    private readonly Dictionary<string, Command> _allCommands = [];

    private delegate void handleDelegate(params string[] args);
    private readonly Dictionary<Command, handleDelegate> commandToHandler = [];

    public CommandHandler(Game game)
    {
        _game = game;
        LoadCommands();
    }

    private void LoadCommands()
    {
        foreach (var command in Enum.GetValues<Command>())
            _allCommands.Add(command.ToString(), command);

        commandToHandler[Command.MOVE] = HandleMove;
        commandToHandler[Command.PICK] = HandlePick;
        commandToHandler[Command.DROP] = HandleDrop;
        commandToHandler[Command.SHOW] = HandleShow;
        commandToHandler[Command.CLEAR] = HandleClear;
        commandToHandler[Command.RESTART] = HandleRestart;
        commandToHandler[Command.EXIT] = HandleExit;
    }

    public void Handle(string line)
    {
        var words = line.Trim().Split().Where(word => !string.IsNullOrEmpty(word)).ToArray();
        string commandString = words[0];

        if (!_allCommands.TryGetValue(commandString.ToUpper(), out Command command))
        {
            // Error Message (Not a valid command)
            Console.Error($"[bold]{commandString}[/] is not a valid command.");
            return;
        }

        commandToHandler[command](words[1..]);
    }

    public void HandleMove(params string[] args)
    {
        if (args.Length > 1)
        {
            Console.Error("Move should accept exactly one argument: [bold]Direction[/]");
            return;
        }
        string directionStr = "";
        if (args.Length == 0)
            directionStr = Console.SelectionPrompt("to which direction?", ["LEFT", "RIGHT", "UP", "DOWN"]);
        else
            directionStr = args[0];

        var directions = Enum.GetValues<Direction>().ToList();

        if (directions.Any(d => string.Equals(d.ToString(), directionStr, StringComparison.OrdinalIgnoreCase)))
            _game.Move(directions.Find(d => string.Equals(d.ToString(), directionStr, StringComparison.OrdinalIgnoreCase)));
        else
        {
            Console.Error($"[bold]{directionStr}[/] is not a valid direction.");
        }
    }

    public void HandlePick(params string[] args)
    {
        List<Skill> availableSkills = _game.GetCellSkills();
        string[] chosen = [];

        if (availableSkills.Count == 0)
        {
            Console.Warning("There are no items to pick :confused_face:");
            return;
        }

        if (args.Length > 0)
            chosen = args;
        else
        {
            _game.ShowCell();
            // Console.NormalText($"Current Cell Skills:\n{string.Join('\n', availableSkills)}");
            chosen = Console.MultiSelectionPrompt(
                "which skills to pick?",
                availableSkills.Select(skill => skill.Name).ToArray()
            ).ToArray();
        }

        foreach (string skillName in chosen)
        {
            if (skillName == "CANCEL") return;
            var skill = availableSkills.Find(skill => string.Equals(skillName, skill.Name, StringComparison.OrdinalIgnoreCase));
            if (skill != null)
                _game.Pick(skill.ID);
            else if (Item.TryGet<Skill>(skillName, out Skill? skill2))
                Console.Warning($"Skill {skillName} is not available in current brain cell!");
            else
                Console.Error($"{skillName} is not a valid skill name!");
        }
    }

    public void HandleDrop(params string[] args)
    {
        List<Skill> availableSkills = _game.GetBagSkills();

        if (availableSkills.Count == 0)
        {
            Console.Warning("There are no items to drop :confused_face:");
            return;
        }

        Console.NormalText($"Bag Skills:\n{string.Join('\n', availableSkills)}");

        // return;
        string[] chosen = [];

        if (args.Length > 0)
            chosen = args;
        else
        {
            chosen = Console.MultiSelectionPrompt(
                "which skills to drop?",
                availableSkills.Select(skill => skill.Name).ToArray()
            ).ToArray();
        }

        foreach (string skillName in chosen)
        {
            var skill = availableSkills.Find(skill => string.Equals(skillName, skill.Name, StringComparison.OrdinalIgnoreCase));
            if (skill != null)
                _game.Drop(skill.ID);
            else if (Item.TryGet<Skill>(skillName, out Skill? skill2))
                Console.Warning($"Skill {skillName} is not available in current brain cell!");
            else
                Console.Error($"{skillName} is not a valid skill name!");
        }
    }


    public void HandleShow(params string[] args)
    {
        string[] choices = ["BAG", "CELL", "COMMANDS", "INTRO"];
        if (args.Length > 1)
        {
            Console.Warning($"Warning: Show command can have maximum 1 argument.");
            return;
        }
        string? choice = null;
        if (args.Length == 1)
        {
            args[0] = args[0].ToUpper();
            if (choices.Contains(args[0]))
                choice = args[0];
            else
                Console.Error($"[bold]{args[0]}[/] is not a valid choice.");
        }

        if (choice == null)
        {
            choice = Console.SelectionPrompt("What to show info about?", choices);
            if (choice == "CANCEL") return;
        }

        if (choice == "BAG")
            _game.ShowBag();
        else if (choice == "CELL")
            _game.ShowCell();
        else if (choice == "COMMANDS")
            _game.ShowCommands();
        else if (choice == "INTRO")
            _game.ShowIntro();
    }

    public void HandleClear(params string[] args)
        => Console.Clear();

    public void HandleRestart(params string[] args)
    {
        _game.Restart();
    }

    public void HandleExit(params string[] args)
    {
        _game.Exit();
    }
}