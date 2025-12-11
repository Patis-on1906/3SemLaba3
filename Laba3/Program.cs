namespace Laba3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var state = GameController.CreateTestLevel();
            var controller = new GameController(state);
            controller.Run();
        }
    }
}