namespace Laba3;

public class GameState
{
    public Player Player { get; set; }
    public Map Map { get; set; }
    public List<Enemy> Enemies { get; set; } = new();
    public List<Treasure> Treasures { get; set; } = new();
}