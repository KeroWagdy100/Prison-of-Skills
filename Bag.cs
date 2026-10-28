using System.Data.Common;

namespace zelda_game;

public class Bag
{
    private readonly List<Guid> _skillIds = [];

    public bool Drop(Guid skillId)
        => _skillIds.Remove(skillId);

    public void Add(Guid skillId)
        => _skillIds.Add(skillId);

    public bool HasSkill(Guid skillId)
        => _skillIds.Contains(skillId);

    public List<Skill> GetSkills()
    {
        List<Skill> listOfSkills = [];
        foreach (Guid id in _skillIds)
        {
            if (Skill.Get(id, out Skill? skill))
                listOfSkills.Add(skill);
            else
                throw new Exception($"Bag contains invalid skill ID");
        }
        return listOfSkills;
    }
}