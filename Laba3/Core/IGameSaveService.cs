namespace Laba3;

public interface IGameSaveService
{
    void Save(GameState state, string filePath);
    GameState Load(string filePath);
}