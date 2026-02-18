namespace Laba3;

public interface ILevelGenerator
{
    GameState CreateRandomLevel(int width, int height);
}