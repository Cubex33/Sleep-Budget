namespace Cubex33Engine.SceneManagement
{
    public static class SceneManager
    {
        private static GameState _currentState;

        public static event Action<GameState>? OnStateChanged;

        public static GameState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;
                _currentState = value;
                OnStateChanged?.Invoke(_currentState);
            }
        }
    }
}