using System.Windows;
using Laba3.WPF.ViewModels;

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
        }
    }
}
