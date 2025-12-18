namespace Laba3;

public interface IUpdatable
{
    void Update(IMapCollision map, IPlayerLocator playerLocator, IEntityCollision entities);
}