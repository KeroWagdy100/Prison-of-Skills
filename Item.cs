using System.Diagnostics.CodeAnalysis;
namespace prison_of_skills;


public class Item
{
    protected static readonly Dictionary<Guid, Item> allItems = [];

    public Guid ID { get; } = Guid.NewGuid();
    public string Name { get; set; } = "Item";

    public static bool Exists(Guid guid)
        => allItems.ContainsKey(guid);

    public static bool Get(Guid itemId, [MaybeNullWhen(false)] out Item item)
    {
        allItems.TryGetValue(itemId, out item);
        return item is not null;
    }

    public static bool Get(string name, [MaybeNullWhen(false)] out Item item)
    {
        item = null;
        bool matchingPairByName(KeyValuePair<Guid, Item> pair)
            => string.Equals(pair.Value.Name, name, StringComparison.OrdinalIgnoreCase);

        if (!allItems.Any(matchingPairByName))
            return false;
        item = allItems.First(matchingPairByName).Value;
        return item is not null;
    }

    // For inherited types of Item
    public static bool TryGet<T>(Guid itemId, [MaybeNullWhen(false)] out T item) where T : Item
    {
        if (Get(itemId, out Item? temp) && temp is T typedItem)
        {
            item = typedItem;
            return true;
        }

        item = null;
        return false;
    }

    // For inherited types of Item
    public static bool TryGet<T>(string name, [MaybeNullWhen(false)] out T item) where T : Item
    {
        if (Get(name, out Item? temp) && temp is T typedItem)
        {
            item = typedItem;
            return true;
        }

        item = null;
        return false;
    }
}