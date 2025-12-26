namespace Laba3;

public class InputHandler
{
    public enum GameCommand
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Save,
        Load,
        Quit,
        None
    }

    public GameCommand GetCommand()
    {
        var key = Console.ReadKey(true).Key;

        return key switch
        {
            ConsoleKey.W or ConsoleKey.UpArrow => GameCommand.MoveUp,
            ConsoleKey.S or ConsoleKey.DownArrow => GameCommand.MoveDown,
            ConsoleKey.A or ConsoleKey.LeftArrow => GameCommand.MoveLeft,
            ConsoleKey.D or ConsoleKey.RightArrow => GameCommand.MoveRight,
            ConsoleKey.J => GameCommand.Save,
            ConsoleKey.L => GameCommand.Load,
            ConsoleKey.Q or ConsoleKey.Escape => GameCommand.Quit,
            _ => GameCommand.None
        };
    }

    public (int dx, int dy) GetMovementVector(GameCommand command)
    {
        return command switch
        {
            GameCommand.MoveUp => (0, -1),
            GameCommand.MoveDown => (0, 1),
            GameCommand.MoveLeft => (-1, 0),
            GameCommand.MoveRight => (1, 0),
            _ => (0, 0)
        };
    }
}