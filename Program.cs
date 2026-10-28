using System.Security.Cryptography;
using Spectre.Console;
using zelda_game;

Game Game = new();
Game.Init("Game.json");
await Game.Run();