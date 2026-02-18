using System;
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
                _gameLogicService);

            // Подписываемся на событие RequestClose из контроллера
            _gameController.RequestClose += OnGameControllerRequestClose;

            UpdatePlayerInfo();
            _renderer.Draw(_gameController.GameState);
        }

        private void OnGameControllerRequestClose()
        {
            // Вызываем событие RequestClose ViewModel, которое закрывает окно
            RequestClose?.Invoke();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _gameController.Update();
                _renderer.Draw(_gameController.GameState);
                UpdatePlayerInfo();
            }
            catch (GameOverException)
            {
                _gameTimer.Stop();
                _renderer.ShowGameOver();
            }
            catch (VictoryException)
            {
                _gameTimer.Stop();
                HandleVictory();
            }
        }

        private void HandleVictory()
        {
            // Используем Dispatcher для показа MessageBox в UI-потоке
            Application.Current.Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(
                    "=== ПОБЕДА! ===\n\nВы собрали все сокровища!\n\nХотите начать новую игру?", 
                    "Победа!", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Exclamation);
        
                if (result == MessageBoxResult.Yes)
                {
                    NewGame();
                }
                else
                {
                    RequestClose?.Invoke(); // Закрываем окно при отказе
                }
            });
        }

        private void HandleInput(object? parameter)
        {
            if (parameter is InputCommand command)
            {
                _gameController.HandleCommand(command);
            }
        }

        private void NewGame()
        {
            _gameTimer.Stop();
            var newState = _levelGenerator.CreateRandomLevel(46, 21);
            ReplaceGameController(newState);
            _gameTimer.Start();
            _renderer.Draw(_gameController.GameState);
        }

        private void SaveGame()
        {
            _gameController.SaveGame();
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
                var result = MessageBox.Show(
                    "Загрузить сохранение? Текущий прогресс будет потерян.", 
                    "Подтверждение", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
        
                if (result != MessageBoxResult.Yes)
                    return;
        
                _gameTimer.Stop();
                _gameController.LoadGame();
        
                // Обновляем отображение
                _renderer.Draw(_gameController.GameState);
                UpdatePlayerInfo();
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