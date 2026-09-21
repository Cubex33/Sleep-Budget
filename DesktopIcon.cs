using Cubex33Engine.UI;

public class DesktopIcon : UIElement
{
    private Sprite sprite;
    private Text text;

    public float LabelSpacing { get; set; } = 3f;

    public DesktopIcon(Texture texture, Font font, string name)
    {
        sprite = new Sprite(texture);

        text = new Text(font)
        {
            DisplayedString = name,
            CharacterSize = 12
        };
    }

    public override void Draw(RenderWindow window)
    {
        sprite.Position = Position;
        sprite.Scale = Scale;

        FloatRect spriteBounds = sprite.GetGlobalBounds();
        FloatRect textBounds = text.GetLocalBounds();

        float centerX = Position.X + spriteBounds.Width / 2f;

        text.Position = new Vector2f(
            centerX - textBounds.Width / 2f,
            Position.Y + spriteBounds.Height + LabelSpacing
        );

        window.Draw(sprite);
        window.Draw(text);
    }
}