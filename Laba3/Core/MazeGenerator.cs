namespace Laba3
{
    public class MazeGenerator
    {
        private readonly Random _random = new Random();
        private readonly IEntityFactory _entityFactory;

        public MazeGenerator(IEntityFactory entityFactory = null)
        {
            _entityFactory = entityFactory ?? new EntityFactory();
        }

        public GameState CreateRandomMazeLevel(int width, int height)
        {
            var map = CreateBaseMap(width, height);
            var state = new GameState(map);

            PlacePlayer(state);
            PlaceTreasures(state, 3, 6);
            PlaceEnemies(state, 2, 5);

            return state;
        }

        private Map CreateBaseMap(int width, int height)
        {
            var map = new Map(width, height);
            GenerateScatteredWalls(map, (width * height) / 15);
            return map;
        }

        private void PlacePlayer(GameState state)
        {
            var playerPos = FindSpawnPosition(state.Map, state.EntityRepository);
            state.EntityRepository.SetPlayer(_entityFactory.CreatePlayer(playerPos.x, playerPos.y));
        }

        private void PlaceTreasures(GameState state, int minCount, int maxCount)
        {
            int count = _random.Next(minCount, maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                var pos = FindSpawnPosition(state.Map, state.EntityRepository);
                state.EntityRepository.AddTreasure(
                    _entityFactory.CreateTreasure(pos.x, pos.y, _random.Next(10, 50))
                );
            }
        }

        private void PlaceEnemies(GameState state, int minCount, int maxCount)
        {
            int count = _random.Next(minCount, maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                var pos = FindSpawnPosition(state.Map, state.EntityRepository);
                if (_random.Next(100) < 60)
                {
                    state.EntityRepository.AddMovingEnemy(
                        _entityFactory.CreateMovingEnemy(pos.x, pos.y, _random.Next(8, 15))
                    );
                }
                else
                {
                    state.EntityRepository.AddStaticEnemy(
                        _entityFactory.CreateStaticEnemy(pos.x, pos.y, _random.Next(10, 18))
                    );
                }
            }
        }

        private (int x, int y) FindSpawnPosition(Map map, IEntityRepository entities)
        {
            var walkablePositions = GetWalkablePositions(map).ToList();
            var validPositions = walkablePositions
                .Where(pos => !entities.HasEntityAt(pos.x, pos.y))
                .ToList();

            return validPositions.Count > 0
                ? validPositions[_random.Next(validPositions.Count)]
                : walkablePositions[_random.Next(walkablePositions.Count)];
        }

        private IEnumerable<(int x, int y)> GetWalkablePositions(Map map)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    if (map.IsWalkable(x, y))
                        yield return (x, y);
        }

        private void GenerateScatteredWalls(Map map, int wallCount)
        {
            int placed = 0;
            for (int i = 0; i < wallCount * 3 && placed < wallCount; i++)
            {
                int x = _random.Next(1, map.Width - 2);
                int y = _random.Next(1, map.Height - 2);

                if (map.IsWalkable(x, y) && CanPlaceWall(map, x, y))
                {
                    map.SetCellType(x, y, Cell.CellType.Wall);
                    placed++;
                }
            }
        }

        private bool CanPlaceWall(Map map, int x, int y)
        {
            // Не ставим стену рядом с центром карты
            int centerX = map.Width / 2;
            int centerY = map.Height / 2;
            return Math.Abs(x - centerX) + Math.Abs(y - centerY) > 5;
        }
    }
}