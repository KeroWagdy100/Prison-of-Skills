namespace zelda_game;

public class Cell
{
    private readonly Inventory _inventory = new();

    public bool Take(Guid skillId)
        => _inventory.Drop(skillId);

    public void Add(Guid skillId)
        => _inventory.Add(skillId);

    public void ShowInfo()
    {
        Console.WriteLine($"Skills: ");
        _inventory.ShowSkills();
    }

    public bool HasSkill(Guid skillId)
        => _inventory.HasSkill(skillId);


    public string Description { get; set; } = "";
    public Guid? Obstacle { get; set; }
}