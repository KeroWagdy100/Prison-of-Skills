using System.Diagnostics.CodeAnalysis;

namespace zelda_game;

public abstract class Item
{
    protected static readonly Dictionary<Guid, Item> allItems = [];

    public Guid ID {get;} = Guid.NewGuid();
    public string Name {get; set;} = "Item";

    public static bool Get(Guid itemId, [MaybeNullWhen(false)] out Item item)
    {
        if (!allItems.TryGetValue(itemId, out item))
            return false;
        return item is not null;
    }
    public static bool Get(string itemName, [MaybeNullWhen(false)] out Item item)
    {
        item = null;
        if (!allItems.Any(pair => string.Equals(pair.Value.Name, itemName, StringComparison.OrdinalIgnoreCase)))
            return false;
        item = allItems.First(pair => string.Equals(pair.Value.Name, itemName, StringComparison.OrdinalIgnoreCase)).Value;
        return item is not null;
    }
}

public class Key: Item
{
    public static Guid CreateKey(string keyName)
    {
        Key key = new() { Name = keyName };
        allItems.Add(key.ID, key);
        return key.ID;
    }

    public static bool Get(Guid keyId, [MaybeNullWhen(false)] out Key key)
    {
        key = null;
        if (Get(keyId, out Item? item))
        {
            key = item as Key;
            return key is not null;
        }
        return false;
    }

    public static bool Get(string keyName, [MaybeNullWhen(false)] out Key key)
    {
        key = null;
        if (Get(keyName, out Item? item))
        {
            key = item as Key;
            return key is not null;
        }
        return false;
    }
}

public class Weapon: Item
{
    public int Price {get; set;} = 0;
    public static Guid CreateWeapon(string weaponName, int price)
    {
        Weapon weapon = new() { Name = weaponName, Price = price};
        allItems.Add(weapon.ID, weapon);
        return weapon.ID;
    }

    public static bool Get(Guid weaponId, [MaybeNullWhen(false)] out Weapon weapon)
    {
        weapon = null;
        if (Get(weaponId, out Item? item))
        {
            weapon = item as Weapon;
            return weapon is not null;
        }
        return false;
    }

    public static bool Get(string weaponName, [MaybeNullWhen(false)] out Weapon weapon)
    {
        weapon = null;
        if (Get(weaponName, out Item? item))
        {
            weapon = item as Weapon;
            return weapon is not null;
        }
        return false;
    }

}