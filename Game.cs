using System.Runtime.InteropServices;
using System.Text.Json;

namespace zelda_game;

public class Game
{
    private bool IsRunning { get; set; } = false;
    private readonly Player Player = new();

    private List<Item> Items { get; set; } = [];
    private List<Room> Rooms { get; set; } = [];

    readonly private CommandHandler Handler;
    readonly private Movement Movement;

    // -----------------------------------------
    // Initializes the game with the given filename
    public Game(string filename)
    {
        Handler = new(this);
        Movement = new();

        Logger.Clear();
        LoadGame(filename);
    }

    private void LoadGame(string filename)
    {
        if (!File.Exists(filename))
            throw new ArgumentException($"{filename} does not exist. Please ensure the file exists.");

        GameDto gameDto = new();
        try
        {
            string s = File.ReadAllText(filename);
            gameDto = JsonSerializer.Deserialize<GameDto>(s) ?? throw new InvalidDataException();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game data ({filename} is not well formatted)");
            Console.WriteLine($"Please check and validate Game DTO structure and try again.");
            Logger.Log(ex);
        }

        Movement.Width = gameDto.Width;
        Movement.Height = gameDto.Height;
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