using System.Security.Cryptography;
using Spectre.Console;
using zelda_game;

Game Game = new();
Game.Init("json/Game.json");
Game.Run();