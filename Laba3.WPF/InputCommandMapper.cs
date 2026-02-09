namespace Laba3.WPF
{
    public class InputCommandMapper : IInputHandler
    {
        public InputCommand GetCommand()
        {
            return InputCommand.None;
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
}
