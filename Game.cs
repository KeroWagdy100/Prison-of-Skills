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

    public async Task Run()
    {
        IsRunning = true;
        System.Console.Clear();
        await ShowIntro();

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

    public async Task ShowIntro()
        => await Console.SlowPrint(Console.GetStaticText("intro"));

    public async Task ShowCommands()
        => await Console.SlowPrint(Console.GetStaticText("commands"));

    public void Move(Direction direction)
    {
        if (!Movement.MoveTo(direction))
        {
            // Error Message: out of map bounds
            Console.Error($"Cannot move [bold]{direction}[/]");
            return;
        }
        Console.Confirm($"Moved {direction} to {Movement.Pos}");
    }

    public void Pick(Guid skillId)
    {
        if (!Skill.Get(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!CurrentCell.HasSkill(skillId))
        {
            // User Error: skill is not available in the current cell
            Console.Error($"Error: {skill.Name} is not available in the current room");
            return;
        }

        CurrentCell.Take(skillId);
        MyBag.Add(skillId);
        Console.Confirm($"Picked {skill.Name}.");
    }

    public void Drop(Guid skillId)
    {
        if (!Skill.Get(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!MyBag.HasSkill(skillId))
        {
            // User Error: skill is not available in my bag
            Console.Error($"Error: {skill.Name} is not available in my bag.");
            return;
        }

        MyBag.Drop(skillId);
        CurrentCell.Add(skillId);
        Console.Confirm($"Dropped {skill.Name}.");
    }

    public void ShowBagSkills()
    {
        var list = MyBag.GetSkills();
        Console.NormalText($"Current Bag Skills: {string.Join('\n', list)}");
    }

    public void ShowCellDetails()
    {
        Console.NormalText("Current Cell: ");
        CurrentCell.ShowInfo();
    }

    public List<Skill> GetBagSkills()
        => MyBag.GetSkills();

    public List<Skill> GetCellSkills()
        => CurrentCell.GetSkills();

    public void Restart()
    {

    }

    public void Exit()
        => IsRunning = false;

}