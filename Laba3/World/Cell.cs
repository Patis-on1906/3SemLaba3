namespace Laba3;

public class Cell
{
    public enum CellType { Wall, Floor }
    
    public CellType Type { get; set; } = CellType.Floor;
    public bool IsWalkable => Type == CellType.Floor;
}