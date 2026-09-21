using Cubex33Engine.UI;

public class DesktopIcon : UIElement
{
    private Sprite sprite;
    private Text text;

    public Vector2f IconSize { get; set; } = new Vector2f(64, 64);

    public DesktopIcon(Texture texture, Font font, string name)
    {
        sprite = new Sprite(texture);

        text = new Text(font)
        {
            DisplayedString = name,
            CharacterSize = 12
        };

        sprite.Scale = new Vector2f(
            IconSize.X / texture.Size.X,
            IconSize.Y / texture.Size.Y
        );
    }

    public override void Draw(RenderWindow window)
    {
        sprite.Position = Position;

        var spriteBounds = sprite.GetGlobalBounds();
        var textBounds = text.GetLocalBounds();

        text.Position = new Vector2f(
            Position.X + spriteBounds.Width / 2f - textBounds.Width / 2f,
            Position.Y + spriteBounds.Height + 3
        );

        window.Draw(sprite);
        window.Draw(text);
    }
}