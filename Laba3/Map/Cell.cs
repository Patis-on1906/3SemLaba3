using System.Text.Json.Serialization;

namespace Laba3;

public class Cell
{
    public CellType Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
    [JsonIgnore]
    public bool IsWalkable => Type != CellType.Wall;
    
    [JsonIgnore]
    public char Symbol => IsWalkable ? '.' : '#';
    
    public Cell(CellType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
    
    public Cell(int x, int y) : this(CellType.Floor, x, y) { }

    [JsonConstructor] 
    public Cell() { }
    
    public enum CellType { Wall, Floor }
}
