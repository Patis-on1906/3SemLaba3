namespace Laba3;

public class ConsoleInputHandler : IInputHandler
{
    public InputCommand GetCommand()
    {
        var key = Console.ReadKey(true).Key;

        return key switch
        {
            ConsoleKey.W or ConsoleKey.UpArrow => InputCommand.MoveUp,
            ConsoleKey.S or ConsoleKey.DownArrow => InputCommand.MoveDown,
            ConsoleKey.A or ConsoleKey.LeftArrow => InputCommand.MoveLeft,
            ConsoleKey.D or ConsoleKey.RightArrow => InputCommand.MoveRight,
            ConsoleKey.J => InputCommand.Save,
            ConsoleKey.L => InputCommand.Load,
            ConsoleKey.Q or ConsoleKey.Escape => InputCommand.Quit,
            _ => InputCommand.None
        };
    }

    public (int dx, int dy) GetMovementVector(InputCommand command)
    {
        return command switch
        {
            InputCommand.MoveUp => (0, -1),
            InputCommand.MoveDown => (0, 1),
            InputCommand.MoveLeft => (-1, 0),
            InputCommand.MoveRight => (1, 0),
            _ => (0, 0)
        };
    }
}