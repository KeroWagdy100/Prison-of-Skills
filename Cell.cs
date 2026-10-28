namespace zelda_game;

public class Cell
{
    private readonly Inventory<Skill> _skills = new();
    private readonly Inventory<Obstacle> _obstacles = new();

    public bool TakeSkill(Guid skillId)
        => _skills.Drop(skillId);

    public void AddSkill(Guid skillId)
        => _skills.Add(skillId);

    public void AddObstacle(Guid obstacleId)
        => _obstacles.Add(obstacleId);
    

    public void ShowInfo()
    {
        Console.NormalText(_skills.ToString());
    }

    public bool HasSkill(Guid skillId)
        => _skills.HasItem(skillId);

    public List<Skill> GetSkills()
        => _skills.GetItems();

    public List<Obstacle> GetObstacles()
        => _obstacles.GetItems();

    public string Description { get; set; } = "";
}