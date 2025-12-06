using System.Security.Cryptography.X509Certificates;
using static Laba3.Cell;

namespace Laba3;

public class Map : IMapCollision
{

    public int Width { get; set; }
    public int Height { get; set; }
    public Cell[,] Grid { get; set; }

    public Map(int width, int height)
    {

        if (Width < 3 || height < 3)

            throw new ArgumentException("минимальный аргумент карты 3x3");

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
                //стена по краям 
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)


                //создание внешних аспектов
                {
                    Grid[x, y] = new Cell(CellType.Wall, x, y);
                }
                else
                {
                    Grid[x, y] = new Cell(CellType.Floor, x, y);
                }
            }
        }
    }



    // Получение клетки по указанным коор.
    public Cell GetCell(int x, int y)
    {
        if (!IsWalkable(x, y))
            return null;

        return Grid[x, y];
    }

    //Устанавливаем тип клетки по указанным коор.
    public void SetCellType(int x, int y, CellType cellType)
    {
        if (!IsWalkable(x, y))
            throw new ArgumentOutOfRangeException($"координаты([x]) , ([y]) вне границы карты");

        Grid[x, y].Type = cellType;
    }



    //првоерка можно ли проходить через стены 
    public bool IsWalkable(int x, int y)
    {

        return x >= 0 && x < Width && y >= 0 && y < Height;
    }



}
