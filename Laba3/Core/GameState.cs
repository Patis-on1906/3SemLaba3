namespace Laba3;

public class GameState : IPlayerLocator
{
    public Player Player { get; set; }
    public Map Map { get; set; }
    public List<MovingEnemy> MovingEnemies { get; set; } = new();
    public List<StaticEnemy> StaticEnemies { get; set; } = new();
    public List<Treasure> Treasures { get; set; } = new();
    public string GameVersion { get; set; }
    public DateTime SaveTime { get; set; }
    
    public int PlayerX => Player.X;
    public int PlayerY => Player.Y;

    public GameState(Map map, string gameVersion = "1.0")
    {
        Map = map ?? throw new ArgumentNullException(nameof(map));
        GameVersion = gameVersion;
        SaveTime = DateTime.Now;
    }

    public GameState() { }
}
