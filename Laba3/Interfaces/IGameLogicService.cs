namespace Laba3;

public interface IGameLogicService
{
    void ProcessPlayerMovement(GameState state, int dx, int dy);
    void UpdateWorld(GameState state);
    void CheckGameOver(GameState state);
}
