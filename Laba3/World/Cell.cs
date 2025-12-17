// Cell.cs
using System.Text.Json.Serialization;

namespace Laba3;
public class Cell
{
    public CellType Type { get; set; }

    [JsonIgnore] // Не сериализуем вычисляемые свойства
    public bool IsWalkable => Type != CellType.Wall;

    public int X { get; set; }
    public int Y { get; set; }

    [JsonIgnore] // Не сериализуем вычисляемые свойства
    public char Symbol => IsWalkable ? '.' : '#';

    public Cell() { } // Конструктор по умолчанию

    public Cell(CellType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }

    public Cell(int x, int y) : this(CellType.Floor, x, y) { }

    public enum CellType { Wall, Floor }
}