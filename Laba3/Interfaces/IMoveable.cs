namespace Laba3;

public interface IMoveable : IEntity
{
    bool TryMove(int dx, int dy, IMapCollision map, IEntityCollision entities);
}