namespace Laba3;

public abstract class MovingUnit : IEntity, IMoveable 
{
    public int X { get; set; }
    public int Y { get; set; }
    public abstract char Symbol { get; }

    public void Move(int dx, int dy, IMapCollision map, GameState state, bool isPlayerMove = false)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsWalkable(newX, newY)) return;

        if (!state.IsCellFree(newX, newY, isPlayerMove)) return;

        X = newX;
        Y = newY;
    }
}