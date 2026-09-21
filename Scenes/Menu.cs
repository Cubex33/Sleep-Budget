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
        Texture fileTexture = new(@"./Assets/Sprites/label.png");

        DesktopIcon thisComputer;
        DesktopIcon trash;
        DesktopIcon gameLabel;
        DesktopIcon settingLabel;

        public Menu(RenderWindow _window)
        {
            window = _window;

            var grid = new Grid(new Vector2f(20, 10), new Vector2f(80, 90));

            thisComputer = new(thisComputerTexture, font, "This Computer");

            trash = new(trashTexture, font, "Trash");

            gameLabel = new(fileTexture, font, "Game.exe");

            settingLabel = new(fileTexture, font, "Setting.exe");

            thisComputer.Position = grid.GetPosition(0, 0);
            trash.Position = grid.GetPosition(0, 1);
            gameLabel.Position = grid.GetPosition(0, 2);
            settingLabel.Position = grid.GetPosition(0, 3);
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
            gameLabel.Draw(window);
            settingLabel.Draw(window);

            Debug.Draw(window);
        }
    }
}
