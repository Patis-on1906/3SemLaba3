namespace Laba3;

public interface IMoveable : IEntity
{
    bool Move(int dx, int dy, IMapCollision map, IEntityCollision entities);
}