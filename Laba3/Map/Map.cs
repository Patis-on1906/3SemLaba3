
namespace Laba3
{
    public class Map : IMapCollision
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell[,] Grid { get; set; }

        public Map(int width, int height)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный аргумент карты 3x3");

            Width = width;
            Height = height;
            Grid = new Cell[width, height];

            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                        Grid[x, y] = new Cell(Cell.CellType.Wall, x, y);
                    else
                        Grid[x, y] = new Cell(Cell.CellType.Floor, x, y);
                }
            }
        }

        // Возвращает null если вне карты
        public Cell? GetCell(int x, int y)
        {
            if (OutsideMap(x, y)) return null;
            return Grid[x, y];
        }

        // Установка типа клетки (дает возможность изменять карту)
        public void SetCellType(int x, int y, Cell.CellType cellType)
        {
            if (OutsideMap(x, y))
                throw new ArgumentOutOfRangeException(nameof(x), "Координаты вне карты");

            Grid[x, y].Type = cellType;
        }

        // Проверяет и границы, и саму проходимость клетки
        public bool IsWalkable(int x, int y)
        {
            if (OutsideMap(x, y)) return false;
            return Grid[x, y].IsWalkable;
        }

        public bool OutsideMap(int x, int y)
        {
            return x < 0 || x >= Width || y < 0 || y >= Height;
        }
    }
}