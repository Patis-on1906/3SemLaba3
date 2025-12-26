namespace Laba3;

public interface ISaveService
{
    void Save(GameState state);
    GameState? Load();
}
