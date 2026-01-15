using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Laba3;
using System.Diagnostics;

namespace Laba3.WPF
{
    public partial class MainWindow : Window
    {
        private GameController _gameController;
        private WpfRenderer _renderer;
        private DispatcherTimer _gameTimer;
        private const int TileSize = 32;

        public MainWindow()
        {
            try
            {
                Debug.WriteLine("MainWindow constructor started");
                InitializeComponent();
                Debug.WriteLine("InitializeComponent completed");
                InitializeGame();
                Debug.WriteLine("InitializeGame completed");
                StartGameTimer();
                Debug.WriteLine("StartGameTimer completed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MainWindow constructor error: {ex}");
                MessageBox.Show($"Ошибка инициализации окна: {ex.Message}", "Критическая ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void InitializeGame()
        {
            try
            {
                Debug.WriteLine("InitializeGame started");
                var entityFactory = new EntityFactory();
                Debug.WriteLine("EntityFactory created");

                _renderer = new WpfRenderer(GameCanvas, InfoText, TileSize);
                Debug.WriteLine("WpfRenderer created");

                var saveService = new JsonSaveService();
                var inputHandler = new WpfInputHandler();
                var gameLogic = new GameLogicService(entityFactory);
                var levelGenerator = new LevelGenerator(entityFactory);

                Debug.WriteLine("Creating random level...");
                var gameState = levelGenerator.CreateRandomLevel(61, 31); 
                Debug.WriteLine($"Level created. Map size: {gameState.Map.Width}x{gameState.Map.Height}");
                Debug.WriteLine($"Player position: ({gameState.PlayerX}, {gameState.PlayerY})");
                Debug.WriteLine($"Entities count: {gameState.EntityRepository.GetAllEntities().Count()}");

                _gameController = new GameController(
                    gameState,
                    _renderer,
                    saveService,
                    inputHandler,
                    gameLogic
                );
                Debug.WriteLine("GameController created successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InitializeGame error: {ex}");
                MessageBox.Show($"Ошибка инициализации игры: {ex.Message}\n{ex.StackTrace}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw; 
            }
        }

        private void StartGameTimer()
        {
            try
            {
                _gameTimer = new DispatcherTimer();
                _gameTimer.Interval = TimeSpan.FromMilliseconds(16); 
                _gameTimer.Tick += GameTimer_Tick;
                _gameTimer.Start();
                Debug.WriteLine("Game timer started");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"StartGameTimer error: {ex}");
                throw;
            }
        }
        private void CheckVictory()
        {
            var state = _gameController?.GameState;
            if (state != null)
            {
                var allTreasuresCollected = state.EntityRepository.Treasures.All(t => t.Collected);
                if (allTreasuresCollected && state.Player?.IsAlive == true)
                {
                    _gameTimer.Stop();  // Остановить таймер

                    // Показать сообщение о победе
                    MessageBoxResult result = MessageBox.Show("=== ПОБЕДА! ===\nХотите сыграть еще?", "Победа!",
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                    if (result == MessageBoxResult.Yes)
                    {
                        InitializeGame();
                        _gameTimer.Start();
                    }
                    else
                    {
                        Close();  
                    }
                }
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_gameController != null)
                {
                    _gameController.Update();
                    _renderer.Render(_gameController.GameState);

                    CheckVictory();
                }
            }
            catch (GameOverException)
            {
                _gameTimer.Stop();
                MessageBox.Show("Игра окончена!", "Game Over",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();  
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var inputHandler = _gameController?.InputHandler as WpfInputHandler;
            if (inputHandler != null)
            {
                inputHandler.ProcessKey(e.Key);

                // Обработка движения
                var command = inputHandler.GetCommand();
                if (command >= InputCommand.MoveUp && command <= InputCommand.MoveRight)
                {
                    _gameController?.HandleCommand(command);
                }
                else
                {
                    switch (command)
                    {
                        case InputCommand.Save:
                            SaveGame();
                            break;
                        case InputCommand.Load:
                            LoadGame();
                            break;
                        case InputCommand.Quit:
                            Close();
                            break;
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _gameTimer?.Stop();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Начать новую игру?", "Новая игра",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            LoadGame();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveGame()
        {
            try
            {
                _gameController?.HandleCommand(InputCommand.Save);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       private void LoadGame()
        {
            try
            {
                _gameTimer?.Stop(); // Останавливаем таймер перед загрузкой
                
                var saveService = new JsonSaveService();
                var loadedState = saveService.Load();
                
                if (loadedState == null)
                {
                    MessageBox.Show("Сохранение не найдено", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    _gameTimer?.Start(); // Возобновляем таймер
                    return;
                }

                // Проверяем целостность загруженного состояния
                if (loadedState.Map == null || loadedState.EntityRepository == null || loadedState.Player == null)
                {
                    MessageBox.Show("Ошибка: повреждённое сохранение", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    _gameTimer?.Start();
                    return;
                }

                // Пересоздаём игровой контроллер с загруженным состоянием
                var entityFactory = new EntityFactory();
                var inputHandler = new WpfInputHandler();
                var gameLogic = new GameLogicService(entityFactory);

                _gameController = new GameController(
                    loadedState,
                    _renderer,
                    saveService,
                    inputHandler,
                    gameLogic
                );

                // Рендерим сразу после загрузки
                _renderer.Render(loadedState);

                MessageBox.Show("Игра успешно загружена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _gameTimer?.Start(); // Возобновляем таймер
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _gameTimer?.Start();
            }
        }

        private void TileSize24_Click(object sender, RoutedEventArgs e)
        {
            _renderer?.ChangeTileSize(24);
        }

        private void TileSize32_Click(object sender, RoutedEventArgs e)
        {
            _renderer?.ChangeTileSize(32);
        }

        private void TileSize48_Click(object sender, RoutedEventArgs e)
        {
            _renderer?.ChangeTileSize(48);
        }
        private void NewMazeGame_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Создать новый случайный лабиринт?", "Новый лабиринт",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                InitializeMazeGame();
            }
        }

        private void InitializeMazeGame()
        {
            try
            {
                var entityFactory = new EntityFactory();
                _renderer = new WpfRenderer(GameCanvas, InfoText, TileSize);
                var saveService = new JsonSaveService();
                var inputHandler = new WpfInputHandler();
                var gameLogic = new GameLogicService(entityFactory);
                var levelGenerator = new LevelGenerator(entityFactory);

                var gameState = levelGenerator.CreateRandomLevel(41, 21);

                _gameController = new GameController(
                    gameState,
                    _renderer,
                    saveService,
                    inputHandler,
                    gameLogic
                );

                _gameTimer?.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}