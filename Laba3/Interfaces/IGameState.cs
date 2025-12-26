namespace Laba3;

public interface IGameState : IPlayerLocator
{
    Map Map { get; }
    IEntityRepository EntityRepository { get; }
    DateTime SaveTime { get; }
}
