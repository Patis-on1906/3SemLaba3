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
            if (state.Player != null && state.Player.X == x && state.Player.Y == y)
                return state.Player.Symbol;
            
            var treasure = state.Treasures.FirstOrDefault(t => !t.Collected && t.X == x && t.Y == y);
            if (treasure != null) return treasure.Symbol;
            
            var mEnemy = state.MovingEnemies.FirstOrDefault(e => e.X == x && e.Y == y);
            if (mEnemy != null) return mEnemy.Symbol;
            
            var sEnemy = state.StaticEnemies.FirstOrDefault(e => e.X == x && e.Y == y);
            if (sEnemy != null) return sEnemy.Symbol;
            
            var cell = state.Map.GetCell(x, y);
            if (cell != null) return cell.Symbol;

            return ' ';
        }
    }
}