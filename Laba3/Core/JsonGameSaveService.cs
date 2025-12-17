using System.Text.Json;

namespace Laba3;

public class JsonGameSaveService : IGameSaveService
{
    private readonly JsonSerializerOptions _options;

    public JsonGameSaveService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public void Save(GameState state, string filePath)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty", nameof(filePath));

        string json = JsonSerializer.Serialize(state, _options);
        File.WriteAllText(filePath, json);
    }

    public GameState Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Save file not found", filePath);

        string json = File.ReadAllText(filePath);
        var state = JsonSerializer.Deserialize<GameState>(json, _options);

        if (state == null)
            throw new InvalidOperationException("Failed to deserialize game state");

        // После десериализации нужно восстановить ссылки
        // Устанавливаем PlayerLocator на основе загруженного игрока
        // Это важно для работы врагов
        state.Player ??= new Player();

        // Проверяем и инициализируем списки, если они null
        state.MovingEnemies ??= new List<MovingEnemy>();
        state.StaticEnemies ??= new List<StaticEnemy>();
        state.Treasures ??= new List<Treasure>();

        return state;
    }
}