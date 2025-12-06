namespace Laba3;

public class GameState
{
    public Player Player { get; set; }
    public Map Map { get; set; }
    public List<MovingEnemy> MovingEnemies { get; set; } = new();
    public List<StaticEnemy> StaticEnemies { get; set; } = new();
    public List<Treasure> Treasures { get; set; } = new();
    public string GameVersion { get; set; }
    public DateTime SaveTime { get; set; }

    public GameState(Map map, string gameVersion = "1.0")
    {

        CurrentMap = map ?? throw new ArgumentNullException(nameof(map));
        GameVersion = gameVersion;
        SaveTime = DateTime.Now;
    }

    public GameState() { }

}
