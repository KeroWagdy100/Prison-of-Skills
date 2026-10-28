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


public class Skill
{
    protected static readonly Dictionary<Guid, Skill> allSkills = [];

    public Guid ID {get;} = Guid.NewGuid();
    public string Name {get; set;} = "Skill";
    public SkillType Type {get; set;} = SkillType.GENERIC;
    public int TimeCost {get; set;}
    public int Strength {get; set;}

    public static bool Get(Guid skillId, [MaybeNullWhen(false)] out Skill skill)
    {
        if (!allSkills.TryGetValue(skillId, out skill))
            return false;
        return skill is not null;
    }

    public static bool Get(string skillName, [MaybeNullWhen(false)] out Skill skill)
    {
        skill = null;
        if (!allSkills.Any(pair => string.Equals(pair.Value.Name, skillName, StringComparison.OrdinalIgnoreCase)))
            return false;
        skill = allSkills.First(pair => string.Equals(pair.Value.Name, skillName, StringComparison.OrdinalIgnoreCase)).Value;
        return skill is not null;
    }

    public static Guid CreateSkill(string skillName, SkillType type, int costInMonths, int strength)
    {
        Skill skill = new()
        {
            Name = skillName,
            Type = type,
            TimeCost = costInMonths,
            Strength = strength
        };

        allSkills.Add(skill.ID, skill);
        return skill.ID;
    }

    private Skill() {}
}