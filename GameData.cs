namespace zelda_game;

public record GameDto
{
    public int Rows { get; set; }
    public int Cols { get; set; }

    public List<SkillDto> Skills { get; set; } = [];
    public List<ObstacleDto> Obstacles { get; set; } = [];
    public List<CellDto> Cells { get; set; } = [];
}

public record SkillDto
{
    public string Skill { get; set; } = "";
    public int Cost { get; set; }
    public int Strength { get; set; }
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
}

public record ObstacleDto
{
    public string Obstacle { get; set; } = "";
    public int TimeCost { get; set; }
    public string DefeatedBy { get; set; } = "";
    public int? RewardCoins { get; set; } = 0;
}

public class CellDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public List<string>? Skills { get; set; }
    public List<string>? Obstacles { get; set; }
    public string Description { get; set; } = "";
}