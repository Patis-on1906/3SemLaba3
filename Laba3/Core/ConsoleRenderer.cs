using System;
using System.Linq;

namespace Laba3
{
    public class ConsoleRenderer : IRenderer
    {
        private readonly Dictionary<EntityType, ConsoleColor> _colors = new()
        {
            { EntityType.Player, ConsoleColor.Green },
            { EntityType.MovingEnemy, ConsoleColor.Red },
            { EntityType.StaticEnemy, ConsoleColor.DarkRed },
            { EntityType.Treasure, ConsoleColor.Yellow }
        };
        
        public void Draw(IGameState state)
        {
            Console.SetCursorPosition(0, 0);
            
            var entityMap = new Dictionary<(int, int), IEntity>();
            foreach (var entity in state.Entities)
            {
                if (entity is Treasure t && t.Collected) continue;
                
                if (!entityMap.ContainsKey((entity.X, entity.Y)))
                {
                    entityMap.Add((entity.X, entity.Y), entity);
                }
            }

            for (int y = 0; y < state.Map.Height; y++)
            {
                for (int x = 0; x < state.Map.Width; x++)
                {
                    if (entityMap.TryGetValue((x, y), out var entity))
                    {
                        // Получаем цвет из словаря по типу сущности
                        Console.ForegroundColor = _colors[entity.EntityType];
                        Console.Write(entity.Symbol);
                        Console.ResetColor();
                    }
                    else
                    {
                        var cell = state.Map.GetCell(x, y);
                        char sym = cell?.Symbol ?? ' ';
                        ConsoleColor col = (sym == '#') ? ConsoleColor.DarkGray : ConsoleColor.White;
                        Print(sym, col);
                    }
                }
                Console.WriteLine();
            }
            DrawInfo(state);
        }

        private void Print(char c, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(c);
            Console.ResetColor();
        }

        private void DrawInfo(IGameState state)
        {
            Console.WriteLine(new string('=', 20));
            Console.WriteLine($"HP: {state.Player?.Health}/{state.Player?.MaxHealth} | Score: {state.Player?.Score}");
        }

        public void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Нажмите клавишу...");
            Console.ReadKey(true);
        }

        public void ShowGameOver()
        {
            Console.Clear();
            ShowMessage("GAME OVER", ConsoleColor.Red);
        }
    }
}