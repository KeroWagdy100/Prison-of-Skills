namespace zelda_game;

public record GameDto
{
    public int Width { get; set; }
    public int Height { get; set; }

    public List<ItemDto> Items { get; set; } = [];
    public List<MonsterDto> Monsters { get; set; } = [];
    public List<RoomDto> Rooms { get; set; } = [];
}

public record ItemDto
{
    public string ItemName { get; set; } = "";
    public string ItemType { get; set; } = "";
    public int Price { get; set; }
}

public record MonsterDto
{
    public string MonsterName { get; set; } = "";
    public List<string> RequiredWeapons { get; set; } = [];
    public string? RewardItem { get; set; } = "";
    public int? RewardCoins { get; set; } = 0;
}

public class RoomDto
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public string Description { get; set; } = "";
    public List<string>? Items { get; set; } = [];
    public string? RequiredKey { get; set; } = "";
    public int? Coins { get; set; } = 0;
    public string? Monster { get; set; } = "";
    public bool HasWitch { get; set; } = false;
}