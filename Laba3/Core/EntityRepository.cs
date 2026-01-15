using System.Text.Json.Serialization;

namespace Laba3;

public class EntityRepository : IEntityRepository
{
    [JsonPropertyName("player")]
    public Player Player { get; set; }

    [JsonPropertyName("movingEnemies")]
    public List<MovingEnemy> MovingEnemiesList { get; set; } = new();

    [JsonPropertyName("staticEnemies")]
    public List<StaticEnemy> StaticEnemiesList { get; set; } = new();

    [JsonPropertyName("treasures")]
    public List<Treasure> TreasuresList { get; set; } = new();

    [JsonIgnore]
    public IReadOnlyList<MovingEnemy> MovingEnemies => MovingEnemiesList;

    [JsonIgnore]
    public IReadOnlyList<StaticEnemy> StaticEnemies => StaticEnemiesList;

    [JsonIgnore]
    public IReadOnlyList<Treasure> Treasures => TreasuresList;

    [JsonConstructor]
    public EntityRepository(Player player, List<MovingEnemy> movingEnemiesList,
        List<StaticEnemy> staticEnemiesList, List<Treasure> treasuresList)
    {
        Player = player;
        MovingEnemiesList = movingEnemiesList ?? new List<MovingEnemy>();
        StaticEnemiesList = staticEnemiesList ?? new List<StaticEnemy>();
        TreasuresList = treasuresList ?? new List<Treasure>();
    }

    public EntityRepository()
    {
        MovingEnemiesList = new List<MovingEnemy>();
        StaticEnemiesList = new List<StaticEnemy>();
        TreasuresList = new List<Treasure>();
    }

    public void SetPlayer(Player player)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
    }

    public void AddMovingEnemy(MovingEnemy enemy)
    {
        MovingEnemiesList.Add(enemy ?? throw new ArgumentNullException(nameof(enemy)));
    }

    public void AddStaticEnemy(StaticEnemy enemy)
    {
        StaticEnemiesList.Add(enemy ?? throw new ArgumentNullException(nameof(enemy)));
    }

    public void AddTreasure(Treasure treasure)
    {
        TreasuresList.Add(treasure ?? throw new ArgumentNullException(nameof(treasure)));
    }

    public bool RemoveTreasure(string id)
    {
        var treasure = TreasuresList.FirstOrDefault(t => t.Id == id);
        if (treasure != null)
        {
            return TreasuresList.Remove(treasure);
        }
        return false;
    }
    
    public bool HasEntityAt(int x, int y, IEntity exclude = null)
    {
        return GetAllEntities()
            .Any(e => e.X == x && e.Y == y && e != exclude && !e.IsPassable);
    }
    
    public IEntity GetEntityAt(int x, int y)
    {
        return GetAllEntities().FirstOrDefault(e => e.X == x && e.Y == y);
    }

    public IEnumerable<IEntity> GetAllEntities()
    {
        var entities = new List<IEntity>();

        if (Player != null)
            entities.Add(Player);

        entities.AddRange(MovingEnemiesList);
        entities.AddRange(StaticEnemiesList);
        entities.AddRange(TreasuresList.Where(t => !t.Collected));

        return entities;
    }

    public IEnumerable<IUpdatable> GetUpdatableEntities()
    {
        var updatables = new List<IUpdatable>();
        
        updatables.AddRange(MovingEnemiesList);
        
        updatables.AddRange(StaticEnemiesList);

        return updatables;
    }
}