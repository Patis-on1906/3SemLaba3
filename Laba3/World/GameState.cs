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

    public int PlayerX => Player?.X ?? 0;
    public int PlayerY => Player?.Y ?? 0;

    //  онструктор по умолчанию дл€ десериализации
    public GameState()
    {
        GameVersion = "1.0";
        SaveTime = DateTime.Now;
    }

    public GameState(Map map, string gameVersion = "1.0")
    {
        Map = map ?? throw new ArgumentNullException(nameof(map));
        GameVersion = gameVersion;
        SaveTime = DateTime.Now;
    }
    public bool IsCellFree(int x, int y, bool isPlayerMove = false)
    {
        // ≈сли ходит игрок Ч он проходит везде (кроме стен)
        if (isPlayerMove)
            return true;

        // ≈сли ходит враг Ч не может встать на игрока или другого врага
        if (Player.X == x && Player.Y == y)
            return false;

        if (MovingEnemies.Any(e => e.X == x && e.Y == y))
            return false;

        if (StaticEnemies.Any(e => e.X == x && e.Y == y))
            return false;

        return true;
    }

}
