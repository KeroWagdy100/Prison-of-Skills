# Prison of Skills

A small interactive terminal game written in C#/.NET. Explore a 3 × 4 map of “brain cells,” collect skills, avoid obstacles, and try to reach a backend internship interview with enough strength points and the required skill types.

## Game Rules

- Learn the game’s simple command interface through an interactive terminal experience, with colored output and selection menus powered by [Spectre.Console](https://spectreconsole.net/).
- Choose which skills to collect: each one adds strength and has a time cost.
- Obstacles cost time unless you already carry a skill of the matching type.
- The interview is passed with at least 300 strength points and skills in Language, Database, DSA, Generic, and Technology.

## Requirements

- .NET 10 SDK
- A terminal that supports interactive input and ANSI colors

## Get started

Clone the repository and run the game from its root directory. The game loads its scenario and story text using repository-relative paths, so keep the working directory at the project root.

```sh
git clone https://github.com/KeroWagdy100/Prison-of-Skills.git
cd Prison-of-Skills
dotnet run --project prison_of_skills.csproj
```

Alternatively, build and run the executable:

```sh
dotnet build prison_of_skills.sln
dotnet run --no-build --project prison_of_skills.csproj
```

The game starts at row 1, column 1. Enter commands at the `>` prompt; command names and options are case-insensitive. You can enter a command without its option to choose from an interactive menu.

```text
MOVE RIGHT
SHOW CELL
PICK SQLite
SHOW BAG
```

### Commands

| Command | Example | Purpose |
| --- | --- | --- |
| `MOVE <LEFT\|RIGHT\|UP\|DOWN>` | `MOVE RIGHT` | Move one cell. |
| `SHOW <CELL\|BAG\|COMMANDS\|INTRO>` | `SHOW BAG` | Display the current cell, your bag, command help, or introduction. |
| `PICK <skill name>` | `PICK SQLite` | Add a skill available in the current cell to your bag. |
| `DROP <skill name>` | `DROP SQLite` | Drop a skill from your bag into the current cell. |
| `CLEAR` | `CLEAR` | Clear the terminal. |
| `RESTART` | `RESTART` | Stop the current game; start it again from the shell to play again. |
| `EXIT` | `EXIT` | Quit. |

You can also type `MOVE`, `SHOW`, `PICK`, or `DROP` alone to use the built-in selection prompts. Movement is limited to the map. The final cell is row 3, column 4.

## Game data

The default scenario is in [`json/Game.json`](json/Game.json). Its skill, obstacle, and cell entries define the map and game content. The scenario also points to the introduction and command text in [`static_text/Introduction.txt`](static_text/Introduction.txt) and [`static_text/Commands.txt`](static_text/Commands.txt). Run the game from the repository root so these relative paths resolve.

## Help and documentation

The in-game `SHOW COMMANDS` and `SHOW INTRO` commands display the included help and story. For bugs or questions, open an issue in the [GitHub repository](https://github.com/KeroWagdy100/Prison-of-Skills/issues).

## Maintainers and contributing

The project is maintained by [Kero Wagdy](https://github.com/KeroWagdy100). Contributions are welcome: open an issue to discuss a change, then submit a pull request with a clear description. Keep changes focused and include steps to reproduce or review them where useful.
