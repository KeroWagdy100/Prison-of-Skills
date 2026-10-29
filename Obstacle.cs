using System.Diagnostics.CodeAnalysis;
namespace zelda_game;

public class Obstacle: Item
{
    public SkillType DefeatedBy { get; private set; }
    public int TimeCost { get; private set; }
    public string TrapText {get; private set;} = "";
    public string PassedText {get; private set;} = "";

    // private static readonly Dictionary<Guid, Obstacle> allObstacles = [];

    public static Guid Create(
        string obstacleName,
        SkillType defeatedBySkillType,
        int timeCost,
        string trapText,
        string passedText
        )
    {
        Obstacle obstacle = new() {
            Name = obstacleName,
            DefeatedBy = defeatedBySkillType,
            TimeCost = timeCost,
            TrapText = trapText,
            PassedText = passedText
            };
        
        allItems.Add(obstacle.ID, obstacle);
        return obstacle.ID;
    }

}