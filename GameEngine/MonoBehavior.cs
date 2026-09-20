global using SFML.Graphics;
global using SFML.Window;
global using SFML.System;
global using SFML.Audio;

namespace Cubex33Engine
{
    public abstract class MonoBehavior
    {
        public static List<MonoBehavior> Objects = new();

        protected MonoBehavior()
        {
            Objects.Add(this);
        }

        public virtual void Start() { }
        public virtual void Update() { }
    }
}
