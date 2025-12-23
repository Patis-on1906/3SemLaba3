namespace Laba3;

public interface IRenderer
{
    void Draw(IGameState state);
    void ShowMessage(string message, ConsoleColor color);
    void ShowGameOver();
}