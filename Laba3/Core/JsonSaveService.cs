using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laba3;

public class JsonSaveService : ISaveService
{
    private const string FileName = "savegame.json";
    
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        // Эта опция позволяет сохранять enum как строки (опционально)
        Converters = { new JsonStringEnumConverter() } 
    };

    public void Save(GameState state)
    {
        try
        {
            state.SaveTime = DateTime.Now;
            string json = JsonSerializer.Serialize(state, _options);
            File.WriteAllText(FileName, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving: {ex.Message}");
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
            Console.WriteLine($"Error loading: {ex.Message}");
            return null;
        }
    }
}