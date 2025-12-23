using System.Text.Json.Serialization;

namespace Laba3;

public class GameState : IGameState
{
    public Player Player { get; set; }
    public Map Map { get; set; }
    public List<MovingEnemy> MovingEnemies { get; set; } = new();
    public List<StaticEnemy> StaticEnemies { get; set; } = new();
    public List<Treasure> Treasures { get; set; } = new();
    
    public DateTime SaveTime { get; set; }

    public GameState() { } 
    public GameState(Map map) { Map = map ?? throw new ArgumentNullException(nameof(map)); }

    // Динамическое объединение всех сущностей (не хранится в JSON)
    [JsonIgnore]
    public IEnumerable<IEntity> Entities => 
        MovingEnemies.Cast<IEntity>()
            .Concat(StaticEnemies)
            .Concat(Treasures)
            .Concat(Player != null ? new[] { Player } : Array.Empty<IEntity>());

    [JsonIgnore]
    public IEnumerable<IUpdatable> UpdatableEntities => 
        MovingEnemies.Cast<IUpdatable>().Concat(StaticEnemies.Cast<IUpdatable>());

    [JsonIgnore] public int PlayerX => Player?.X ?? 0;
    [JsonIgnore] public int PlayerY => Player?.Y ?? 0;
    
    public void AddEntity(IEntity entity)
    {
        switch (entity)
        {
            case Player p: Player = p; break;
            case MovingEnemy m: MovingEnemies.Add(m); break;
            case StaticEnemy s: StaticEnemies.Add(s); break;
            case Treasure t: Treasures.Add(t); break;
        }
    }

    public bool HasEntityAt(int x, int y, IEntity exclude = null)
    {
        return Entities.Any(e => e.X == x && e.Y == y && e != exclude && !e.IsPassable);
    }

    public void CollectTreasuresAtPlayerPosition()
    {
        foreach (var t in Treasures.Where(t => !t.Collected))
        {
            if (t.X == Player.X && t.Y == Player.Y) t.Collect(Player);
        }
    }
}
