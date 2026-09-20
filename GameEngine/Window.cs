namespace Cubex33Engine
{
    public static class Window
    {
        public static RenderWindow Create(
            VideoMode videoMode,
            string title,
            uint fps,
            Styles style = Styles.None,
            State state = State.Windowed)
        {
            RenderWindow window = new(
                videoMode,
                title,
                style,
                state
            );

            window.SetFramerateLimit(fps);

            window.Closed += (_, _) => window.Close();

            return window;
        }
    }
}