using Cubex33Engine;
using Cubex33Engine.Debug;
using Cubex33Engine.SceneManagement;
using Sleep_Budget;

namespace SleepBudget
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            RenderWindow window = Cubex33Engine.Window.Create(
                VideoMode.DesktopMode,
                "Sleep Budget",
                144,
                Styles.None,
                State.Fullscreen
            );

            Debug.isDebug = false;

            Debug.Init(new Font(@"./Assets/Fonts/UpheavalPro.ttf"));

            SceneManager.CurrentState = GameState.Splash;

            new SplashImage(window);
            new Menu(window);

            foreach(var obj in MonoBehavior.Objects)
            {
                obj.Start();
            }

            while(window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();
                foreach (var obj in MonoBehavior.Objects)
                {
                    obj.Update();
                }

                window.Display();
            }
        }
    }
}