using System.Text.Json;
namespace zelda_game;

public class GameLoader
{
    public static void LoadGame(string filename, out Room[,] rooms, out Movement movement)
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

        movement = new(gameDto.Width, gameDto.Height);

        LoadItems(gameDto);
        LoadMonsters(gameDto);
        LoadRooms(gameDto, out rooms);
    }

    private static void LoadItems(GameDto gameDto)
    {
        Logger.Log($"---- Loading Items ----");
        foreach (ItemDto itemDto in gameDto.Items)
        {
            switch (itemDto.ItemType)
            {
                case "Weapon": 
                    Weapon.CreateWeapon(itemDto.ItemName, itemDto.Price);
                    Logger.Log($"Loaded Weapon {itemDto.ItemName}");
                break;
                case "Key":
                    Key.CreateKey(itemDto.ItemName);
                    Logger.Log($"Loaded Key {itemDto.ItemName}");
                break;
            }
        }
        Logger.Log($"---- ------ ----");
    }

    private static void LoadMonsters(GameDto gameDto)
    {
        Logger.Log($"---- Loading Monsters ----");
        foreach (MonsterDto dto in gameDto.Monsters)
        {
            List<Guid> requiredWeapons = [];
            foreach (string weaponName in dto.RequiredWeapons)
                if (Weapon.Get(weaponName, out Weapon? weapon))
                    requiredWeapons.Add(weapon.ID);

            Item? rewardItem = null;
            if (dto.RewardItem != null)
                Item.Get(dto.RewardItem ?? "", out rewardItem);

            Monster.CreateMonster(
                dto.MonsterName,
                requiredWeapons,
                rewardItem?.ID,
                dto.RewardCoins ?? 0
            );

            Logger.Log($"Loaded Monster {dto.MonsterName}, Reward item: {rewardItem?.ID}, Reward Coins: {dto.RewardCoins}");
            Logger.Log($"Needed Weapons: {string.Join('-', requiredWeapons)}\n");
        }
        Logger.Log($"---- ------ ----");
    }

    private static void LoadRooms(GameDto gameDto, out Room[,] rooms)
    {
        Logger.Log($"---- Loading Rooms ----");
        rooms = new Room[gameDto.Height, gameDto.Width];

        foreach (RoomDto roomDto in gameDto.Rooms)
        {
            Logger.Log($"Loading Room at {roomDto.PosX}, {roomDto.PosY}");

            Room room = rooms[roomDto.PosX, roomDto.PosY] = new Room();
            room.Coins = roomDto.Coins ?? 0;
            room.Description = roomDto.Description;
            room.HasWitch = roomDto.HasWitch;
            
            if (roomDto.RequiredKey != null && Key.Get(roomDto.RequiredKey, out Key? key))
                room.RequiredKey = key.ID;

            if (roomDto.Monster != null && Monster.Get(roomDto.Monster, out Monster? monster))
                room.Monster = monster.ID;

            // Adding room items
            foreach (string itemName in roomDto.Items ?? [])
            {
                if (Item.Get(itemName, out Item? item))
                {
                    room.Add(item.ID);
                    Logger.Log($"Added Item: {item.Name}");
                }
            }
            
            Logger.Log($"Coins: {room.Coins}");
            Logger.Log($"Desc: {room.Description}");
            Logger.Log($"HasWitch: {room.HasWitch}");
            Logger.Log($"RequiredKey: {room.RequiredKey}");
            Logger.Log($"Monster: {room.Monster}\n");
        }
        Logger.Log($"---- ------ ----");
    }

}