namespace Laba3;

public class PlayerLocator : IPlayerLocator
{
    public Player Player { get; }

    public PlayerLocator(Player player)
    {
        Player = player;
    }

    public int PlayerX => Player.X;
    public int PlayerY => Player.Y;
}