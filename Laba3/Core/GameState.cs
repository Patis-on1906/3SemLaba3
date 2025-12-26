using System.Text.Json.Serialization;

namespace Laba3;

public class GameState : IGameState
{
    public Map Map { get; }
    public IEntityRepository EntityRepository { get; }
    public DateTime SaveTime { get; private set; }
    
    [JsonIgnore]
    public Player Player => EntityRepository.Player;
    
    [JsonIgnore]
    public int PlayerX => Player?.X ?? 0;
    
    [JsonIgnore]
    public int PlayerY => Player?.Y ?? 0;
    
    [JsonConstructor]
    public GameState(Map map, EntityRepository entityRepository)
    {
        Map = map ?? throw new ArgumentNullException(nameof(map));
        EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
    }
    
    public GameState(Map map) : this(map, new EntityRepository()) { }
    
    public void UpdateSaveTime()
    {
        SaveTime = DateTime.Now;
    }
}