namespace Laba3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Тест генератора лабиринта ===");

                var entityFactory = new EntityFactory();
                var levelGenerator = new LevelGenerator(entityFactory);

                Console.WriteLine("Создаем случайный уровень...");
                var gameState = levelGenerator.CreateRandomLevel(21, 11);

                Console.WriteLine($"Уровень создан!");
                Console.WriteLine($"Размер карты: {gameState.Map.Width}x{gameState.Map.Height}");
                Console.WriteLine($"Позиция игрока: ({gameState.PlayerX}, {gameState.PlayerY})");

                // Простая отрисовка карты в консоли
                for (int y = 0; y < gameState.Map.Height; y++)
                {
                    for (int x = 0; x < gameState.Map.Width; x++)
                    {
                        var cell = gameState.Map.GetCell(x, y);
                        Console.Write(cell.IsWalkable ? "." : "#");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.ReadKey();
            }
        }
    }
}