using System.Diagnostics.CodeAnalysis;
namespace zelda_game;

public record Monster
{
    public Guid ID {get;} = Guid.NewGuid();
    public string Name { get; private set; } = "";
    public List<Guid> DefeatedBy { get; private set; } = [];
    public Guid? RewardItem { get; private set; }
    public int RewardCoins { get; private set; } = 0;


    private static readonly Dictionary<Guid, Monster> allMonsters = [];

    public static Guid CreateMonster(
        string monsterName,
        List<Guid> defeatedByWeaponsIds,
        Guid? rewardItem,
        int rewardCoins = 0
        )
    {
        Monster monster = new() {
            Name = monsterName,
            DefeatedBy = defeatedByWeaponsIds,
            RewardItem = rewardItem,
            RewardCoins = rewardCoins};
        
        allMonsters.Add(monster.ID, monster);
        return monster.ID;
    }

    public static bool Get(Guid monsterId, [MaybeNullWhen(false)] out Monster monster)
    {
        if (!allMonsters.TryGetValue(monsterId, out monster))
            return false;
        return monster is not null;
    }
    public static bool Get(string monsterName, [MaybeNullWhen(false)] out Monster monster)
    {
        monster = null;
        if (!allMonsters.Any(pair => pair.Value.Name == monsterName))
            return false;
        monster = allMonsters.First(pair => pair.Value.Name == monsterName).Value;
        return monster is not null;
    }
}