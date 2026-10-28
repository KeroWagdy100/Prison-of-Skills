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
            Console.Error($"Failed to load game data ({filename} is not well formatted)");
            Console.Error($"Please check and validate Game DTO structure and try again.");
            Logger.Error(ex.ToString());
        }

        movement = new(gameDto.Rows, gameDto.Cols);

        LoadSkills(gameDto);
        LoadObstacles(gameDto);
        LoadCells(gameDto, out cells);

        LoadIntroText(gameDto.IntroPath);
        LoadCommandsText(gameDto.CommandsPath);
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
                Logger.Success(
                    $"Loaded SKill {skill.Name}, Type: {skill.Type}",
                    $"Time Cost: {skill.TimeCost}m, Strength: {skill.Strength}\n"
                );
            }
            else
            {
                Logger.Error($"Could not load skill with name: {dto.Skill}");
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
                Logger.Success(
                    $"Loaded Obstacle {obs.Name}, Time Cost: {obs.TimeCost}m",
                    $"Defeated by: {obs.DefeatedBy}\n"
                );
            }
            else
            {
                Logger.Error($"Could not load obstacle with name: {dto.Obstacle}");
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
            Logger.Success($"Desc: {room.Description}");

            List<Guid> obstaclesIds = [];
            foreach (string obsName in dto.Obstacles ?? [])
            {
                if (Obstacle.Get(obsName, out Obstacle? obs))
                {
                    obstaclesIds.Add(obs.ID);
                    Logger.Success($"Added obstacle {obsName}.");
                }
                else
                    Logger.Error($"obstacle {obsName} is not found!");
            }


            foreach (string skillName in dto.Skills ?? [])
            {
                if (Skill.Get(skillName, out Skill? skill))
                {
                    room.Add(skill.ID);
                    Logger.Success($"Added Skill: {skill.Name}");
                }
                else
                    Logger.Error($"Skill {skillName} is not found!");
            }
        }
        Logger.Log($"---- ------ ----");
    }


    private static void LoadIntroText(string introPath)
    {
        if (!File.Exists(introPath))
            throw new FileNotFoundException($"Intro Path {introPath} does not exist. Please ensure the file exists.");
        Console.AddStaticText("intro", File.ReadAllText(introPath));
    }

    private static void LoadCommandsText(string commandsPath)
    {
        if (!File.Exists(commandsPath))
            throw new FileNotFoundException($"Commands Path {commandsPath} does not exist. Please ensure the file exists.");

        Console.AddStaticText("commands", File.ReadAllText(commandsPath));
    }
}