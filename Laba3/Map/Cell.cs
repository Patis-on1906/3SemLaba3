using System.Text.Json.Serialization;

namespace Laba3;

public class Cell
{
    [JsonPropertyName("type")]
    public CellType Type { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonIgnore]
    public bool IsWalkable => Type != CellType.Wall;

    [JsonIgnore]
    public char Symbol => IsWalkable ? '.' : '#';

    [JsonConstructor]
    public Cell(CellType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }

    public Cell(int x, int y) : this(CellType.Floor, x, y) { }

    public Cell() : this(0, 0) { }

    public enum CellType 
    { 
        Wall = 0, 
        Floor = 1 
    }
}