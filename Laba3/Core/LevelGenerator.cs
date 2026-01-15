namespace Laba3;

public class LevelGenerator : ILevelGenerator
{
    private readonly IEntityFactory _entityFactory;
    private readonly MazeGenerator _mazeGenerator;

    public LevelGenerator(IEntityFactory entityFactory = null)
    {
        _entityFactory = entityFactory ?? new EntityFactory();
        _mazeGenerator = new MazeGenerator(entityFactory);
    }

    public GameState CreateTestLevel()
    {
        var map = new Map(50, 25);
        var state = new GameState(map);

        // Создаем комнаты
        CreateRoom(map, 2, 2, 10, 8);
        CreateRoom(map, 2, 12, 12, 10);
        CreateRoom(map, 35, 2, 12, 8);
        CreateRoom(map, 18, 8, 14, 10);
        CreateRoom(map, 35, 12, 12, 10);
        CreateRoom(map, 2, 20, 8, 4);

        // Коридоры
        DigHorizontalPassage(map, 11, 18, 5);
        DigVerticalPassage(map, 6, 9, 12);
        DigHorizontalPassage(map, 31, 35, 6);
        DigPath(map, 31, 13, 35, 17);
        DigHorizontalPassage(map, 13, 35, 17);
        DigVerticalPassage(map, 6, 21, 20);

        // Препятствия
        map.SetCellType(6, 15, Cell.CellType.Wall);
        map.SetCellType(10, 15, Cell.CellType.Wall);
        CreateMiniMaze(map, 22, 11);

        // Сущности - гарантируем, что они на проходимых клетках
        // Проверяем, что позиции проходимы
        if (!map.IsWalkable(6, 5)) map.SetCellType(6, 5, Cell.CellType.Floor);
        if (!map.IsWalkable(40, 5)) map.SetCellType(40, 5, Cell.CellType.Floor);
        if (!map.IsWalkable(25, 13)) map.SetCellType(25, 13, Cell.CellType.Floor);
        if (!map.IsWalkable(5, 21)) map.SetCellType(5, 21, Cell.CellType.Floor);
        if (!map.IsWalkable(15, 5)) map.SetCellType(15, 5, Cell.CellType.Floor);
        if (!map.IsWalkable(38, 6)) map.SetCellType(38, 6, Cell.CellType.Floor);

        state.EntityRepository.SetPlayer(_entityFactory.CreatePlayer(6, 5));
        state.EntityRepository.AddTreasure(_entityFactory.CreateTreasure(40, 5, 100));
        state.EntityRepository.AddTreasure(_entityFactory.CreateTreasure(25, 13, 50));
        state.EntityRepository.AddTreasure(_entityFactory.CreateTreasure(5, 21, 20));
        state.EntityRepository.AddMovingEnemy(_entityFactory.CreateMovingEnemy(15, 5, 5));
        state.EntityRepository.AddMovingEnemy(_entityFactory.CreateMovingEnemy(38, 6, 12));

        state.UpdateSaveTime();
        return state;
    }

    public GameState CreateRandomLevel(int width, int height)
    {
        return _mazeGenerator.CreateRandomMazeLevel(width, height);
    }

    private void CreateRoom(Map map, int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                if (i == x || i == x + width - 1 || j == y || j == y + height - 1)
                    map.SetCellType(i, j, Cell.CellType.Wall);
                else
                    map.SetCellType(i, j, Cell.CellType.Floor);
            }
        }
    }

    private void DigHorizontalPassage(Map map, int x1, int x2, int y)
    {
        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
        }
    }

    private void DigVerticalPassage(Map map, int x, int y1, int y2)
    {
        for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
        }
    }

    private void DigPath(Map map, int x1, int y1, int x2, int y2)
    {
        DigHorizontalPassage(map, x1, x2, y1);
        DigVerticalPassage(map, x2, y1, y2);
    }

    private void CreateMiniMaze(Map map, int startX, int startY)
    {
        map.SetCellType(startX, startY, Cell.CellType.Wall);
        map.SetCellType(startX + 1, startY, Cell.CellType.Wall);
        map.SetCellType(startX, startY + 2, Cell.CellType.Wall);
    }
}