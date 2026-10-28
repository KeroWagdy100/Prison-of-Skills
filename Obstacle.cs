using System.Diagnostics.CodeAnalysis;
namespace zelda_game;

public class Obstacle: Item
{
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

}