using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laba3;

public class JsonSaveService : ISaveService
{
    private const string FileName = "savegame.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public void Save(GameState state)
    {
        try
        {
            state.UpdateSaveTime();
            string json = JsonSerializer.Serialize(state, _options);
            File.WriteAllText(FileName, json);
        }
        catch (Exception ex)
        {
            throw new SaveLoadException("Ошибка сохранения", ex);
        }
    }

    public GameState? Load()
    {
        if (!File.Exists(FileName)) return null;

        try
        {
            string json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<GameState>(json, _options);
        }
        catch (Exception ex)
        {
            throw new SaveLoadException("Ошибка загрузки", ex);
        }
    }
}

public class SaveLoadException : Exception
{
    public SaveLoadException(string message, Exception inner) : base(message, inner) { }
}