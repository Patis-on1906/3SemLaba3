namespace Laba3;

public interface IInputHandler
{
    InputCommand GetCommand();
    (int dx, int dy) GetMovementVector(InputCommand command);
}

public enum InputCommand
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
