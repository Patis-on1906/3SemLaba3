using System.Windows.Input;

namespace Laba3.WPF
{
    public class WpfInputHandler : IInputHandler
    {
        private InputCommand _currentCommand = InputCommand.None;

        public InputCommand GetCommand()
        {
            var command = _currentCommand;
            _currentCommand = InputCommand.None;
            return command;
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

        public void ProcessKey(Key key)
        {
            _currentCommand = key switch
            {
                Key.W or Key.Up => InputCommand.MoveUp,
                Key.S or Key.Down => InputCommand.MoveDown,
                Key.A or Key.Left => InputCommand.MoveLeft,
                Key.D or Key.Right => InputCommand.MoveRight,
                Key.J => InputCommand.Save,
                Key.L => InputCommand.Load,
                Key.Q or Key.Escape => InputCommand.Quit,
                _ => InputCommand.None
            };
        }
    }
}