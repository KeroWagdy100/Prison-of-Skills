using System.Text.Json;

namespace zelda_game;

public class Game
{
    private bool IsRunning { get; set; } = false;
    private readonly Player Player = new();

    private  Room[,] Rooms { get; set; } = new Room[1,1];
    private Movement Movement {get; set;} = new(1, 1);
    private Inventory MyBag {get; set;} = new();
    
    public Room CurrentRoom => Rooms[Movement.Pos.x, Movement.Pos.y];

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

    public void Pick(Guid itemId)
    {
        if (!Item.Get(itemId, out Item? item))
            throw new ArgumentException($"ItemId {itemId} is not a valid Item ID");

        if (!CurrentRoom.HasItem(itemId))
        {
            // User Error: item is not available in the current room
            Console.WriteLine($"Error: {item.Name} is not available in the current room");
            return;
        }

        CurrentRoom.Take(itemId);
        MyBag.Add(itemId);
        Console.WriteLine($"Picked {item.Name}.");
    }

    public void Drop(Guid itemId)
    {
        if (!Item.Get(itemId, out Item? item))
            throw new ArgumentException($"ItemId {itemId} is not a valid Item ID");

        if (!MyBag.HasItem(itemId))
        {
            // User Error: item is not available in my bag
            Console.WriteLine($"Error: {item.Name} is not available in my bag.");
            return;
        }

        MyBag.Drop(itemId);
        CurrentRoom.Add(itemId);
        Console.WriteLine($"Dropped {item.Name}.");
    }

    public void ShowInventory()
    {
        Console.WriteLine("Current Inventory Items: ");
        MyBag.ShowItems();
    }

    public void ShowRoomDetails()
    {
        Console.WriteLine("Current Room: ");
        CurrentRoom.ShowInfo();
    }


    public void Exit()
        => IsRunning = false;

}