using Cubex33Engine;
using Cubex33Engine.SceneManagement;
using Cubex33Engine.Debug;

namespace SleepBudget
{
    public class SplashImage : MonoBehavior
    {
        public Sprite splashTexture;
        public Texture splashTextureTexture;

        RenderWindow window;

        private Clock clock = new Clock();
        private const float displayDuration = 3f;
        private bool sceneChanged = false;

        public SplashImage(RenderWindow _window)
        {
            window = _window;
            splashTextureTexture = new Texture(@"./Assets/Sprites/SplashImage.png");
            splashTexture = new Sprite(splashTextureTexture);
        }

        public override void Start()
        {
            splashTexture.Scale = new Vector2f(0.7f, 0.7f);
            splashTexture.Position = new Vector2f(
                (window.Size.X / 2 - splashTextureTexture.Size.X * splashTexture.Scale.X / 2),
                (window.Size.Y / 2 - splashTextureTexture.Size.Y * splashTexture.Scale.Y / 2)
            );

            clock.Restart();
        }

        public override void Update()
        {
            if (SceneManager.CurrentState != GameState.Splash)
                return;

            window.Draw(splashTexture);

            if (!sceneChanged && clock.ElapsedTime.AsSeconds() >= displayDuration)
            {
                sceneChanged = true;
                SceneManager.CurrentState = GameState.MainMenu;
            }
            Debug.Draw(window);
        }
    }
}