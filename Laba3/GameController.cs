using System;
using System.Linq;
using System.Threading;

namespace Laba3
{
    public class GameController
    {
        private readonly GameState _state;
        private readonly IPlayerLocator _playerLocator;

        public GameController(GameState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state)); 
            _playerLocator = new PlayerLocator(_state.Player);
        }

        public void Run()
        {
            // простой игровой цикл
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
                }

                if (dx != 0 || dy != 0)
                {
                    // вызываем Move у игрока (Player наследует MovingUnit)
                    _state.Player.Move(dx, dy, _state.Map);

                    // После шага игрока — автосбор сокровищ
                    foreach (var t in _state.Treasures)
                        if (!t.Collected && t.X == _state.Player.X && t.Y == _state.Player.Y)
                            t.Collected = true;
                }

                // обновление врагов — через интерфейс IUpdatable
                foreach (var enemy in _state.MovingEnemies)
                    (enemy as IUpdatable)?.Update(_state.Map, _playerLocator);

                foreach (var enemy in _state.StaticEnemies)
                    (enemy as IUpdatable)?.Update(_state.Map, _playerLocator);

                // простой throttle
                Thread.Sleep(80);
            }
        }

        // вспомогательный метод для тестового уровня
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
