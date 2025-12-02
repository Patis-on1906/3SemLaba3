using System;
using System.Threading;

namespace Laba3;

public static class GameController
{
    public static void Run()
    {
        GameState state = new GameState();
        CreateTestLevel(state);  

        while (true)
        {
            Renderer.Draw(state);

            var key = Console.ReadKey(true).Key;

            int dx = 0, dy = 0;
            switch (key)
            {
                case ConsoleKey.UpArrow: case ConsoleKey.W: dy = -1; break;
                case ConsoleKey.DownArrow: case ConsoleKey.S: dy = 1; break;
                case ConsoleKey.LeftArrow: case ConsoleKey.A: dx = -1; break;
                case ConsoleKey.RightArrow: case ConsoleKey.D: dx = 1; break;
                case ConsoleKey.Q: return;
            }

            if (dx != 0 || dy != 0)
            {
                int newX = state.Player.X + dx;
                int newY = state.Player.Y + dy;

                if (state.Map.IsWalkable(newX, newY))
                {
                    state.Player.X = newX;
                    state.Player.Y = newY;

                    // Автосбор сокровищ
                    foreach (var t in state.Treasures)
                        if (t.X == newX && t.Y == newY && !t.Collected)
                            t.Collected = true;
                }
            }

            Thread.Sleep(50);
        }
    }

    private static void CreateTestLevel(GameState state)
    {
        state.Map = new Map(0, 0);        
        state.Map.Width = 20;
        state.Map.Height = 12;
        state.Map.Grid = new Cell[20, 12];

        // Заполняем полом
        for (int x = 0; x < 20; x++)
            for (int y = 0; y < 12; y++)
                state.Map.Grid[x, y] = new Cell { Type = Cell.CellType.Floor };

        // Рамка из стен
        for (int x = 0; x < 20; x++)
        {
            state.Map.Grid[x, 0] = new Cell { Type = Cell.CellType.Wall };
            state.Map.Grid[x, 11] = new Cell { Type = Cell.CellType.Wall };
        }
        for (int y = 0; y < 12; y++)
        {
            state.Map.Grid[0, y] = new Cell { Type = Cell.CellType.Wall };
            state.Map.Grid[19, y] = new Cell { Type = Cell.CellType.Wall };
        }

        state.Player = new Player { X = 5, Y = 5 };

        state.Treasures.Clear();
        state.Treasures.Add(new Treasure { X = 10, Y = 3 });
        state.Treasures.Add(new Treasure { X = 15, Y = 8 });

        state.Enemies.Clear();
        state.Enemies.Add(new Enemy { X = 8, Y = 7 });
        state.Enemies.Add(new Enemy { X = 12, Y = 4 });
    }
}