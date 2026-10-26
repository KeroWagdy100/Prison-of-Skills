using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace zelda_game;

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public struct Position
{
    public int x, y;

    public readonly override string ToString()
    {
        return $"{{{x}, {y}}}";
    }
}


public class Movement(int BoundWidth, int BoundHeight, Position initalPosition = default)
{
    private readonly static int[] dx = [-1, 1, 0, 0];
    private readonly static int[] dy = [0, 0, -1, 1];

    public Position Pos { get; private set; } = initalPosition;
    public int Width { get; set; } = BoundWidth;
    public int Height { get; set; } = BoundHeight;


    public bool MoveTo(Direction direction)
    {
        int nx = Pos.x + dx[(int)direction];
        int ny = Pos.y + dy[(int)direction];
        if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
            return false;

        Pos = new Position{x = nx, y = ny};
        return true;
    }
}
