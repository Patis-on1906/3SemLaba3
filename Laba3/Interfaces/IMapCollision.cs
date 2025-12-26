namespace Laba3;

public interface IMapCollision
{
    bool IsWalkable(int x, int y);
    bool IsWithinBounds(int x, int y);
    int Width { get; }
    int Height { get; }
}