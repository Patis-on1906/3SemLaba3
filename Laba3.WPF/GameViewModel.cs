using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laba3.WPF.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private GameState _gameState;
        private string _playerInfo;
        private PropertyChangedEventHandler? _propertyChanged;

        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnPropertyChanged();
                UpdatePlayerInfo();
            }
        }

        public string PlayerInfo
        {
            get => _playerInfo;
            set
            {
                _playerInfo = value;
                OnPropertyChanged();
            }
        }

        private void UpdatePlayerInfo()
        {
            if (GameState?.Player != null)
            {
                PlayerInfo = $"HP: {GameState.Player.Health}/{GameState.Player.MaxHealth} | " +
                            $"Score: {GameState.Player.Score}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => _propertyChanged += value;
            remove => _propertyChanged -= value;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}