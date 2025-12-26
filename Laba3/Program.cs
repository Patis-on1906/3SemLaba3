namespace Laba3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.CursorVisible = false;
                Console.Title = "Подземелье с сокровищами";

                var entityFactory = new EntityFactory();
                var renderer = new ConsoleRenderer();
                var saveService = new JsonSaveService();
                var inputHandler = new ConsoleInputHandler();
                var gameLogic = new GameLogicService(entityFactory);
                var levelGenerator = new LevelGenerator(entityFactory);

                GameState gameState;

                if (args.Contains("--load") || args.Contains("-l"))
                {
                    gameState = saveService.Load() ?? levelGenerator.CreateTestLevel();
                }
                else if (args.Contains("--random") || args.Contains("-r"))
                {
                    gameState = levelGenerator.CreateRandomLevel(60, 30);
                }
                else
                {
                    gameState = levelGenerator.CreateTestLevel();
                }

                var controller = new GameController(
                    gameState,
                    renderer,
                    saveService,
                    inputHandler,
                    gameLogic
                );

                controller.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
