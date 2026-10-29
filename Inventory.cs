namespace prison_of_skills;

// A generic container for any type of items
public class Inventory<T> where T : Item
{
    private readonly List<Guid> _itemIds = [];

    public virtual bool Drop(Guid itemId)
    {
        if (!Item.Exists(itemId))
            throw new ArgumentException($"{itemId} is not a valid {nameof(T)} id");

        return _itemIds.Remove(itemId);
    }

    public virtual void Add(Guid itemId)
    {
        if (!Item.Exists(itemId))
            throw new ArgumentException($"{itemId} is not a valid {nameof(T)} id");

        _itemIds.Add(itemId);
    }

    public bool HasItem(Guid itemId)
        => _itemIds.Contains(itemId);

    public List<T> GetItems()
    {
        List<T> listOfItems = [];
        foreach (Guid id in _itemIds)
        {
            if (Item.TryGet<T>(id, out T? typedItem))
                listOfItems.Add(typedItem);
            else
                throw new Exception($"Bag contains invalid skill ID");
        }
        return listOfItems;
    }

    public override string ToString()
    {
        var items = GetItems();

        return items.Count == 0 ? "[dim italic]Empty[/]" : $"Items:\n- {string.Join("\n- ", items)}";
    }
}
