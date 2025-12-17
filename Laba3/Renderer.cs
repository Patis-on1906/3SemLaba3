using System;
using System.Linq;

namespace Laba3
{
    public static class Renderer
    {
        public static void Draw(GameState state)
        {
            Console.Clear();

            for (int y = 0; y < state.Map.Height; y++)
            {
                for (int x = 0; x < state.Map.Width; x++)
                {
                    char c = GetCharForCell(x, y, state);
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            int collected = state.Treasures.Count(t => t.Collected);
            Console.WriteLine($"Собрано сокровищ: {collected}");
            Console.WriteLine("WASD/Arrows - move, Q - quit");
        }

        private static char GetCharForCell(int x, int y, GameState state)
        {
            // 1. Игрок — наивысший приоритет (виден всегда)
            if (state.Player.X == x && state.Player.Y == y)
                return state.Player.Symbol; // 'P'

            // 2. Движущиеся враги — перекрывают сокровища и клетку
            var movingEnemy = state.MovingEnemies.FirstOrDefault(e => e.X == x && e.Y == y);
            if (movingEnemy != null)
                return movingEnemy.Symbol; // 'E'

            // 3. Статичные враги — тоже перекрывают сокровища и клетку
            var staticEnemy = state.StaticEnemies.FirstOrDefault(e => e.X == x && e.Y == y);
            if (staticEnemy != null)
                return staticEnemy.Symbol; // 'S'

            // 4. Несобранные сокровища — видны только если сверху ничего нет
            var treasure = state.Treasures.FirstOrDefault(t => !t.Collected && t.X == x && t.Y == y);
            if (treasure != null)
                return treasure.Symbol; // 'T'

            // 5. Фон — символ клетки (стена или пол)
            var cell = state.Map.GetCell(x, y);
            return cell?.Symbol ?? ' '; // '#' или '.', или пробел если вне карты
        }
    }
}