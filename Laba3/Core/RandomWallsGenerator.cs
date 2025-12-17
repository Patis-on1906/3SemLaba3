namespace Laba3; // Убрать лишние скобки

public class RandomWallsGenerator : IMapGenerator
{
    private const double WallChance = 0.15;

    public void Generate(Map map)
    {
        // Инициализируем Grid если он null
        if (map.Grid == null)
        {
            map.Grid = new Cell[map.Width][];
            for (int x = 0; x < map.Width; x++)
            {
                map.Grid[x] = new Cell[map.Height];
            }
        }

        // Базовая рамка из стен
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (x == 0 || y == 0 || x == map.Width - 1 || y == map.Height - 1)
                    map.Grid[x][y] = new Cell(Cell.CellType.Wall, x, y);
                else
                    map.Grid[x][y] = new Cell(Cell.CellType.Floor, x, y);
            }
        }

        // Случайные внутренние стены
        Random rnd = new Random();
        for (int x = 1; x < map.Width - 1; x++)
        {
            for (int y = 1; y < map.Height - 1; y++)
            {
                if (rnd.NextDouble() < WallChance)
                {
                    map.SetCellType(x, y, Cell.CellType.Wall);
                }
            }
        }
    }
}