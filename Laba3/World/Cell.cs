using System.Net.NetworkInformation;

namespace Laba3;


public class Cell
{
    public CellType Type { get; set; }
    public bool IsWalkable => Type != CellType.Wall;
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol => IsWalkable ? '.' : '#';

    
    public Cell(CellType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
    
    public Cell(int x, int y) : this(CellType.Floor, x, y) { }
    
    public enum CellType { Wall, Floor }
}
