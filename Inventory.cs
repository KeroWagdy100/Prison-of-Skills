namespace zelda_game;

public class Inventory
{
    private readonly List<Guid> ItemIds = [];

    public bool Drop(Guid itemId)
        => ItemIds.Remove(itemId);

    public void Add(Guid itemId)
        => ItemIds.Add(itemId);

    public bool HasItem(Guid itemId)
        => ItemIds.Contains(itemId);

    public void ShowItems()
    {
        foreach (var itemId in ItemIds)
        {
            if (!Item.Get(itemId, out Item? item))
                continue;

            Console.WriteLine($"{item.Name}");
            if (item is Key)
                Console.WriteLine($"(Key)");
            else if (item is Weapon)
            {
                Weapon weapon = (item as Weapon)!;
                Console.WriteLine($"Price: {weapon.Price}");
            }
        }
    }
}