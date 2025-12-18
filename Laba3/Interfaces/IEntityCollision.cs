namespace Laba3;

public interface IEntityCollision
{
    bool HasEntityAt(int x, int y, IEntity exclude = null);
}