namespace Laba3;

public class PlayerLocator : IPlayerLocator
{
    private readonly Player _player;

    public PlayerLocator(Player player)
    {
        _player = player;
    }

    public int PlayerX => _player.X;
    public int PlayerY => _player.Y;
}