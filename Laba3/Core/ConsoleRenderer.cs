using System.Text;

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
            foreach (var entity in state.EntityRepository.GetAllEntities())
            {
                if (entity is Treasure t && t.Collected) continue;
                
                if (!entityMap.ContainsKey((entity.X, entity.Y)))
                {
                    entityMap.Add((entity.X, entity.Y), entity);
                }
            }
            
            var sb = new StringBuilder();
            
            for (int y = 0; y < state.Map.Height; y++)
            {
                for (int x = 0; x < state.Map.Width; x++)
                {
                    if (entityMap.TryGetValue((x, y), out var entity))
                    {
                        sb.Append(GetColoredChar(entity.Symbol, _colors[entity.EntityType]));
                    }
                    else
                    {
                        var cell = state.Map.GetCell(x, y);
                        char sym = cell?.Symbol ?? ' ';
                        ConsoleColor col = (sym == '#') ? ConsoleColor.DarkGray : ConsoleColor.White;
                        sb.Append(GetColoredChar(sym, col));
                    }
                }
                sb.AppendLine();
            }
            
            Console.Write(sb.ToString());
            DrawInfo(state);
        }
        
        private string GetColoredChar(char c, ConsoleColor color)
        {
            return $"\u001b[38;5;{(int)color}m{c}\u001b[0m";
        }
        
        private void DrawInfo(IGameState state)
        {
            Console.WriteLine(new string('=', 30));
            if (state.Player != null)
            {
                Console.WriteLine($"HP: {state.Player.Health}/{state.Player.MaxHealth} | " +
                                 $"Score: {state.Player.Score} | " +
                                 $"Time: {state.SaveTime:HH:mm:ss}");
            }
            Console.WriteLine("Управление: WASD/Стрелки - движение, J - сохранить, L - загрузить, Q - выход");
        }
        
        public void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
        }
        
        public void ShowGameOver()
        {
            Console.Clear();
            ShowMessage("=== GAME OVER ===", ConsoleColor.Red);
        }
        
        public void ShowVictory()
        {
            Console.Clear();
            ShowMessage("=== ПОБЕДА! ===", ConsoleColor.Green);
        }
    }
}