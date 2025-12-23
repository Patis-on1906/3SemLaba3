using System;
using System.Linq;
using System.Threading;

namespace Laba3
{
    public class GameController
    {
        private GameState _state;
        private readonly IRenderer _renderer;
        private readonly InputHandler _inputHandler;
        private readonly ISaveService _saveService;
        private bool _isRunning = true;

        // Внедрение зависимостей (Dependency Injection)
        public GameController(GameState state, IRenderer renderer, ISaveService saveService)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _renderer = renderer;
            _saveService = saveService;
            _inputHandler = new InputHandler(); // Можно тоже вынести в интерфейс
        }

        public void Run()
        {
            while (_isRunning)
            {
                _renderer.Draw(_state);

                if (_state.Player is IDamageable p && !p.IsAlive)
                {
                    _renderer.ShowGameOver();
                    break;
                }

                // Обработка ввода
                var command = _inputHandler.GetCommand();
                HandleCommand(command);
            }
        }

        private void HandleCommand(InputHandler.GameCommand command)
        {
            bool playerMoved = false;

            switch (command)
            {
                case InputHandler.GameCommand.Quit:
                    _isRunning = false;
                    return;
                case InputHandler.GameCommand.Save:
                    _saveService.Save(_state);
                    _renderer.ShowMessage("Игра сохранена!", ConsoleColor.Green);
                    return;
                case InputHandler.GameCommand.Load:
                    var newState = _saveService.Load();
                    if (newState != null)
                    {
                        _state = newState;
                        _renderer.ShowMessage("Игра загружена!", ConsoleColor.Green);
                    }
                    else
                    {
                        _renderer.ShowMessage("Ошибка загрузки!", ConsoleColor.Red);
                    }

                    return;
                case InputHandler.GameCommand.MoveUp:
                case InputHandler.GameCommand.MoveDown:
                case InputHandler.GameCommand.MoveLeft:
                case InputHandler.GameCommand.MoveRight:
                    var (dx, dy) = _inputHandler.GetMovementVector(command);
                    // Игрок ходит
                    if (_state.Player.Move(dx, dy, _state.Map, _state))
                    {
                        _state.CollectTreasuresAtPlayerPosition();
                        playerMoved = true;
                    }

                    break;
            }

            // Обновляем мир ТОЛЬКО если игрок походил
            if (playerMoved)
            {
                UpdateWorld();
            }
        }

        private void UpdateWorld()
        {
            // Вместо класса EntityUpdater просто перебираем список здесь
            foreach (var entity in _state.UpdatableEntities)
            {
                entity.Update(_state.Map, _state, _state);
            }
        }
    }
}
