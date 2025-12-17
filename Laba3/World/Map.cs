namespace Laba3
{
    public class Map : IMapCollision
    {
        public int Width { get; set; }  // Изменено с get; на get; set;
        public int Height { get; set; } // Изменено с get; на get; set;
        public Cell[][] Grid { get; set; } // Изменено с get; на get; set;

        // Для сериализации нужен конструктор по умолчанию
        public Map() { }

        // Основной конструктор
        public Map(int width, int height, IMapGenerator? generator = null)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный размер карты 3x3");

            Width = width;
            Height = height;

            Grid = new Cell[width][];
            for (int x = 0; x < width; x++)
            {
                Grid[x] = new Cell[height];
            }

            var actualGenerator = generator ?? new RandomWallsGenerator();
            actualGenerator.Generate(this);
        }

        // Возвращает null если вне карты
        public Cell? GetCell(int x, int y)
        {
            if (OutsideMap(x, y)) return null;
            return Grid[x][y];
        }

        // Установка типа клетки 
        public void SetCellType(int x, int y, Cell.CellType cellType)
        {
            if (OutsideMap(x, y))
                throw new ArgumentOutOfRangeException(nameof(x), "Координаты вне карты");

            Grid[x][y].Type = cellType;
        }

        // Проверяет и границы, и проходимость клетки
        public bool IsWalkable(int x, int y)
        {
            if (OutsideMap(x, y)) return false;
            return Grid[x][y].IsWalkable;
        }

        public bool OutsideMap(int x, int y)
        {
            return x < 0 || x >= Width || y < 0 || y >= Height;
        }
    }
}