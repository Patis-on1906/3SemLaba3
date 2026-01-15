namespace Laba3
{
    public class GameController
    {
        private GameState _state;
        private readonly IRenderer _renderer;
        private readonly IInputHandler _inputHandler;
        private readonly ISaveService _saveService;
        private readonly IGameLogicService _gameLogic;
        private readonly GameStateChecker _stateChecker;
        private bool _isRunning = true;

        public GameController(
            GameState state,
            IRenderer renderer,
            ISaveService saveService,
            IInputHandler inputHandler,
            IGameLogicService gameLogic)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            _gameLogic = gameLogic ?? throw new ArgumentNullException(nameof(gameLogic));
            _stateChecker = new GameStateChecker();
        }

        public void Run()
        {
            try
            {
                while (_isRunning)
                {
                    _renderer.Draw(_state);

                    var command = _inputHandler.GetCommand();
                    HandleCommand(command);

                    CheckGameState();
                }
            }
            catch (GameOverException)
            {
                _renderer.ShowGameOver();
            }
        }

        public void Update()
        {
            _gameLogic.UpdateWorld(_state);
            CheckGameState();
        }

        public GameState GameState => _state;
        public IInputHandler InputHandler => _inputHandler;

        public void HandleCommand(InputCommand command)
        {
            switch (command)
            {
                case InputCommand.Quit:
                    _isRunning = false;
                    break;
                case InputCommand.Save:
                    SaveGame();
                    break;
                case InputCommand.Load:
                    LoadGame();
                    break;
                case InputCommand.None:
                    break;
                default:
                    HandleMovement(command);
                    break;
            }
        }

        private void HandleMovement(InputCommand command)
        {
            var (dx, dy) = _inputHandler.GetMovementVector(command);
            _gameLogic.ProcessPlayerMovement(_state, dx, dy);
            _gameLogic.UpdateWorld(_state);
        }

        private void SaveGame()
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

        private void LoadGame()
        {
            try
            {
                var newState = _saveService.Load();
                if (newState != null)
                {
                    _state = newState;
                    _renderer.ShowMessage("Игра успешно загружена!", ConsoleColor.Green);
                }
                else
                {
                    _renderer.ShowMessage("Сохранение не найдено", ConsoleColor.Yellow);
                }
            }
            catch (SaveLoadException ex)
            {
                _renderer.ShowMessage($"Ошибка загрузки: {ex.Message}", ConsoleColor.Red);
            }
        }

        private void CheckGameState()
        {
            _gameLogic.CheckGameOver(_state);

            if (_stateChecker.CheckVictory(_state))
            {
                _renderer.ShowVictory();
                _isRunning = false;
            }
        }
    }

    public class GameStateChecker
    {
        public bool CheckVictory(GameState state)
        {
            return state.EntityRepository.Treasures.All(t => t.Collected)
                   && state.Player?.IsAlive == true;
        }
    }
}