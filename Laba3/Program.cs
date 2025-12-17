namespace Laba3
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameState state = GameController.CreateTestLevel();
                var controller = new GameController(state);

                if (File.Exists("save.json"))
                {
                    Console.WriteLine("Найден файл сохранения. Загрузить? (Y/N)");
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.Enter)
                    {
                        controller.LoadGame("save.json");
                    }
                }

                controller.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }
    }
}