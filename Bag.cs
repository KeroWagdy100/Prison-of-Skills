
namespace zelda_game;

public class Bag : Inventory<Skill>
{
    public int TotalCost { get; set; } = 0;
    public int TotalPoints { get; set; } = 0;

    public override bool Drop(Guid skillId)
    {
        // TODO: Unnecessary second check
        if (!Item.TryGet<Skill>(skillId, out Skill? skill))
            return false;

        if (base.Drop(skillId))
        {
            TotalCost -= skill.TimeCost;
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
        TotalCost += skill.TimeCost;
        TotalPoints += skill.Strength;
    }

    public override string ToString()
    {
        var list = GetItems();
        if (list.Count == 0)
            return $"Bag of skills is currently empty :pleading_face:";
        else
            return
              $"Bag of Skills:\n- {string.Join("\n- ", list)}\n"
            + $"\t[grey]Total time cost: [blue bold]{TotalCost} months[/]"
            + $", [italic]You have [white bold]{4 * 12 - TotalCost} months[/] remaning.[/][/]\n"
            + $"\t[grey]You currently have [bold green]{TotalPoints} points[/][/]";
    }
}