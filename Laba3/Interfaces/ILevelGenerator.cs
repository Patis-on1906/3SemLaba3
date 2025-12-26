namespace Laba3;

public interface ILevelGenerator
{
    GameState CreateTestLevel();
    GameState CreateRandomLevel(int width, int height);
}