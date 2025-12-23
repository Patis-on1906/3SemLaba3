namespace Laba3;

public class LevelGenerator
{
    private readonly Random _random = new Random();

    public GameState CreateTestLevel()
    {
        var state = new GameState(new Map(50, 25));

        // 1. Создаем комнаты (теперь стены не перекрывают друг друга)
        CreateRoom(state.Map, 2, 2, 10, 8);    // Вход
        CreateRoom(state.Map, 2, 12, 12, 10);  // Зал
        CreateRoom(state.Map, 35, 2, 12, 8);   // Сокровищница
        CreateRoom(state.Map, 18, 8, 14, 10);  // Арена
        CreateRoom(state.Map, 35, 12, 12, 10); // Темница
        CreateRoom(state.Map, 2, 20, 8, 4);    // Библиотека

        // 2. Прорубаем коридоры (теперь они гарантированно проходимы)
        // Вход -> Арена
        DigHorizontalPassage(state.Map, 11, 18, 5);
        
        // Вход -> Зал (вертикально через стену)
        DigVerticalPassage(state.Map, 6, 9, 12);
        
        // Арена -> Сокровищница
        DigHorizontalPassage(state.Map, 31, 35, 6);
        
        // Арена -> Темница
        DigPath(state.Map, 31, 13, 35, 17);
        
        // Зал -> Темница
        DigHorizontalPassage(state.Map, 13, 35, 17);
        
        // Зал -> Библиотека
        DigVerticalPassage(state.Map, 6, 21, 20);

        // 3. Декорации и препятствия (только там, где не мешают проходу)
        state.Map.SetCellType(6, 15, Cell.CellType.Wall);
        state.Map.SetCellType(10, 15, Cell.CellType.Wall);
        
        // Мини-лабиринт в Арене смещен, чтобы не перекрывать входы
        CreateMiniMaze(state.Map, 22, 11);

        // 4. Размещение игрока и сокровищ
        var player = new Player { X = 6, Y = 5 };
        state.AddEntity(player);

        // Расставляем сокровища
        state.AddEntity(new Treasure { X = 40, Y = 5, Value = 100 }); 
        state.AddEntity(new Treasure { X = 25, Y = 13, Value = 50 });
        state.AddEntity(new Treasure { X = 5, Y = 21, Value = 20 });

        // Враги
        state.AddEntity(new MovingEnemy { X = 15, Y = 5, Damage = 5 });
        state.AddEntity(new MovingEnemy { X = 38, Y = 6, Damage = 12 });

        return state;
    }

    /// <summary>
    /// Создает пустую комнату со стенами по периметру.
    /// </summary>
    private void CreateRoom(Map map, int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                // Если это граница прямоугольника — ставим стену
                if (i == x || i == x + width - 1 || j == y || j == y + height - 1)
                    map.SetCellType(i, j, Cell.CellType.Wall);
                else
                    map.SetCellType(i, j, Cell.CellType.Floor);
            }
        }
    }

    /// <summary>
    /// "Прокапывает" горизонтальный проход, превращая любые клетки в пол.
    /// </summary>
    private void DigHorizontalPassage(Map map, int x1, int x2, int y)
    {
        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
        }
    }

    /// <summary>
    /// "Прокапывает" вертикальный проход.
    /// </summary>
    private void DigVerticalPassage(Map map, int x, int y1, int y2)
    {
        for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
        }
    }

    /// <summary>
    /// Г-образный проход между двумя точками.
    /// </summary>
    private void DigPath(Map map, int x1, int y1, int x2, int y2)
    {
        DigHorizontalPassage(map, x1, x2, y1);
        DigVerticalPassage(map, x2, y1, y2);
    }

    private void CreateMiniMaze(Map map, int startX, int startY)
    {
        // Упрощенные препятствия, чтобы не блокировать проход
        map.SetCellType(startX, startY, Cell.CellType.Wall);
        map.SetCellType(startX + 1, startY, Cell.CellType.Wall);
        map.SetCellType(startX, startY + 2, Cell.CellType.Wall);
    }

    /// <summary>
    /// Создает горизонтальный коридор
    /// </summary>
    private void CreateHorizontalCorridor(Map map, int x1, int y, int x2)
    {
        int start = Math.Min(x1, x2);
        int end = Math.Max(x1, x2);
        
        for (int x = start; x <= end; x++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
            // Стены по бокам коридора
            if (map.IsWalkable(x, y - 1))
                map.SetCellType(x, y - 1, Cell.CellType.Wall);
            if (map.IsWalkable(x, y + 1))
                map.SetCellType(x, y + 1, Cell.CellType.Wall);
        }
    }

    /// <summary>
    /// Создает вертикальный коридор
    /// </summary>
    private void CreateVerticalCorridor(Map map, int x, int y1, int y2)
    {
        int start = Math.Min(y1, y2);
        int end = Math.Max(y1, y2);
        
        for (int y = start; y <= end; y++)
        {
            map.SetCellType(x, y, Cell.CellType.Floor);
            // Стены по бокам коридора
            if (map.IsWalkable(x - 1, y))
                map.SetCellType(x - 1, y, Cell.CellType.Wall);
            if (map.IsWalkable(x + 1, y))
                map.SetCellType(x + 1, y, Cell.CellType.Wall);
        }
    }

    /// <summary>
    /// Создает L-образный коридор
    /// </summary>
    private void CreateCorridor(Map map, int x1, int y1, int x2, int y2)
    {
        // Сначала горизонтально
        CreateHorizontalCorridor(map, x1, y1, x2);
        // Потом вертикально
        CreateVerticalCorridor(map, x2, y1, y2);
    }

    public GameState CreateRandomLevel(int width, int height)
    {
        var state = new GameState(new Map(width, height));

        // Генерируем случайные стены
        GenerateRandomWalls(state.Map);

        // Игрок в центре
        var player = new Player
        {
            X = width / 2,
            Y = height / 2
        };
        state.AddEntity(player);

        // Случайные сокровища
        for (int i = 0; i < 5; i++)
        {
            var pos = GetRandomWalkablePosition(state.Map, state);
            state.AddEntity(new Treasure
            {
                X = pos.x,
                Y = pos.y,
                Value = _random.Next(5, 30)
            });
        }

        // Случайные враги с разными параметрами
        for (int i = 0; i < 3; i++)
        {
            var pos = GetRandomWalkablePosition(state.Map, state);
            state.AddEntity(new MovingEnemy
            {
                X = pos.x,
                Y = pos.y,
                Damage = _random.Next(7, 12),
                MoveSpeed = _random.Next(2, 4),
            });
        }

        for (int i = 0; i < 2; i++)
        {
            var pos = GetRandomWalkablePosition(state.Map, state);
            state.AddEntity(new StaticEnemy
            {
                X = pos.x,
                Y = pos.y,
                Damage = _random.Next(10, 18),
                AttackRange = _random.Next(1, 3),
                AttackCooldown = _random.Next(2, 4)
            });
        }

        return state;
    }

    /// <summary>
    /// Генерирует случайные структуры стен
    /// </summary>
    private void GenerateRandomWalls(Map map)
    {
        int structureCount = (map.Width * map.Height) / 15;

        for (int i = 0; i < structureCount; i++)
        {
            int x = _random.Next(2, map.Width - 3);
            int y = _random.Next(2, map.Height - 3);

            int structure = _random.Next(4);

            switch (structure)
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
        if (!map.OutsideMap(x, y) && map.IsWalkable(x, y))
        {
            map.SetCellType(x, y, Cell.CellType.Wall);
        }
    }

    private (int x, int y) GetRandomWalkablePosition(Map map, IEntityCollision entities)
    {
        int x, y;
        int attempts = 0;

        do
        {
            x = _random.Next(1, map.Width - 1);
            y = _random.Next(1, map.Height - 1);
            attempts++;

            if (attempts > 1000)
                throw new Exception("Не удалось найти свободную позицию");

        } while (!map.IsWalkable(x, y) || entities.HasEntityAt(x, y));

        return (x, y);
    }
}