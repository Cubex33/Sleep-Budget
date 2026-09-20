using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Cubex33Engine.Debug
{
    public static class Debug
    {
        public static bool isDebug = false;

        private static Font? font;
        private static Text? debugText;
        private static Text? debugOnText;
        private static List<string> lines = new List<string>();
        private static bool subscribed = false;
        private static bool initialized = false;
        private static Dictionary<string, string> values = new();
        private static Dictionary<string, string> lastLoggedValues = new();

        public static void SetValue(string key, string value)
        {
            values[key] = value;
        }

        public static void Init(Font? customFont = null)
        {
            if (initialized)
                return;

            font = customFont ?? GetFallbackFont();

            debugText = new Text(font, "", 15u);
            debugOnText = new Text(font, "Debug", 15u);

            initialized = true;
        }

        private static Font GetFallbackFont()
        {
            string systemDrive = Environment.GetEnvironmentVariable("SystemDrive") ?? "C:";
            return new Font(Path.Combine(systemDrive, @"Windows\Fonts\arial.ttf"));
        }

        public static void Log(string message)
        {
            if (!isDebug) return;

            if (lines.Count > 0 && lines[^1] == message)
                return; 

            if (lines.Count > 50)
                lines.RemoveAt(0);

            lines.Add(message);
        }

        public static void LogValue(string key, string value)
        {
            if (!isDebug) return;

            if (lastLoggedValues.ContainsKey(key) && lastLoggedValues[key] == value)
                return;

            lastLoggedValues[key] = value;
            Log($"{key}: {value}");
        }

        public static void RemoveLog(string message)
        {
            lines.Remove(message);
        }

        public static void RemoveLogsContaining(string text)
        {
            lines.RemoveAll(line => line.Contains(text));
        }

        public static void RemoveLastLog()
        {
            if (lines.Count > 0)
                lines.RemoveAt(lines.Count - 1);
        }

        public static void RemoveValue(string key)
        {
            values.Remove(key);
            lastLoggedValues.Remove(key);

            lines.RemoveAll(line => line.StartsWith($"{key}:"));
        }

        public static void ClearLogs()
        {
            lines.Clear();
            lastLoggedValues.Clear();
        }

        public static void Draw(RenderWindow window)
        {
            if (debugText == null || debugOnText == null) return;
            if (!initialized)
                Init();

            if (!subscribed)
            {
                subscribed = true;
                window.KeyPressed += (_, e) =>
                {
                    if (e.Code == Keyboard.Key.B && e.Control && e.Alt)
                    {
                        isDebug = !isDebug;
                        if (!isDebug)
                        {
                            ClearLogs();
                        }
                    }
                };
            }

            if (!isDebug)
            {
                debugOnText.DisplayedString = "";
                debugText.DisplayedString = "";
                return;
            }

            debugOnText.DisplayedString = "Debug on";

            var displayLines = new List<string>(lines);
            foreach (var kvp in values)
            {
                displayLines.Add($"{kvp.Key}: {kvp.Value}");
            }

            debugText.DisplayedString = string.Join("\n", displayLines);
            debugText.FillColor = new Color(0, 255, 0, 155);
            debugOnText.Position = new Vector2f(10, 10);
            debugText.Position = new Vector2f(10, 30);

            window.Draw(debugText);
            window.Draw(debugOnText);
        }
    }
}