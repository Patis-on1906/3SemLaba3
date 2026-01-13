using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Laba3;

namespace Laba3.WPF
{
    public class WpfRenderer : IRenderer
    {
        private readonly Canvas _canvas;
        private readonly TextBlock _infoText;
        private int _tileSize;

        private readonly Brush[] _entityBrushes;
        private readonly Typeface _typeface = new Typeface("Consolas");

        public WpfRenderer(Canvas canvas, TextBlock infoText, int tileSize = 32)
        {
            _canvas = canvas;
            _infoText = infoText;
            _tileSize = tileSize;

            _entityBrushes = new Brush[]
            {
                Brushes.Green,      // Player
                Brushes.Red,        // MovingEnemy
                Brushes.DarkRed,    // StaticEnemy
                Brushes.Yellow      // Treasure
            };
        }

        public void Draw(IGameState state)
        {
            Render(state);
        }

        public void Render(IGameState state)
        {
            if (state?.Map == null) return;

            // Очищаем канвас
            _canvas.Children.Clear();

            // Устанавливаем размер канваса в зависимости от карты
            _canvas.Width = state.Map.Width * _tileSize;
            _canvas.Height = state.Map.Height * _tileSize;

            // Отрисовка карты
            for (int y = 0; y < state.Map.Height; y++)
            {
                for (int x = 0; x < state.Map.Width; x++)
                {
                    var cell = state.Map.GetCell(x, y);
                    var isWall = cell?.IsWalkable == false;

                    // Создаем клетку
                    var rect = new Rectangle
                    {
                        Width = _tileSize,
                        Height = _tileSize,
                        Fill = isWall ? Brushes.DarkSlateGray : Brushes.DarkGray,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0.5
                    };

                    Canvas.SetLeft(rect, x * _tileSize);
                    Canvas.SetTop(rect, y * _tileSize);
                    _canvas.Children.Add(rect);

                    if (isWall)
                    {
                        var innerRect = new Rectangle
                        {
                            Width = _tileSize - 4,
                            Height = _tileSize - 4,
                            Fill = Brushes.SlateGray,
                            Stroke = Brushes.Black,
                            StrokeThickness = 0.3
                        };

                        Canvas.SetLeft(innerRect, x * _tileSize + 2);
                        Canvas.SetTop(innerRect, y * _tileSize + 2);
                        _canvas.Children.Add(innerRect);
                    }
                }
            }

            // Отрисовка сущностей
            foreach (var entity in state.EntityRepository.GetAllEntities())
            {
                if (entity is Treasure t && t.Collected) continue;

                var brush = _entityBrushes[(int)entity.EntityType];

                var ellipse = new Ellipse
                {
                    Width = _tileSize - 8,
                    Height = _tileSize - 8,
                    Fill = brush,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(ellipse, entity.X * _tileSize + 4);
                Canvas.SetTop(ellipse, entity.Y * _tileSize + 4);
                _canvas.Children.Add(ellipse);

                // Текст символа сущности
                var text = new TextBlock
                {
                    Text = entity.Symbol.ToString(),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = _tileSize * 0.4,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Canvas.SetLeft(text, entity.X * _tileSize + _tileSize * 0.3);
                Canvas.SetTop(text, entity.Y * _tileSize + _tileSize * 0.2);
                _canvas.Children.Add(text);

                if (entity.EntityType == EntityType.Player)
                {
                    var highlight = new Rectangle
                    {
                        Width = _tileSize,
                        Height = _tileSize,
                        Stroke = Brushes.Lime,
                        StrokeThickness = 2,
                        Fill = Brushes.Transparent
                    };

                    Canvas.SetLeft(highlight, entity.X * _tileSize);
                    Canvas.SetTop(highlight, entity.Y * _tileSize);
                    _canvas.Children.Add(highlight);
                }
            }

            UpdateInfoText(state);
        }

        private void UpdateInfoText(IGameState state)
        {
            if (state.Player != null)
            {
                _infoText.Text = $"HP: {state.Player.Health}/{state.Player.MaxHealth} | " +
                                $"Score: {state.Player.Score} | " +
                                $"Time: {state.SaveTime:HH:mm:ss}";
            }
        }

        public void ShowMessage(string message, ConsoleColor color)
        {
            var brush = color switch
            {
                ConsoleColor.Red => Brushes.Red,
                ConsoleColor.Green => Brushes.Green,
                ConsoleColor.Yellow => Brushes.Yellow,
                _ => Brushes.White
            };

            MessageBox.Show(message, "Сообщение",
                MessageBoxButton.OK,
                color == ConsoleColor.Red ? MessageBoxImage.Error :
                color == ConsoleColor.Green ? MessageBoxImage.Information :
                MessageBoxImage.Warning);
        }

        public void ShowGameOver()
        {
            MessageBox.Show("=== GAME OVER ===", "Конец игры",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowVictory()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("=== ПОБЕДА! ===", "Победа!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            });
        }

        public void ChangeTileSize(int newSize)
        {
            _tileSize = newSize;
        }
    }
}