using System.Text.Json;

namespace zelda_game;

public class Game
{
    private bool IsRunning { get; set; } = false;
    private readonly Player Player = new();

    private  Room[,] Rooms { get; set; } = new Room[1,1];
    private Movement Movement = new(1, 1);

    readonly private CommandHandler Handler;

    // -----------------------------------------
    // Initializes the game with the given filename
    public Game()
    {
        Handler = new(this);
        Logger.Clear();
    }

    public void Init(string filename)
    {
        GameLoader.LoadGame(filename, out var rooms, out var movement);
        Rooms = rooms;
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


    public void Exit()
    {
        IsRunning = false;
    }

}