using System.Windows;

namespace Laba3.WPF
{
    public partial class MainWindow : Window
    {
        private const int TileSize = 32;
        private readonly GameViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var renderer = new WpfRenderer(GameCanvas, TileSize);
            _viewModel = new GameViewModel(renderer);
            _viewModel.RequestClose += Close;

            DataContext = _viewModel;
            Closing += (_, _) => _viewModel.HandleClosing();
    
            // Добавляем обработчик для случая, если пользователь закроет окно после победы
            Closed += (_, _) => 
            {
                if (_viewModel != null)
                {
                    _viewModel.HandleClosing();
                }
            };
        }
    }
}