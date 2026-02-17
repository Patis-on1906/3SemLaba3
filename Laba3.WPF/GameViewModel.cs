using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Laba3.WPF
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly IRenderer _renderer;
        private readonly ISaveService _saveService;
        private readonly IGameLogicService _gameLogicService;
        private readonly ILevelGenerator _levelGenerator;
        private readonly IEntityFactory _entityFactory;
        private readonly IInputHandler _inputHandler;
        private readonly GameStateChecker _stateChecker;
        private readonly DispatcherTimer _gameTimer;
        private GameController _gameController;
        private string _playerInfo = string.Empty;

        public GameViewModel(IRenderer renderer)
            : this(
                renderer,
                new JsonSaveService(),
                new EntityFactory())
        {
        }

        public GameViewModel(
            IRenderer renderer,
            ISaveService saveService,
            IEntityFactory entityFactory)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _gameLogicService = new GameLogicService(_entityFactory);
            _levelGenerator = new LevelGenerator(_entityFactory);
            _inputHandler = new InputCommandMapper();
            _stateChecker = new GameStateChecker();

            NewGameCommand = new RelayCommand(_ => NewGame());
            SaveGameCommand = new RelayCommand(_ => SaveGame());
            LoadGameCommand = new RelayCommand(_ => LoadGame());
            ExitCommand = new RelayCommand(_ => RequestClose?.Invoke());
            ChangeTileSizeCommand = new RelayCommand(size => ChangeTileSize(size));
            HandleInputCommand = new RelayCommand(command => HandleInput(command));

            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _gameTimer.Tick += GameTimer_Tick;

            InitializeGame();
            _gameTimer.Start();
        }

        public ICommand NewGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ChangeTileSizeCommand { get; }
        public ICommand HandleInputCommand { get; }

        public string PlayerInfo
        {
            get => _playerInfo;
            private set
            {
                if (_playerInfo == value)
                {
                    return;
                }

                _playerInfo = value;
                OnPropertyChanged();
            }
        }

        public event Action? RequestClose;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void HandleClosing()
        {
            SaveOnExit();
            _gameTimer.Stop();
        }

        private void InitializeGame()
        {
            var newState = _levelGenerator.CreateRandomLevel(46, 21);
            ReplaceGameController(newState);
        }

        private void ReplaceGameController(GameState state)
        {
            _gameController = new GameController(
                state,
                _renderer,
                _saveService,
                _inputHandler,
                _gameLogicService);

            UpdatePlayerInfo();
            _renderer.Draw(_gameController.GameState);
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _gameController.Update();
                _renderer.Draw(_gameController.GameState);
                UpdatePlayerInfo();

                if (_stateChecker.CheckVictory(_gameController.GameState))
                {
                    _gameTimer.Stop();
                    _renderer.ShowVictory();
                }
            }
            catch (GameOverException)
            {
                _gameTimer.Stop();
                _renderer.ShowGameOver();
            }
        }

        private void HandleInput(object? parameter)
        {
            if (parameter is not InputCommand command)
            {
                return;
            }

            switch (command)
            {
                case InputCommand.Save:
                    SaveGame();
                    break;
                case InputCommand.Load:
                    LoadGame();
                    break;
                case InputCommand.Quit:
                    RequestClose?.Invoke();
                    break;
                case InputCommand.None:
                    break;
                default:
                    _gameController.HandleCommand(command);
                    break;
            }
        }

        private void NewGame()
        {
            // Останавливаем таймер на время создания новой игры
            _gameTimer.Stop();
    
            var newState = _levelGenerator.CreateRandomLevel(46, 21);
            ReplaceGameController(newState);
    
            // Запускаем таймер
            _gameTimer.Start();
    
            _renderer.Draw(_gameController.GameState);
        }

        private void SaveGame()
        {
            try
            {
                _saveService.Save(_gameController.GameState);
                _renderer.ShowMessage("Игра успешно сохранена!", ConsoleColor.Green);
            }
            catch (SaveLoadException ex)
            {
                _renderer.ShowMessage($"Ошибка сохранения: {ex.Message}", ConsoleColor.Red);
            }
        }

        private void SaveOnExit()
        {
            try
            {
                _saveService.Save(_gameController.GameState);
            }
            catch (SaveLoadException)
            {
                // Avoid blocking closing flow.
            }
        }
        
        private void LoadGame()
        {
            try
            {
                // Спрашиваем подтверждение, так как текущий прогресс будет потерян
                var result = MessageBox.Show(
                    "Загрузить сохранение? Текущий прогресс будет потерян.", 
                    "Подтверждение", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
            
                if (result != MessageBoxResult.Yes)
                    return;
            
                _gameTimer.Stop();
                var loadedState = _saveService.Load();

                if (loadedState == null)
                {
                    _renderer.ShowMessage("Сохранение не найдено", ConsoleColor.Yellow);
                    _gameTimer.Start();
                    return;
                }

                if (loadedState.Map == null || loadedState.EntityRepository == null || loadedState.Player == null)
                {
                    _renderer.ShowMessage("Ошибка: повреждённое сохранение", ConsoleColor.Red);
                    _gameTimer.Start();
                    return;
                }

                ReplaceGameController(loadedState);
                _renderer.ShowMessage("Игра успешно загружена!", ConsoleColor.Green);
            }
            catch (SaveLoadException ex)
            {
                _renderer.ShowMessage($"Ошибка загрузки: {ex.Message}", ConsoleColor.Red);
            }
            finally
            {
                _gameTimer.Start();
            }
        }

        private void ChangeTileSize(object? parameter)
        {
            if (parameter is not int size)
            {
                return;
            }

            if (_renderer is Laba3.WPF.WpfRenderer wpfRenderer)
            {
                wpfRenderer.ChangeTileSize(size);
                _renderer.Draw(_gameController.GameState);
            }
        }

        private void UpdatePlayerInfo()
        {
            var player = _gameController?.GameState?.Player;
            if (player == null)
            {
                PlayerInfo = string.Empty;
                return;
            }

            PlayerInfo = $"HP: {player.Health}/{player.MaxHealth} | Score: {player.Score}";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
