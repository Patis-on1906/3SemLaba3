using System;
using System.Linq;
using System.Threading;

namespace Laba3
{
    public class GameController
    {
        private GameState _state; 
        private IPlayerLocator _playerLocator; 
        private readonly IGameSaveService _saveService;

        public GameController(GameState state, IGameSaveService? saveService = null)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _playerLocator = new PlayerLocator(_state.Player); 
            _saveService = saveService ?? new JsonGameSaveService();
        }

        public void SaveGame(string filePath = "save.json")
        {
            _saveService.Save(_state, filePath);
            Console.WriteLine($"Игра сохранена в {filePath}");
        }

        public void LoadGame(string filePath = "save.json")
        {
            try
            {
                var loadedState = _saveService.Load(filePath);
                _state = loadedState;

                // Создаем новый PlayerLocator с загруженным игроком
                _playerLocator = new PlayerLocator(_state.Player);

                Console.WriteLine($"Игра загружена из {filePath}");
                Console.Clear();
                Renderer.Draw(_state);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                Console.WriteLine("Продолжаем текущую игру...");
            }
        }

        public void Run()
        {
            while (true)
            {
                Renderer.Draw(_state);

                var key = Console.ReadKey(true).Key;
                int dx = 0, dy = 0;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        dy = -1; break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        dy = 1; break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        dx = -1; break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        dx = 1; break;
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.T:
                        SaveGame();
                        break;
                    case ConsoleKey.L: // Загрузка (L — Load)
                        LoadGame();
                        break;
                }

                // Враги ходят первыми
                foreach (var enemy in _state.MovingEnemies)
                    enemy.Update(_state, _playerLocator);

                foreach (var enemy in _state.StaticEnemies)
                    enemy.Update(_state, _playerLocator);

                // Потом игрок
                if (dx != 0 || dy != 0)
                {
                    _state.Player.Move(dx, dy, _state.Map, _state, isPlayerMove: true);

                    // Сбор сокровищ
                    foreach (var t in _state.Treasures)
                    {
                        if (!t.Collected && t.X == _state.Player.X && t.Y == _state.Player.Y)
                            t.Collected = true;
                    }

                    // Уничтожение врагов
                    _state.MovingEnemies.RemoveAll(e => e.X == _state.Player.X && e.Y == _state.Player.Y);
                    _state.StaticEnemies.RemoveAll(e => e.X == _state.Player.X && e.Y == _state.Player.Y);
                }

                Thread.Sleep(80);
            }
        }

        public static GameState CreateTestLevel()
        {
            var state = new GameState(new Map(20, 12));
            state.Player = new Player { X = 5, Y = 5 };
            state.Treasures.Clear();
            state.Treasures.Add(new Treasure { X = 10, Y = 3 });
            state.Treasures.Add(new Treasure { X = 15, Y = 8 });
            state.MovingEnemies.Clear();
            state.MovingEnemies.Add(new MovingEnemy { X = 8, Y = 7 });
            state.MovingEnemies.Add(new MovingEnemy { X = 12, Y = 4 });
            state.StaticEnemies.Clear();
            state.StaticEnemies.Add(new StaticEnemy { X = 14, Y = 6 });
            return state;
        }
    }
}