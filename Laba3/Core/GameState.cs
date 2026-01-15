using System.Text.Json.Serialization;

namespace Laba3;

public class GameState : IGameState
{
    [JsonPropertyName("map")]
    public Map Map { get; set; }  // Изменено на set

    [JsonPropertyName("entityRepository")]
    public EntityRepository EntityRepository { get; set; }  // Изменено на set

    [JsonPropertyName("saveTime")]
    public DateTime SaveTime { get; set; }  // Изменено на set

    [JsonIgnore]
    public Player Player => EntityRepository?.Player;

    [JsonIgnore]
    public int PlayerX => Player?.X ?? 0;

    [JsonIgnore]
    public int PlayerY => Player?.Y ?? 0;

    [JsonIgnore]
    IEntityRepository IGameState.EntityRepository => EntityRepository;

    [JsonConstructor]
    public GameState(Map map, EntityRepository entityRepository, DateTime saveTime)
    {
        Map = map ?? throw new ArgumentNullException(nameof(map));
        EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        SaveTime = saveTime;
    }

    public GameState(Map map, EntityRepository entityRepository)
        : this(map, entityRepository, DateTime.Now)
    {
    }

    public GameState(Map map) : this(map, new EntityRepository(), DateTime.Now)
    {
    }

    public void UpdateSaveTime()
    {
        SaveTime = DateTime.Now;
    }
}