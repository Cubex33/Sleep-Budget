using Cubex33Engine;
using Cubex33Engine.SceneManagement;
using Cubex33Engine.UI;
using Cubex33Engine.Debug;

namespace Sleep_Budget
{
    public class Menu : MonoBehavior
    {
        RenderWindow window;

        Color backgraundColor = new Color { R = 0, G = 127, B = 125 };

        Font font = new Font(@"./Assets/Fonts/UpheavalPro.ttf");

        Texture thisComputerTexture = new(@"./Assets/Sprites/ThisComputerIcon.png");

        Texture trashTexture = new(@"./Assets/Sprites/TrashIcon.png");

        DesktopIcon thisComputer;
        DesktopIcon trash;

        public Menu(RenderWindow _window)
        {
            window = _window;

            var grid = new Grid(new Vector2f(20, 10), new Vector2f(80, 90));

            thisComputer = new(thisComputerTexture, font, "This Computer");
            thisComputer.Scale = new Vector2f(2f, 2f);

            trash = new(trashTexture, font, "Trash");
            trash.Scale = new Vector2f(1.6f, 1.6f);

            thisComputer.Position = grid.GetPosition(0, 0);
            trash.Position = grid.GetPosition(0, 1);
        }

        public override void Update()
        {
            if (SceneManager.CurrentState != GameState.MainMenu) return;

            Draw();
        }

        private void Draw()
        {
            window.Clear(backgraundColor);

            thisComputer.Draw(window);
            trash.Draw(window);

            Debug.Draw(window);
        }
    }
}
