namespace zelda_game;

public class Cell
{
    private readonly Inventory<Skill> _skills = new();
    private readonly Inventory<Obstacle> _obstacles = new();

    public bool Take(Guid skillId)
        => _skills.Drop(skillId);

    public void Add(Guid skillId)
        => _skills.Add(skillId);

    public void ShowInfo()
    {
        Console.NormalText(_skills.ToString());
    }

    public bool HasSkill(Guid skillId)
        => _skills.HasItem(skillId);

    public List<Skill> GetSkills()
        => _skills.GetItems();

    public string Description { get; set; } = "";
    public Guid? Obstacle { get; set; }
}