namespace Cubex33Engine.UI
{
    public class Button
    {
        public RectangleShape Shape { get; }
        public Text Text { get; }

        public event Action? Clicked;
        public event Action? Hovered;
        public event Action? Unhovered;

        public bool IsHovered { get; private set; }

        private bool wasMousePressed = false;

        public Button(Font font, string text, uint characterSize, Vector2f position, Vector2f size, Texture? texture = null)
        {
            Shape = new RectangleShape(size)
            {
                Position = position,
                FillColor = Color.White
            };

            if (texture != null)
                Shape.Texture = texture;

            Text = new Text(font, text)
            {
                CharacterSize = characterSize
            };

            CenterText();
        }

        private void CenterText()
        {
            FloatRect bounds = Text.GetLocalBounds();
            Text.Origin = new Vector2f(
                bounds.Position.X + bounds.Size.X / 2f,
                bounds.Position.Y + bounds.Size.Y / 2f
            );

            Text.Position = new Vector2f(
                Shape.Position.X + Shape.Size.X / 2f,
                Shape.Position.Y + Shape.Size.Y / 2f
            );
        }

        public bool Contains(Vector2f point)
        {
            return Shape.GetGlobalBounds().Contains(point);
        }

        public void Update(RenderWindow window)
        {
            Vector2i mousePixel = Mouse.GetPosition(window);
            Vector2f mousePos = window.MapPixelToCoords(mousePixel);

            bool isInside = Contains(mousePos);
            bool isPressed = Mouse.IsButtonPressed(Mouse.Button.Left);

            if (isInside && !IsHovered)
            {
                IsHovered = true;
                Hovered?.Invoke();
            }
            else if (!isInside && IsHovered)
            {
                IsHovered = false;
                Unhovered?.Invoke();
            }

            if (isInside && isPressed && !wasMousePressed)
            {
                Clicked?.Invoke();
            }

            wasMousePressed = isPressed;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Shape);
            window.Draw(Text);
        }

        public void SetTexture(Texture texture)
        {
            Shape.Texture = texture;
            Shape.FillColor = Color.White;
        }
    }
}