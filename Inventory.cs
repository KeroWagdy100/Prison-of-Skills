namespace zelda_game;

public class Inventory
{
    private readonly List<Guid> ItemIds = [];

    public bool Drop(Guid droppedItemId)
        => ItemIds.Remove(droppedItemId);

    public void Add(Guid itemId)
        => ItemIds.Add(itemId);
}