using System.Diagnostics.CodeAnalysis;
namespace zelda_game;

public enum SkillType
{
    LANGUAGE,
    DSA,
    DATABASE,
    TECHNOLOGY,
    GENERIC,
    EXTRA
}


public class Skill : Item
{

    public SkillType Type { get; set; } = SkillType.GENERIC;
    public int TimeCost { get; set; }
    public int Strength { get; set; }

    public static Guid CreateSkill(string skillName, SkillType type, int costInMonths, int strength)
    {
        Skill skill = new()
        {
            Name = skillName,
            Type = type,
            TimeCost = costInMonths,
            Strength = strength
        };

        allItems.Add(skill.ID, skill);
        return skill.ID;
    }

    private Skill() { }

    public override string ToString()
        => $"[bold white on black]{Name}[/] of type [italic]{Type}[/], costs [bold blue]{TimeCost} months[/] and adds [bold green]{Strength} points[/].";
}