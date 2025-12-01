using System;
using System.Linq; // для Count() — можно убрать, если не хочешь

namespace Laba3
{
    public static class Renderer
    {
        public static void Draw(GameState state)
        {
            Console.Clear();

            // Отрисовка карты
            for (int y = 0; y < state.Map.Height; y++)
            {
                for (int x = 0; x < state.Map.Width; x++)
                {
                    char c = GetCharForCell(x, y, state);
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            // Информация под картой
            int collectedCount = state.Treasures.Count(t => t.Collected);
            Console.WriteLine($"Собрано сокровищ: {collectedCount}");
            // Если будет здоровье игрока — добавишь:  | Здоровье: {state.Player.Health}
        }

        private static char GetCharForCell(int x, int y, GameState state)
        {
            // 1. Игрок — самый высокий приоритет
            if (state.Player.X == x && state.Player.Y == y)
                return 'P';

            // 2. Несобранное сокровище
            foreach (var t in state.Treasures)
                if (t.X == x && t.Y == y && !t.Collected)
                    return 'T';

            // 3. Враг
            foreach (var e in state.Enemies)
                if (e.X == x && e.Y == y)
                    return 'E';

            // 4. Стена или пол
            if (x < 0 || x >= state.Map.Width || y < 0 || y >= state.Map.Height)
                return '#';

            if (state.Map.Grid[x, y].Type == Cell.CellType.Wall)
                return '#';

            // 5. Пол — пробел (по ТЗ)
            return ' ';
        }
    }
}