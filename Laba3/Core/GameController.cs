using System;

namespace Laba3
{
    public class GameController
    {
        private GameState _state;
        private readonly IRenderer _renderer;
        private readonly ISaveService _saveService;
        private readonly IGameLogicService _gameLogic;
        private bool _isRunning = true;
        
        public event Action? RequestClose;

        public GameController(
            GameState state,
            IRenderer renderer,
            ISaveService saveService,
            IGameLogicService gameLogic)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _gameLogic = gameLogic ?? throw new ArgumentNullException(nameof(gameLogic));
        }

        public GameState GameState => _state;
        public bool IsRunning => _isRunning;

        public void Update()
        {
            _gameLogic.UpdateWorld(_state);
            CheckGameState();
        }

        public void HandleCommand(InputCommand command)
        {
            switch (command)
            {
                case InputCommand.Quit:
                    RequestClose?.Invoke(); // Вызываем событие вместо _isRunning = false
                    break;
                case InputCommand.Save:
                    SaveGame();
                    break;
                case InputCommand.Load:
                    LoadGame();
                    break;
                case InputCommand.MoveUp:
                case InputCommand.MoveDown:
                case InputCommand.MoveLeft:
                case InputCommand.MoveRight:
                    HandleMovement(command);
                    break;
                case InputCommand.None:
                    break;
            }
        }

        private void HandleMovement(InputCommand command)
        {
            var (dx, dy) = GetMovementVector(command);
            _gameLogic.ProcessPlayerMovement(_state, dx, dy);
            _gameLogic.UpdateWorld(_state);
        }

        private (int dx, int dy) GetMovementVector(InputCommand command)
        {
            return command switch
            {
                InputCommand.MoveUp => (0, -1),
                InputCommand.MoveDown => (0, 1),
                InputCommand.MoveLeft => (-1, 0),
                InputCommand.MoveRight => (1, 0),
                _ => (0, 0)
            };
        }

        public void SaveGame()
        {
            try
            {
                _saveService.Save(_state);
                _renderer.ShowMessage("Игра успешно сохранена!", ConsoleColor.Green);
            }
            catch (SaveLoadException ex)
            {
                _renderer.ShowMessage($"Ошибка сохранения: {ex.Message}", ConsoleColor.Red);
            }
        }

        public void LoadGame()
        {
            try
            {
                var loadedState = _saveService.Load();
                
                if (loadedState == null)
                {
                    _renderer.ShowMessage("Сохранение не найдено", ConsoleColor.Yellow);
                    return;
                }

                if (loadedState.Map == null || loadedState.EntityRepository == null || loadedState.Player == null)
                {
                    _renderer.ShowMessage("Ошибка: повреждённое сохранение", ConsoleColor.Red);
                    return;
                }

                _state = loadedState;
                _renderer.ShowMessage("Игра успешно загружена!", ConsoleColor.Green);
            }
            catch (SaveLoadException ex)
            {
                _renderer.ShowMessage($"Ошибка загрузки: {ex.Message}", ConsoleColor.Red);
            }
        }

        private void CheckGameState()
        {
            try
            {
                _gameLogic.CheckGameOver(_state);
            }
            catch (GameOverException)
            {
                throw; // Пробрасываем для обработки в ViewModel
            }

            if (_gameLogic.CheckVictory(_state))
            {
                _isRunning = false; // Останавливаем обновления
                throw new VictoryException(); // Бросаем специальное исключение
            }
        }
    }
    
    public class VictoryException : Exception
    {
        public VictoryException() : base("Победа!") { }
    }
}