namespace zelda_game;

public class Room
{
    private readonly Inventory _inventory = new();
    private int _coins = 0;

    public bool Take(Guid itemId)
        => _inventory.Drop(itemId);

    public void Add(Guid itemId)
        => _inventory.Add(itemId);

    public int Coins
    {
        get
        {
            return _coins;
        }

        set
        {
            if (value < 0) throw new ArgumentException("Coins cannot be -ve");
            _coins = value;
        }
    }
    public void ShowInfo()
    {
        Console.WriteLine($"Items: ");
        _inventory.ShowItems();
        Console.WriteLine($"Coins: {_coins}");
    }

    public bool HasItem(Guid itemId)
        => _inventory.HasItem(itemId);


    public string Description { get; set; } = "";
    public Guid? RequiredKey { get; set; }
    public bool HasWitch { get; set; } = false;
    public Guid? Monster { get; set; }
}