using System.Net.NetworkInformation;

namespace Laba3;


public class Cell
{
    public CellType Type { get; set; }
    //проходимая ли клетка 
    public bool IsWalkable => Type != CellType.Wall;

    public int X { get; set; }

    public int Y { get; set; }
    public char Symbol => IsWalkable ? '.' : '#';


    //создание новой клетки 
    public Cell(CellType type, int x, int y)
    {

        Type = type;
        X = x;
        Y = y;
    }

    //клетка(пол)
    public Cell(int x, int y) : this(CellType.Floor, x, y) { }


    //типы клеток 
    public enum CellType { Wall, Floor }

}
