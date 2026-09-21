using System;
using System.Collections.Generic;
using System.Text;

namespace Cubex33Engine.UI;

public abstract class UIElement
{
    public Vector2f Position { get; set; }
    public Vector2f Scale { get; set; }

    public abstract void Draw(RenderWindow window);
}
