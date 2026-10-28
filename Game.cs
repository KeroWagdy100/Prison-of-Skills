using System.Text.Json;

namespace zelda_game;

public class Game
{
    private bool IsRunning { get; set; } = false;
    private readonly Player Player = new();

    private Cell[,] Cells { get; set; } = new Cell[1, 1];
    private Movement Movement { get; set; } = new(1, 1);
    private Inventory MyBag { get; set; } = new();

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

        while (IsRunning)
        {
            string? line = Console.ReadLine();
            if (line is null)
            {
                Exit();
                break;
            }

            Handler.Handle(line);
        }
    }

    public void Move(Direction direction)
    {
        if (!Movement.MoveTo(direction))
        {
            // Error Message: out of map bounds
            Console.WriteLine($"Cannot move [{direction}]");
            return;
        }
        Console.WriteLine($"Moved to {Movement.Pos}");
    }

    public void Pick(Guid skillId)
    {
        if (!Skill.Get(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!CurrentCell.HasSkill(skillId))
        {
            // User Error: skill is not available in the current cell
            Console.WriteLine($"Error: {skill.Name} is not available in the current room");
            return;
        }

        CurrentCell.Take(skillId);
        MyBag.Add(skillId);
        Console.WriteLine($"Picked {skill.Name}.");
    }

    public void Drop(Guid skillId)
    {
        if (!Skill.Get(skillId, out Skill? skill))
            throw new ArgumentException($"SkillId {skillId} is not a valid Skill ID");

        if (!MyBag.HasSkill(skillId))
        {
            // User Error: skill is not available in my bag
            Console.WriteLine($"Error: {skill.Name} is not available in my bag.");
            return;
        }

        MyBag.Drop(skillId);
        CurrentCell.Add(skillId);
        Console.WriteLine($"Dropped {skill.Name}.");
    }

    public void ShowInventory()
    {
        Console.WriteLine("Current Inventory Skills: ");
        MyBag.ShowSkills();
    }

    public void ShowCellDetails()
    {
        Console.WriteLine("Current Cell: ");
        CurrentCell.ShowInfo();
    }


    public void Exit()
        => IsRunning = false;

}