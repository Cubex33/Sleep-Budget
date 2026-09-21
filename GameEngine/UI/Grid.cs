namespace Cubex33Engine.UI
{
    public class Grid
    {
        public Vector2f Position { get; set; }

        public Vector2f CellSize { get; set; }

        public Vector2f Spacing { get; set; }

        public Grid(Vector2f position, Vector2f cellSize) {
            Position = position;
            CellSize = cellSize;
            Spacing = new Vector2f(0, 0);
        }

        public Vector2f GetPosition(int collumn, int row)
        {
            float x = Position.X + collumn * (CellSize.X + Spacing.X);
            float y = Position.Y + row * (CellSize.Y + Spacing.Y);

            return new Vector2f(x, y);
        }
    }
}
