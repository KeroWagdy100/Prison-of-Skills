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
        commandToHandler[Command.EXIT] = HandleExit;
    }

    public void Handle(string line)
    {
        var words = line.Split();
        string commandString = words[0].ToUpper();

        if (!AllCommmands.TryGetValue(commandString, out Command command))
        {
            // Error Message (Not a valid command)
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
    }

    public void HandleDrop(params string[] args)
    {
    }

    public void HandleAttack(params string[] args)
    {
    }

    public void HandleInventory(params string[] args)
    {
    }

    public void HandleExit(params string[] args)
    {
        _game.Exit();
    }
}