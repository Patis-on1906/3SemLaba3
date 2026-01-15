using System.Text.Json;

namespace Laba3;

public class GameStateRepository : IRepository<GameState>
{
    private readonly string _filePath = "savegame.json";
    private GameState _gameState;
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions 
    { 
        WriteIndented = true,
        IncludeFields = true 
    };

    public GameState GetData() => _gameState;
    public void SetData(GameState data) => _gameState = data;

    public void Load()
    {
        if (!File.Exists(_filePath)) return;
        var json = File.ReadAllText(_filePath);
        _gameState = JsonSerializer.Deserialize<GameState>(json, Options);
    }

    public void Save()
    {
        if (_gameState == null) return;
        var json = JsonSerializer.Serialize(_gameState, Options);
        File.WriteAllText(_filePath, json);
    }
}