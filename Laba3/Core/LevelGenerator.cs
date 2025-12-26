namespace Laba3;

public class LevelGenerator : ILevelGenerator
{
    private readonly Random _random = new();
    private readonly IEntityFactory _entityFactory;
    
    public LevelGenerator(IEntityFactory entityFactory = null)
    {
        _entityFactory = entityFactory ?? new EntityFactory();
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
        
        // Сущности
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
        var map = new Map(width, height);
        var state = new GameState(map);
        
        GenerateRandomWalls(map);
        
        state.EntityRepository.SetPlayer(_entityFactory.CreatePlayer(width / 2, height / 2));
        
        // Сокровища
        for (int i = 0; i < 5; i++)
        {
            var pos = GetRandomWalkablePosition(map, state);
            state.EntityRepository.AddTreasure(_entityFactory.CreateTreasure(pos.x, pos.y, _random.Next(5, 30)));
        }
        
        // Враги
        for (int i = 0; i < 3; i++)
        {
            var pos = GetRandomWalkablePosition(map, state);
            var enemy = _entityFactory.CreateMovingEnemy(pos.x, pos.y, _random.Next(7, 12));
            enemy.MoveSpeed = _random.Next(2, 4);
            state.EntityRepository.AddMovingEnemy(enemy);
        }
        
        for (int i = 0; i < 2; i++)
        {
            var pos = GetRandomWalkablePosition(map, state);
            state.EntityRepository.AddStaticEnemy(_entityFactory.CreateStaticEnemy(
                pos.x, pos.y, 
                _random.Next(10, 18), 
                _random.Next(1, 3)
            ));
        }
        
        state.UpdateSaveTime();
        return state;
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
    
    private void GenerateRandomWalls(Map map)
    {
        int structureCount = (map.Width * map.Height) / 15;
        
        for (int i = 0; i < structureCount; i++)
        {
            int x = _random.Next(2, map.Width - 3);
            int y = _random.Next(2, map.Height - 3);
            
            switch (_random.Next(4))
            {
                case 0: // Горизонтальная линия
                    for (int dx = 0; dx < 3 && x + dx < map.Width - 1; dx++)
                        TrySetWall(map, x + dx, y);
                    break;
                    
                case 1: // Вертикальная линия
                    for (int dy = 0; dy < 3 && y + dy < map.Height - 1; dy++)
                        TrySetWall(map, x, y + dy);
                    break;
                    
                case 2: // Квадрат 2x2
                    for (int dx = 0; dx < 2 && x + dx < map.Width - 1; dx++)
                        for (int dy = 0; dy < 2 && y + dy < map.Height - 1; dy++)
                            TrySetWall(map, x + dx, y + dy);
                    break;
                    
                case 3: // L-образная фигура
                    TrySetWall(map, x, y);
                    TrySetWall(map, x + 1, y);
                    TrySetWall(map, x, y + 1);
                    break;
            }
        }
    }
    
    private void TrySetWall(Map map, int x, int y)
    {
        if (map.IsWithinBounds(x, y) && map.IsWalkable(x, y))
        {
            map.SetCellType(x, y, Cell.CellType.Wall);
        }
    }
    
    private (int x, int y) GetRandomWalkablePosition(Map map, GameState state)
    {
        var collisionDetector = new EntityCollisionService(state.EntityRepository);
        int x, y;
        int attempts = 0;
        
        do
        {
            x = _random.Next(1, map.Width - 1);
            y = _random.Next(1, map.Height - 1);
            attempts++;
            
            if (attempts > 1000)
                throw new Exception("Не удалось найти свободную позицию");
                
        } while (!map.IsWalkable(x, y) || collisionDetector.HasEntityAt(x, y));
        
        return (x, y);
    }
}