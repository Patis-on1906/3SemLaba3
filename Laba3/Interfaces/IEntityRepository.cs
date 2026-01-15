namespace Laba3;

public interface IEntityRepository
{
    Player Player { get; }
    IReadOnlyList<MovingEnemy> MovingEnemies { get; }
    IReadOnlyList<StaticEnemy> StaticEnemies { get; }
    IReadOnlyList<Treasure> Treasures { get; }
    
    bool HasEntityAt(int x, int y, IEntity exclude = null);
    IEntity GetEntityAt(int x, int y);

    void SetPlayer(Player player);
    void AddMovingEnemy(MovingEnemy enemy);
    void AddStaticEnemy(StaticEnemy enemy);
    void AddTreasure(Treasure treasure);
    bool RemoveTreasure(string id);
    IEnumerable<IEntity> GetAllEntities();
    IEnumerable<IUpdatable> GetUpdatableEntities();
}