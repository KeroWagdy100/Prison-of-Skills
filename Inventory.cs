namespace zelda_game;

public class Inventory
{
    private readonly List<Guid> SkillIds = [];

    public bool Drop(Guid skillId)
        => SkillIds.Remove(skillId);

    public void Add(Guid skillId)
        => SkillIds.Add(skillId);

    public bool HasSkill(Guid skillId)
        => SkillIds.Contains(skillId);

    public void ShowSkills()
    {
        foreach (var skillId in SkillIds)
        {
            if (!Skill.Get(skillId, out Skill? skill))
            {
                Logger.Log($"SkillId {skillId} is not found!");
                continue;
            }

            Console.NormalText($"{skill.Name} of type {skill.Type} costs {skill.TimeCost}m and gives {skill.Strength} points.");
        }
    }
}