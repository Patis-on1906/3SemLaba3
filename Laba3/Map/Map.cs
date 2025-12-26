using System.Text.Json.Serialization;

namespace Laba3
{
    public class Map : IMapCollision
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Cell[,] _grid;

        public int Width => _width;
        public int Height => _height;

        [JsonIgnore]
        public Cell[,] Grid => _grid;

        [JsonConstructor]
        public Map(int width, int height, Cell[,] grid)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный размер карты 3x3");

            _width = width;
            _height = height;
            _grid = grid;
        }

        public Map(int width, int height)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный размер карты 3x3");

            _width = width;
            _height = height;
            _grid = new Cell[width, height];

            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
                        _grid[x, y] = new Cell(Cell.CellType.Wall, x, y);
                    else
                        _grid[x, y] = new Cell(Cell.CellType.Floor, x, y);
                }
            }
        }

        public Cell? GetCell(int x, int y)
        {
            if (!IsWithinBounds(x, y)) return null;
            return _grid[x, y];
        }

        public void SetCellType(int x, int y, Cell.CellType cellType)
        {
            if (!IsWithinBounds(x, y))
                throw new ArgumentOutOfRangeException(nameof(x), "Координаты вне карты");

            _grid[x, y].Type = cellType;
        }

        public bool IsWalkable(int x, int y)
        {
            if (!IsWithinBounds(x, y)) return false;
            return _grid[x, y].IsWalkable;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
    }
}
