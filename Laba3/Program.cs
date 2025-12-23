namespace Laba3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;

            // 1. Создаем сервисы
            ISaveService saveService = new JsonSaveService();
            IRenderer renderer = new ConsoleRenderer();
        
            // 2. Генерируем уровень
            var generator = new LevelGenerator();
            var state = generator.CreateTestLevel();

            // 3. Внедряем зависимости в контроллер
            var controller = new GameController(state, renderer, saveService);
        
            // 4. Запускаем
            controller.Run();
        }
    }
}