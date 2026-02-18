namespace Laba3;

public interface IGameState
{
    Map Map { get; }
    IEntityRepository EntityRepository { get; }
    DateTime SaveTime { get; }

    // Добавляем эти свойства
    Player Player { get; }
    int PlayerX { get; }
    int PlayerY { get; }
}