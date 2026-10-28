namespace zelda_game;

/* Command Handler */

/* To Add/Remove/Edit commands do the following: */
// 1. Update Command.cs
// 2. Create a new Handle<new_command> method that follows the handleDelegate
// or Edit/Remove the corresponding Handle<command> method for the command you want to edit/remove
// 3. Update LoadCommands method

public class CommandHandler
{
    private readonly Game _game;

    private readonly Dictionary<string, Command> AllCommmands = [];

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
            AllCommmands.Add(command.ToString(), command);

        commandToHandler[Command.MOVE] = HandleMove;
        commandToHandler[Command.PICK] = HandlePick;
        commandToHandler[Command.DROP] = HandleDrop;
        commandToHandler[Command.ATTACK] = HandleAttack;
        commandToHandler[Command.INVENTORY] = HandleInventory;
        commandToHandler[Command.LOOK] = HandleLook;
        commandToHandler[Command.EXIT] = HandleExit;
    }

    public void Handle(string line)
    {
        var words = line.Trim().Split();
        string commandString = words[0].ToUpper();

        if (!AllCommmands.TryGetValue(commandString, out Command command))
        {
            // Error Message (Not a valid command)
            Console.WriteLine($"Error: {commandString} is not a valid command\n");
            return;
        }

        commandToHandler[command](words[1..]);
    }

    public void HandleMove(params string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Move should accept one argument only: [Direction]");
        var directions = Enum.GetValues<Direction>().ToList();

        if (directions.Any(d => string.Equals(d.ToString(), args[0], StringComparison.OrdinalIgnoreCase)))
            _game.Move(directions.Find(d => string.Equals(d.ToString(), args[0], StringComparison.OrdinalIgnoreCase)));
        else
            throw new ArgumentException($"[{args[0]}] is not a direction.");
    }

    public void HandlePick(params string[] args)
    {
        foreach (string arg in args)
        {
            if (!Skill.Get(arg, out Skill? skill))
            {
                // User Error: {arg} is not an skill
                Console.WriteLine($"Error: {arg} is not an skill");
                continue;
            }
            _game.Pick(skill.ID);
        }
    }

    public void HandleDrop(params string[] args)
    {
        foreach (string arg in args)
        {
            if (!Skill.Get(arg, out Skill? skill))
            {
                // User Error: {arg} is not an skill
                Console.WriteLine($"Error: {arg} is not an skill");
                continue;
            }
            _game.Drop(skill.ID);
        }
    }

    public void HandleAttack(params string[] args)
    {
    }

    public void HandleInventory(params string[] args)
    {
        if (args.Length > 0)
        {
            // User Warning: Inventory Command doesn't require any arguments
            Console.WriteLine($"Warning: Inventory command doesn't require any arguments");
            return;
        }
        _game.ShowInventory();
    }

    public void HandleLook(params string[] args)
    {
        if (args.Length > 0)
        {
            // User Warning: Look Command doesn't require any arguments
            Console.WriteLine($"Warning: Look command doesn't require any arguments");
            return;
        }
        _game.ShowCellDetails();
    }

    public void HandleExit(params string[] args)
    {
        _game.Exit();
    }
}