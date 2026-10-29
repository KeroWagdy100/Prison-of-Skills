using System.Security.Cryptography;
using Spectre.Console;
using prison_of_skills;

Game Game = new();
Game.Init("json/Game.json");
Game.Run();