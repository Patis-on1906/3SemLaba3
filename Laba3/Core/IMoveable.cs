namespace Laba3;

public interface IMoveable : IEntity
{
    void Move(int dx, int dy, IMapCollision map);
}