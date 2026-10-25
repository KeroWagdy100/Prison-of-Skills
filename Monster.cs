namespace zelda_game;

public record Monster
{
    public Guid MonsterId { get; } = new();
    public string Name { get; set; } = "";
    public List<Weapon> DefeatedBy { get; set; } = [];
    public Guid? RewardItem { get; set; }
    public int RewardCoins { get; set; } = 0;
}