namespace Laba3;

public class Map : IMapCollision
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Cell[,] Grid { get; set; }
    
    public Map(int width, int height)
    {}
    
    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return false;
        }
        
        return Grid[x, y].IsWalkable; 
    }
}