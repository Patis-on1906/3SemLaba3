using System.Text.Json.Serialization;

namespace Laba3;

public class EntityRepository : IEntityRepository
{
    private Player _player;
    private readonly List<MovingEnemy> _movingEnemies = new();
    private readonly List<StaticEnemy> _staticEnemies = new();
    private readonly List<Treasure> _treasures = new();
    
    public Player Player => _player;
    
    public IReadOnlyList<MovingEnemy> MovingEnemies => _movingEnemies;
    public IReadOnlyList<StaticEnemy> StaticEnemies => _staticEnemies;
    public IReadOnlyList<Treasure> Treasures => _treasures;
    
    [JsonConstructor]
    public EntityRepository() { }
    
    public void SetPlayer(Player player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }
    
    public void AddMovingEnemy(MovingEnemy enemy)
    {
        _movingEnemies.Add(enemy ?? throw new ArgumentNullException(nameof(enemy)));
    }
    
    public void AddStaticEnemy(StaticEnemy enemy)
    {
        _staticEnemies.Add(enemy ?? throw new ArgumentNullException(nameof(enemy)));
    }
    
    public void AddTreasure(Treasure treasure)
    {
        _treasures.Add(treasure ?? throw new ArgumentNullException(nameof(treasure)));
    }
    
    public bool RemoveTreasure(string id)
    {
        var treasure = _treasures.FirstOrDefault(t => t.Id == id);
        if (treasure != null)
        {
            return _treasures.Remove(treasure);
        }
        return false;
    }
    
    public IEnumerable<IEntity> GetAllEntities()
    {
        var entities = new List<IEntity>();
        
        if (_player != null) entities.Add(_player);
        entities.AddRange(_movingEnemies);
        entities.AddRange(_staticEnemies);
        entities.AddRange(_treasures.Where(t => !t.Collected));
        
        return entities;
    }
    
    public IEnumerable<IUpdatable> GetUpdatableEntities()
    {
        return _movingEnemies.Cast<IUpdatable>()
            .Concat(_staticEnemies.Cast<IUpdatable>());
    }
}