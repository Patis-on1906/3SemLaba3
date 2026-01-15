using System.Text.Json.Serialization;

namespace Laba3
{
    public class Map : IMapCollision
    {
        private Cell[,] _grid;

        [JsonPropertyName("width")]
        public int Width { get; private set; }

        [JsonPropertyName("height")]
        public int Height { get; private set; }
        
        [JsonPropertyName("cells")]
        public Cell[] Cells
        {
            get
            {
                if (_grid == null) return Array.Empty<Cell>();

                var flat = new Cell[Width * Height];
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        flat[y * Width + x] = _grid[x, y];
                    }
                }
                return flat;
            }
            set
            {
                if (value == null || value.Length == 0) return;

                _grid = new Cell[Width, Height];
                for (int i = 0; i < value.Length && i < Width * Height; i++)
                {
                    int x = i % Width;
                    int y = i / Width;
                    _grid[x, y] = value[i];
                }
            }
        }
        
        [JsonConstructor]
        public Map(int width, int height, Cell[] cells)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный размер карты 3x3");

            Width = width;
            Height = height;
            _grid = new Cell[width, height];

            if (cells != null && cells.Length > 0)
            {
                Cells = cells;
            }
            else
            {
                GenerateMap();
            }
        }
        
        public Map(int width, int height)
        {
            if (width < 3 || height < 3)
                throw new ArgumentException("Минимальный размер карты 3x3");

            Width = width;
            Height = height;
            _grid = new Cell[width, height];
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
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
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}