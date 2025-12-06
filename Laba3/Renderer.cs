using System;
using System.Linq;

namespace Laba3;

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
    }

    private static char GetCharForCell(int x, int y, GameState state)
    {
        // 1. Игрок
        if (state.Player.X == x && state.Player.Y == y)
            return 'P';

        // 2. Сокровище
        foreach (var t in state.Treasures)
            if (t.X == x && t.Y == y && !t.Collected)
                return 'T';

        // 3. Враг
        foreach (var e in state.Enemies)
            if (e.X == x && e.Y == y)
                return 'E';

        // 4. Стена
        if (x >= 0 && x < state.Map.Width && y >= 0 && y < state.Map.Height)
        {
            if (state.Map.Grid[x, y].Type == Cell.CellType.Wall)
                return '#';
        }

        return ' ';
    }
}