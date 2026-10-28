using System.Diagnostics;
using System.Text.Json;
namespace zelda_game;

public class GameLoader
{
    public static void LoadGame(string filename, out Cell[,] cells, out Movement movement)
    {        
        if (!File.Exists(filename))
            throw new ArgumentException($"{filename} does not exist. Please ensure the file exists.");

        GameDto gameDto = new();
        try
        {
            string s = File.ReadAllText(filename);
            gameDto = JsonSerializer.Deserialize<GameDto>(s) ?? throw new InvalidDataException();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game data ({filename} is not well formatted)");
            Console.WriteLine($"Please check and validate Game DTO structure and try again.");
            Logger.Log(ex);
        }

        movement = new(gameDto.Rows, gameDto.Cols);

        LoadSkills(gameDto);
        LoadObstacles(gameDto);
        LoadCells(gameDto, out cells);
    }

    private static void LoadSkills(GameDto gameDto)
    {
        Logger.Log($"---- Loading Skills ----");
        foreach (SkillDto dto in gameDto.Skills)
        {
            var id = Skill.CreateSkill (
                dto.Skill,
                Enum.Parse<SkillType>(dto.Type.ToUpper()), // TODO: Use TryParse to handle unexpected exceptions
                dto.Cost,
                dto.Strength
            );

            if (Skill.Get(id, out Skill? skill))
            {
                Logger.Log($"Loaded SKill {skill.Name}, Type: {skill.Type}m");
                Logger.Log($"Time Cost: {skill.TimeCost}m, Strength: {skill.Strength}\n");
            }
            else
            {
                Logger.Log($"Could not load skill with name: {dto.Skill}");
            }
        }
        Logger.Log($"---- ------ ----");
    }

    private static void LoadObstacles(GameDto gameDto)
    {
        Logger.Log($"---- Loading Obstacles ----");
        foreach (ObstacleDto dto in gameDto.Obstacles)
        {

            var id = Obstacle.Create(
                dto.Obstacle,
                Enum.Parse<SkillType>(dto.DefeatedBy.ToUpper()),
                dto.TimeCost
            );

            if (Obstacle.Get(id, out Obstacle? obs))
            {
                Logger.Log($"Loaded Obstacle {obs.Name}, Time Cost: {obs.TimeCost}m");
                Logger.Log($"Defeated by: {obs.DefeatedBy}\n");
            }
            else
            {
                Logger.Log($"Could not load obstacle with name: {dto.Obstacle}");
            }
        }
        Logger.Log($"---- ------ ----");
    }

    private static void LoadCells(GameDto gameDto, out Cell[,] cells)
    {
        Logger.Log($"---- Loading Cells ----");
        cells = new Cell[gameDto.Rows, gameDto.Cols];

        foreach (CellDto dto in gameDto.Cells)
        {
            Logger.Log($"Loading Cell at {dto.X}, {dto.Y}");

            Cell room = cells[dto.X, dto.Y] = new Cell();
            room.Description = dto.Description;
            Logger.Log($"Desc: {room.Description}");

            List<Guid> obstaclesIds = [];
            foreach (string obsName in dto.Obstacles ?? [])
            {
                if (Obstacle.Get(obsName, out Obstacle? obs))
                {
                    obstaclesIds.Add(obs.ID);
                    Logger.Log($"Added obstacle {obsName}.");
                }
                else
                    Logger.Log($"obstacle {obsName} is not found!");
            }


            foreach (string skillName in dto.Skills ?? [])
            {
                if (Skill.Get(skillName, out Skill? skill))
                {
                    room.Add(skill.ID);
                    Logger.Log($"Added Skill: {skill.Name}");
                }
                else
                    Logger.Log($"Skill {skillName} is not found!");
            }
        }
        Logger.Log($"---- ------ ----");
    }

}