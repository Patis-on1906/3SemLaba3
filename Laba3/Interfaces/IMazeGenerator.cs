namespace Laba3;

public interface IMazeGenerator
{
    GameState CreateRandomMazeLevel(int width, int height);
}