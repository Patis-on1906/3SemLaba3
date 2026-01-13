using System;
using System.Collections.Generic;
using System.Linq;

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
            var map = CreateOpenMapWithWalls(width, height);
            var state = new GameState(map);

            
            var walkablePositions = GetWalkablePositions(map).ToList();

            if (walkablePositions.Count < 10)
                throw new Exception("Недостаточно проходимых позиций на карте");

            var playerPos = GetRandomPositionWithClearArea(map, 3);
            state.EntityRepository.SetPlayer(_entityFactory.CreatePlayer(playerPos.x, playerPos.y));

            int treasureCount = _random.Next(3, 6);
            var treasurePositions = new List<(int x, int y)>();

            for (int i = 0; i < treasureCount; i++)
            {
                var treasurePos = GetSafeSpawnPosition(map, state, minDistanceFromPlayer: 5);
                state.EntityRepository.AddTreasure(
                    _entityFactory.CreateTreasure(treasurePos.x, treasurePos.y, _random.Next(10, 50))
                );
                treasurePositions.Add(treasurePos);
            }

            foreach (var treasurePos in treasurePositions)
            {
                if (_random.Next(100) < 70)
                {
                    var enemyPos = FindPositionNearTreasure(map, state, treasurePos);
                    if (enemyPos != null)
                    {
                        if (_random.Next(100) < 60)
                        {
                            state.EntityRepository.AddMovingEnemy(
                                _entityFactory.CreateMovingEnemy(enemyPos.Value.x, enemyPos.Value.y, _random.Next(8, 15))
                            );
                        }
                        else
                        {
                            state.EntityRepository.AddStaticEnemy(
                                _entityFactory.CreateStaticEnemy(
                                    enemyPos.Value.x, enemyPos.Value.y,
                                    _random.Next(10, 18),
                                    1,
                                    _random.Next(3, 5)
                                )
                            );
                        }
                    }
                }
            }

            int extraEnemies = _random.Next(2, 4);
            for (int i = 0; i < extraEnemies; i++)
            {
                var enemyPos = GetSafeSpawnPosition(map, state, minDistanceFromPlayer: 3);
                state.EntityRepository.AddMovingEnemy(
                    _entityFactory.CreateMovingEnemy(enemyPos.x, enemyPos.y, _random.Next(5, 12))
                );
            }

            state.UpdateSaveTime();
            return state;
        }

        private Map CreateOpenMapWithWalls(int width, int height)
        {
            var map = new Map(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        map.SetCellType(x, y, Cell.CellType.Wall);
                    }
                    else
                    {
                        map.SetCellType(x, y, Cell.CellType.Floor);
                    }
                }
            }

            GenerateScatteredWalls(map, (width * height) / 15);

            return map;
        }

        private void GenerateScatteredWalls(Map map, int wallCount)
        {
            int placedWalls = 0;
            int attempts = 0;
            int maxAttempts = wallCount * 10;

            while (placedWalls < wallCount && attempts < maxAttempts)
            {
                int x = _random.Next(1, map.Width - 2);
                int y = _random.Next(1, map.Height - 2);
                if (map.IsWalkable(x, y) &&
                    !IsNearCenter(map, x, y) &&
                    HasEnoughSpaceAround(map, x, y, 2))
                {
                    if (_random.Next(100) < 30 && x < map.Width - 3 && y < map.Height - 3)
                    {
                        for (int dx = 0; dx < 2; dx++)
                        {
                            for (int dy = 0; dy < 2; dy++)
                            {
                                if (map.IsWalkable(x + dx, y + dy))
                                {
                                    map.SetCellType(x + dx, y + dy, Cell.CellType.Wall);
                                    placedWalls++;
                                }
                            }
                        }
                    }
                    else if (_random.Next(100) < 40)
                    {
                        int length = _random.Next(2, 4);
                        for (int dx = 0; dx < length && x + dx < map.Width - 1; dx++)
                        {
                            if (map.IsWalkable(x + dx, y))
                            {
                                map.SetCellType(x + dx, y, Cell.CellType.Wall);
                                placedWalls++;
                            }
                        }
                    }
                    else if (_random.Next(100) < 40)
                    {
                        int length = _random.Next(2, 4);
                        for (int dy = 0; dy < length && y + dy < map.Height - 1; dy++)
                        {
                            if (map.IsWalkable(x, y + dy))
                            {
                                map.SetCellType(x, y + dy, Cell.CellType.Wall);
                                placedWalls++;
                            }
                        }
                    }
                    else
                    {
                        map.SetCellType(x, y, Cell.CellType.Wall);
                        placedWalls++;
                    }
                }

                attempts++;
            }
        }

        private bool IsNearCenter(Map map, int x, int y)
        {
            int centerX = map.Width / 2;
            int centerY = map.Height / 2;
            int distance = Math.Abs(x - centerX) + Math.Abs(y - centerY);

            return distance < 10;
        }

        private bool HasEnoughSpaceAround(Map map, int x, int y, int minClearRadius)
        {
            int clearCells = 0;
            int totalCells = 0;

            for (int dx = -minClearRadius; dx <= minClearRadius; dx++)
            {
                for (int dy = -minClearRadius; dy <= minClearRadius; dy++)
                {
                    if (x + dx >= 1 && x + dx < map.Width - 1 &&
                        y + dy >= 1 && y + dy < map.Height - 1)
                    {
                        totalCells++;
                        if (map.IsWalkable(x + dx, y + dy))
                        {
                            clearCells++;
                        }
                    }
                }
            }

            return (double)clearCells / totalCells > 0.8;
        }

        private IEnumerable<(int x, int y)> GetWalkablePositions(Map map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.IsWalkable(x, y))
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        private (int x, int y) GetRandomPositionWithClearArea(Map map, int clearRadius)
        {
            var walkablePositions = GetWalkablePositions(map).ToList();
            var centerX = map.Width / 2;
            var centerY = map.Height / 2;

            var centerPositions = walkablePositions
                .Where(pos => Math.Abs(pos.x - centerX) + Math.Abs(pos.y - centerY) < 10)
                .Where(pos => HasClearArea(map, pos.x, pos.y, clearRadius))
                .ToList();

            if (centerPositions.Count > 0)
            {
                return centerPositions[_random.Next(centerPositions.Count)];
            }
            var validPositions = walkablePositions
                .Where(pos => HasClearArea(map, pos.x, pos.y, clearRadius))
                .ToList();

            if (validPositions.Count > 0)
            {
                return validPositions[_random.Next(validPositions.Count)];
            }

            return walkablePositions[_random.Next(walkablePositions.Count)];
        }

        private bool HasClearArea(Map map, int x, int y, int radius)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (x + dx < 0 || x + dx >= map.Width ||
                        y + dy < 0 || y + dy >= map.Height ||
                        !map.IsWalkable(x + dx, y + dy))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private (int x, int y) GetSafeSpawnPosition(Map map, GameState state, int minDistanceFromPlayer = 3)
        {
            var collisionDetector = new EntityCollisionService(state.EntityRepository);
            var walkablePositions = GetWalkablePositions(map).ToList();

            var validPositions = walkablePositions
                .Where(pos =>
                    !collisionDetector.HasEntityAt(pos.x, pos.y) &&
                    Math.Abs(pos.x - state.PlayerX) + Math.Abs(pos.y - state.PlayerY) >= minDistanceFromPlayer)
                .ToList();

            if (validPositions.Count == 0)
            {
                validPositions = walkablePositions
                    .Where(pos => !collisionDetector.HasEntityAt(pos.x, pos.y))
                    .ToList();
            }

            if (validPositions.Count == 0)
            {
                return walkablePositions[_random.Next(walkablePositions.Count)];
            }

            return validPositions[_random.Next(validPositions.Count)];
        }

        private (int x, int y)? FindPositionNearTreasure(Map map, GameState state, (int x, int y) treasurePos)
        {
            var collisionDetector = new EntityCollisionService(state.EntityRepository);

            var directions = new (int dx, int dy)[]
            {
                (0, -1), (0, 1), (-1, 0), (1, 0), 
                (0, -2), (0, 2), (-2, 0), (2, 0), 
                (-1, -1), (-1, 1), (1, -1), (1, 1) 
            };

            var validPositions = new List<(int x, int y)>();

            foreach (var dir in directions)
            {
                int x = treasurePos.x + dir.dx;
                int y = treasurePos.y + dir.dy;

                if (map.IsWithinBounds(x, y) &&
                    map.IsWalkable(x, y) &&
                    !collisionDetector.HasEntityAt(x, y) &&
                    (x != state.PlayerX || y != state.PlayerY))
                {
                    validPositions.Add((x, y));
                }
            }

            if (validPositions.Count == 0)
            {
                for (int distance = 3; distance <= 5; distance++)
                {
                    for (int dx = -distance; dx <= distance; dx++)
                    {
                        for (int dy = -distance; dy <= distance; dy++)
                        {
                            if (Math.Abs(dx) + Math.Abs(dy) == distance)
                            {
                                int x = treasurePos.x + dx;
                                int y = treasurePos.y + dy;

                                if (map.IsWithinBounds(x, y) &&
                                    map.IsWalkable(x, y) &&
                                    !collisionDetector.HasEntityAt(x, y) &&
                                    (x != state.PlayerX || y != state.PlayerY))
                                {
                                    return (x, y);
                                }
                            }
                        }
                    }
                }
                return null;
            }

            return validPositions[_random.Next(validPositions.Count)];
        }
    }
}