namespace Cubex33Engine.UI
{
    public class Slider
    {
        public RectangleShape Track { get; }
        public RectangleShape Fill { get; }
        public RectangleShape Handle { get; }

        public event Action<float>? ValueChanged;
        public event Action? HandleHovered;
        public event Action? HandleUnhovered;

        private float _minValue;
        private float _maxValue;
        private float _value;

        private float _width;
        private float _trackHeight;
        private float _handleRadius;

        private bool _isDragging = false;
        private bool _wasMousePressed = false;

        private Color _trackFallbackColor = new Color(180, 180, 180);
        private Color _fillFallbackColor = new Color(80, 140, 220);
        private Color _handleFallbackColor = Color.White;

        public float Value
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, _minValue, _maxValue);
                UpdateHandlePosition();
                ValueChanged?.Invoke(_value);
            }
        }

        public bool IsHandleHovered { get; private set; }

        /// <summary>
        /// Толщина дорожки/заливки слайдера. При изменении пересчитывает
        /// размеры и Origin у Track и Fill, не трогая текущее значение Value.
        /// </summary>
        public float TrackHeight
        {
            get => _trackHeight;
            set
            {
                _trackHeight = MathF.Max(0f, value);
                ApplyTrackHeight();
            }
        }

        /// <summary>
        /// Длина дорожки слайдера. При изменении пересчитывает позицию Fill/Handle
        /// под текущее значение Value.
        /// </summary>
        public float Width
        {
            get => _width;
            set
            {
                _width = MathF.Max(0f, value);
                Track.Size = new Vector2f(_width, Track.Size.Y);
                UpdateHandlePosition();
            }
        }

        /// <summary>
        /// Радиус (половина стороны) квадратной ручки слайдера.
        /// </summary>
        public float HandleRadius
        {
            get => _handleRadius;
            set
            {
                _handleRadius = MathF.Max(0f, value);
                float diameter = _handleRadius * 2f;
                Handle.Size = new Vector2f(diameter, diameter);
                Handle.Origin = new Vector2f(_handleRadius, _handleRadius);
            }
        }

        private void ApplyTrackHeight()
        {
            Track.Size = new Vector2f(Track.Size.X, _trackHeight);
            Track.Origin = new Vector2f(0f, _trackHeight / 2f);

            Fill.Size = new Vector2f(Fill.Size.X, _trackHeight);
            Fill.Origin = new Vector2f(0f, _trackHeight / 2f);
        }

        public Slider(
            Vector2f position,
            float width,
            float trackHeight,
            float handleRadius,
            float minValue = 0f,
            float maxValue = 1f,
            float initialValue = 0f,
            Texture? trackTexture = null,
            Texture? fillTexture = null,
            Texture? handleTexture = null)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _width = width;
            _trackHeight = trackHeight;
            _handleRadius = handleRadius;

            Track = new RectangleShape(new Vector2f(width, trackHeight))
            {
                Position = position,
                Origin = new Vector2f(0f, trackHeight / 2f)
            };

            Fill = new RectangleShape(new Vector2f(0f, trackHeight))
            {
                Position = position,
                Origin = new Vector2f(0f, trackHeight / 2f)
            };

            float diameter = handleRadius * 2f;
            Handle = new RectangleShape(new Vector2f(diameter, diameter))
            {
                Origin = new Vector2f(handleRadius, handleRadius)
            };

            SetTrackTexture(trackTexture);
            SetFillTexture(fillTexture);
            SetHandleTexture(handleTexture);

            _value = Math.Clamp(initialValue, _minValue, _maxValue);
            UpdateHandlePosition();
        }

        /// <summary>
        /// Задаёт текстуру для дорожки слайдера. Если передать null — дорожка
        /// вернётся к заливке цветом (см. SetTrackColor / SetColors).
        /// </summary>
        public void SetTrackTexture(Texture? texture)
        {
            ApplyTextureOrColor(Track, texture, _trackFallbackColor);
        }

        /// <summary>
        /// Задаёт текстуру для заполненной части слайдера. Если передать null —
        /// заполнение вернётся к заливке цветом.
        /// </summary>
        public void SetFillTexture(Texture? texture)
        {
            ApplyTextureOrColor(Fill, texture, _fillFallbackColor);
        }

        /// <summary>
        /// Задаёт текстуру для ручки слайдера. Если передать null — ручка
        /// вернётся к заливке цветом.
        /// </summary>
        public void SetHandleTexture(Texture? texture)
        {
            ApplyTextureOrColor(Handle, texture, _handleFallbackColor);
        }

        private static void ApplyTextureOrColor(RectangleShape shape, Texture? texture, Color fallbackColor)
        {
            Vector2f originalSize = shape.Size;

            shape.Texture = texture;

            if (texture != null)
            {
                shape.Size = originalSize;
                shape.FillColor = Color.White;
            }
            else
            {
                shape.FillColor = fallbackColor;
            }
        }

        private void UpdateHandlePosition()
        {
            float t = (_value - _minValue) / (_maxValue - _minValue);
            float x = Track.Position.X + t * Track.Size.X;
            float y = Track.Position.Y;

            Handle.Position = new Vector2f(x, y);
            Fill.Size = new Vector2f(t * Track.Size.X, Fill.Size.Y);
        }

        private bool HandleContains(Vector2f point)
        {
            return Handle.GetGlobalBounds().Contains(point);
        }

        public void Update(RenderWindow window)
        {
            Vector2i mousePixel = Mouse.GetPosition(window);
            Vector2f mousePos = window.MapPixelToCoords(mousePixel);

            bool isPressed = Mouse.IsButtonPressed(Mouse.Button.Left);
            bool onHandle = HandleContains(mousePos);

            if (onHandle && !IsHandleHovered)
            {
                IsHandleHovered = true;
                HandleHovered?.Invoke();
            }
            else if (!onHandle && IsHandleHovered && !_isDragging)
            {
                IsHandleHovered = false;
                HandleUnhovered?.Invoke();
            }

            if (isPressed && !_wasMousePressed && onHandle)
                _isDragging = true;

            if (!isPressed)
            {
                _isDragging = false;

                if (!onHandle && IsHandleHovered)
                {
                    IsHandleHovered = false;
                    HandleUnhovered?.Invoke();
                }
            }

            if (_isDragging)
            {
                float trackLeft = Track.Position.X;
                float trackRight = Track.Position.X + Track.Size.X;

                float t = (mousePos.X - trackLeft) / (trackRight - trackLeft);
                t = Math.Clamp(t, 0f, 1f);

                float newValue = _minValue + t * (_maxValue - _minValue);

                if (Math.Abs(newValue - _value) > float.Epsilon)
                {
                    _value = newValue;
                    UpdateHandlePosition();
                    ValueChanged?.Invoke(_value);
                }
            }

            _wasMousePressed = isPressed;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Track);
            window.Draw(Fill);
            window.Draw(Handle);
        }

        /// <summary>
        /// Устанавливает цвета как fallback-значения и сразу применяет их,
        /// убирая текущие текстуры (если были).
        /// </summary>
        public void SetColors(Color track, Color fill, Color handle)
        {
            _trackFallbackColor = track;
            _fillFallbackColor = fill;
            _handleFallbackColor = handle;

            SetTrackTexture(null);
            SetFillTexture(null);
            SetHandleTexture(null);
        }
    }
}