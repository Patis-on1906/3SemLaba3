using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Laba3;

public class JsonSaveService : ISaveService
{
    private const string FileName = "savegame.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        IncludeFields = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters = { new JsonStringEnumConverter() },
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public JsonSaveService()
    {
        // Конструктор без параметров
    }

    public void Save(GameState state)
    {
        try
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            state.UpdateSaveTime();

            var directory = Path.GetDirectoryName(Path.GetFullPath(FileName));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(state, _options);
            File.WriteAllText(FileName, json, Encoding.UTF8);

            Debug.WriteLine($"=== СОХРАНЕНИЕ ===");
            Debug.WriteLine($"Файл: {Path.GetFullPath(FileName)}");
            Debug.WriteLine($"Карта: {state.Map.Width}x{state.Map.Height}");
            Debug.WriteLine($"Игрок: ({state.PlayerX}, {state.PlayerY})");
            Debug.WriteLine($"Здоровье: {state.Player?.Health}/{state.Player?.MaxHealth}");
            Debug.WriteLine($"Сокровищ: {state.EntityRepository.Treasures.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при сохранении: {ex}");
            throw new SaveLoadException("Ошибка сохранения", ex);
        }
    }

    public GameState? Load()
    {
        if (!File.Exists(FileName))
        {
            Debug.WriteLine($"Файл сохранения не найден: {Path.GetFullPath(FileName)}");
            return null;
        }

        try
        {
            string json = File.ReadAllText(FileName, Encoding.UTF8);
            
            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.WriteLine("Файл сохранения пуст");
                return null;
            }

            var state = JsonSerializer.Deserialize<GameState>(json, _options);
            
            if (state == null)
            {
                Debug.WriteLine("Не удалось десериализовать состояние игры");
                return null;
            }

            // Проверка целостности загруженных данных
            Debug.WriteLine($"=== ЗАГРУЗКА ===");
            Debug.WriteLine($"Файл: {Path.GetFullPath(FileName)}");
            Debug.WriteLine($"Карта: {state.Map?.Width}x{state.Map?.Height}");
            Debug.WriteLine($"Игрок: ({state.PlayerX}, {state.PlayerY})");
            Debug.WriteLine($"Здоровье: {state.Player?.Health}/{state.Player?.MaxHealth}");
            Debug.WriteLine($"Жив: {state.Player?.IsAlive}");
            Debug.WriteLine($"Сокровищ: {state.EntityRepository?.Treasures?.Count}");
            Debug.WriteLine($"Врагов движущихся: {state.EntityRepository?.MovingEnemies?.Count}");
            Debug.WriteLine($"Врагов статичных: {state.EntityRepository?.StaticEnemies?.Count}");
            
            return state;
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"Ошибка десериализации JSON: {ex}");
            throw new SaveLoadException("Ошибка загрузки: неверный формат файла", ex);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при загрузке: {ex}");
            throw new SaveLoadException("Ошибка загрузки", ex);
        }
    }
}

public class SaveLoadException : Exception
{
    public SaveLoadException(string message, Exception inner) : base(message, inner) { }
}