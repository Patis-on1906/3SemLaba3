namespace Laba3;

public interface IGameState : IPlayerLocator, IEntityCollision
{
    Map Map { get; }
    IEnumerable<IEntity> Entities { get; }
    IEnumerable<IUpdatable> UpdatableEntities { get; }
}