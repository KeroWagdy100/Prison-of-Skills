namespace zelda_game;

public class Cell
{
    private readonly Bag _bag = new();

    public bool Take(Guid skillId)
        => _bag.Drop(skillId);

    public void Add(Guid skillId)
        => _bag.Add(skillId);

    public void ShowInfo()
    {
        Console.NormalText($"Skills:\n{string.Join('\n', _bag.GetSkills())}");
    }

    public bool HasSkill(Guid skillId)
        => _bag.HasSkill(skillId);

    public List<Skill> GetSkills()
        => _bag.GetSkills();

    public string Description { get; set; } = "";
    public Guid? Obstacle { get; set; }
}