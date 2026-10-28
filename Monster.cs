using System.Diagnostics.CodeAnalysis;
namespace zelda_game;

public class Obstacle
{
    public Guid ID {get;} = Guid.NewGuid();
    public string Name { get; private set; } = "";
    public SkillType DefeatedBy { get; private set; }
    public int TimeCost { get; private set; }


    private static readonly Dictionary<Guid, Obstacle> allObstacles = [];

    public static Guid Create(
        string obstacleName,
        SkillType defeatedBySkillType,
        int timeCost
        )
    {
        Obstacle obstacle = new() {
            Name = obstacleName,
            DefeatedBy = defeatedBySkillType,
            TimeCost = timeCost};
        
        allObstacles.Add(obstacle.ID, obstacle);
        return obstacle.ID;
    }

    public static bool Get(Guid obstacleId, [MaybeNullWhen(false)] out Obstacle obstacle)
    {
        if (!allObstacles.TryGetValue(obstacleId, out obstacle))
            return false;
        return obstacle is not null;
    }
    public static bool Get(string obstacleName, [MaybeNullWhen(false)] out Obstacle obstacle)
    {
        obstacle = null;
        if (!allObstacles.Any(pair => string.Equals(pair.Value.Name, obstacleName, StringComparison.OrdinalIgnoreCase)))
            return false;

        obstacle = allObstacles.First(pair =>
            string.Equals(pair.Value.Name, obstacleName, StringComparison.OrdinalIgnoreCase)
        ).Value;

        return obstacle is not null;
    }
}