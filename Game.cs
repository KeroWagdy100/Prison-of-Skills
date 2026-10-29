namespace zelda_game;

public class Game
{
    private bool IsRunning { get; set; } = false;
    private readonly Player Player = new();

    private Cell[,] Cells { get; set; } = new Cell[1, 1];
    private Movement Movement { get; set; } = new(1, 1);
    private Bag MyBag { get; set; } = new();

    public Cell CurrentCell => Cells[Movement.Pos.x, Movement.Pos.y];

    readonly private CommandHandler Handler;

    // -----------------------------------------
    public Game()
    {
        Handler = new(this);
        Logger.Clear();
    }

    // Initializes the game with the given filename
    public void Init(string filename)
    {
        GameLoader.LoadGame(filename, out var cells, out var movement);
        Cells = cells;
        Movement = movement;
    }

    public void Run()
    {
        IsRunning = true;
        System.Console.Clear();
        ShowIntro();
        ShowCommands();

        while (IsRunning)
        {
            string? line = Console.Ask();
            if (line is null)
            {
                Exit();
                break;
            }

            Handler.Handle(line);
        }
    }

    public void ShowIntro()
        => Console.SlowPrint(Console.GetStaticText("intro"));

    public void ShowCommands()
        => Console.NormalText(Console.GetStaticText("commands"));

    public void Move(Direction direction)
    {
        if (!Movement.MoveTo(direction))
        {
            // Error Message: out of map bounds
            Console.Error($"Cannot move [bold]{direction}[/]");
            return;
        }

        Console.Confirm($"Moved {direction} to {Movement.Pos}");

        var myBagSkills = MyBag.GetItems();

        bool costChanged = false;
        foreach (var obs in CurrentCell.GetObstacles())
        {
            if (myBagSkills.Any(skill => skill.Type == obs.DefeatedBy))
            {
                Console.SlowPrint($"{obs.PassedText}");
                Console.SlowPrint($"[green]You passed the obstacle without wasting time. :happy_face:[/]");
            }
            else
            {
                Console.SlowPrint($"{obs.TrapText}");
                Console.Story($"You could have avoided wasting [bold red]{obs.TimeCost} months[/] if you had learned any {obs.DefeatedBy} skill. :crying_face:");
                MyBag.TimeWasted += obs.TimeCost;
                costChanged = true;
            }
        }

        if (costChanged)
            Console.Story($"You now have {4*12 - MyBag.TotalCost} months left.");
    }

    public void Pick(Guid skillId)
    {
        if (!Item.TryGet<Skill>(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!CurrentCell.HasSkill(skillId))
        {
            Console.Error($"Error: {skill.Name} is not available in the current room");
            return;
        }

        CurrentCell.TakeSkill(skillId);
        MyBag.Add(skillId);
        Console.Confirm($"Picked [bold]{skill.Name}[/].\n");
        ShowBag();
    }

    public void Drop(Guid skillId)
    {
        if (!Item.TryGet<Skill>(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!MyBag.HasItem(skillId))
        {
            // User Error: skill is not available in my bag
            Console.Error($"Error: {skill.Name} is not available in my bag.");
            return;
        }

        MyBag.Drop(skillId);
        CurrentCell.AddSkill(skillId);
        Console.Confirm($"Dropped {skill.Name}.");
    }

    public void ShowBag()
    {
        Console.NormalText(MyBag.ToString());
    }

    public void ShowCell()
    {
        Console.NormalText($"Current Cell: ({Movement.Pos.x}, {Movement.Pos.y})");
        CurrentCell.ShowInfo();
        Console.NormalText("");
    }

    public List<Skill> GetBagSkills()
        => MyBag.GetItems();

    public List<Skill> GetCellSkills()
        => CurrentCell.GetSkills();

    public void Restart()
    {

    }

    public void Exit()
        => IsRunning = false;

}