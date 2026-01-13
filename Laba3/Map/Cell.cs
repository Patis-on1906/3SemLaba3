using System.Text.Json.Serialization;

namespace Laba3;

public class Cell
{
    private CellType _type;
    private readonly int _x;
    private readonly int _y;

    public CellType Type
    {
        get => _type;
        set => _type = value;
    }

    public int X => _x;
    public int Y => _y;

    [JsonIgnore]
    public bool IsWalkable => Type != CellType.Wall;

    [JsonIgnore]
    public char Symbol => IsWalkable ? '.' : '#';

    public Cell(CellType type, int x, int y)
    {
        _type = type;
        _x = x;
        _y = y;
    }

    public Cell(int x, int y) : this(CellType.Floor, x, y) { }

    [JsonConstructor]
    public Cell() : this(0, 0) { }

    public enum CellType { Wall, Floor }
}