namespace Code.Scripts.Game
{
    public class GameSettings
    {
        private static GameSettings instance;
        
        public static GameSettings Instance => instance ??= new GameSettings();
        
        public float volume = 0.5f;
        public float music = 0.5f;
    }
}
