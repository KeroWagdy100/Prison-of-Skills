
namespace prison_of_skills;

public class Bag : Inventory<Skill>
{
    public int SkillCost { get; set; } = 0;
    public int TimeWasted { get; set; } = 0;

    public int TotalCost => SkillCost + TimeWasted;
    public int TotalPoints { get; set; } = 0;

    public override bool Drop(Guid skillId)
    {
        // TODO: Unnecessary second check
        if (!Item.TryGet<Skill>(skillId, out Skill? skill))
            return false;

        if (base.Drop(skillId))
        {
            SkillCost -= skill.TimeCost;
            TotalPoints -= skill.Strength;
            return true;
        }
        return false;
    }

    public override void Add(Guid skillId)
    {
        // TODO: Unnecessary second check
        if (!Item.TryGet<Skill>(skillId, out Skill? skill))
            return;

        base.Add(skillId);
        SkillCost += skill.TimeCost;
        TotalPoints += skill.Strength;
    }

    public override string ToString()
    {
        var list = GetItems();
        string costForSkills = $"[grey]Skills took: [blue bold]{SkillCost} months[/][/]";
        string wastedTime = $"[grey]Time Wasted: [red bold]{TimeWasted} months[/][/]";
        string remainingTime = $"[grey italic]You have [white bold]{4 * 12 - TotalCost} months[/] remaning.[/]";
        string currenPoints = $"[grey]You currently have [bold green]{TotalPoints} points[/][/]";

        if (list.Count == 0)
            return (TimeWasted == 0 ? "" : wastedTime + "\n") + $"Bag of skills is currently empty :pleading_face:";
        return
            $"Bag of Skills:\n- {string.Join("\n- ", list)}\n"
        + $"\t{costForSkills}{(TimeWasted == 0 ? "" : ", " + wastedTime)}\n\t{remainingTime}\n\t{currenPoints}";
    }
}