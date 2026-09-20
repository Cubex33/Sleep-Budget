using Cubex33Engine;
using Cubex33Engine.SceneManagement;
using Cubex33Engine.UI;
using Cubex33Engine.Debug;

//TODO: Add icon grid system. Refactoring code

namespace Sleep_Budget
{
    public class Menu : MonoBehavior
    {
        RenderWindow window;

        Color backgraundColor = new Color { R = 0, G = 127, B = 125 };

        Font font = new Font(@"./Assets/Fonts/UpheavalPro.ttf");

        Texture thisComputerTexture = new(@"./Assets/Sprites/ThisComputerIcon.png");
        Sprite thisComputerSprite;
        Text thisComputerLabel;

        Texture trashTexture = new(@"./Assets/Sprites/TrashIcon.png");
        Sprite trashSprite;
        Text trashLabel;

        public Menu(RenderWindow _window)
        {
            window = _window;
            thisComputerSprite = new(thisComputerTexture);
            thisComputerLabel = new(font)
            {
                DisplayedString = "This Computer",
                CharacterSize = 12
            };

            trashSprite = new(trashTexture);
            trashLabel = new(font)
            {
                DisplayedString = "Trash",
                CharacterSize = 12
            };

            trashSprite.Scale = new Vector2f(1.6f, 1.6f);
            trashSprite.Position = new Vector2f(25, 100);
            trashLabel.Position = new Vector2f(30, 150);

            thisComputerSprite.Scale = new Vector2f(2, 2);
            thisComputerSprite.Position = new Vector2f(20, 10);
            thisComputerLabel.Position = new Vector2f(10, 70);
        }

        public override void Update()
        {
            if (SceneManager.CurrentState != GameState.MainMenu) return;

            Debug.Log($"{thisComputerLabel.Position.X} {thisComputerLabel.Position.Y}");
            Draw();
        }

        private void Draw()
        {
            window.Clear(backgraundColor);
            window.Draw(thisComputerSprite);
            window.Draw(thisComputerLabel);
            window.Draw(trashSprite);
            window.Draw(trashLabel);

            Debug.Draw(window);
        }
    }
}
