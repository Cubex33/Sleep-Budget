using System.Text;

namespace Cubex33Engine
{
    public static class SaveManager
    {
        private static readonly string SavePath = "./Save/save.dat";
        private static readonly byte XorKey = 0x5A; // можешь поменять

        private static Dictionary<string, string> _data = new();

        public static void SetInt(string key, int value) => _data[key] = $"i:{value}";
        public static void SetFloat(string key, float value) =>
    _data[key] = $"f:{value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        public static void SetString(string key, string value) => _data[key] = $"s:{value}";

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (_data.TryGetValue(key, out var raw) && raw.StartsWith("i:"))
                return int.TryParse(raw[2..], out var v) ? v : defaultValue;
            return defaultValue;
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            if (_data.TryGetValue(key, out var raw) && raw.StartsWith("f:"))
                return float.TryParse(raw[2..], System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : defaultValue;
            return defaultValue;
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (_data.TryGetValue(key, out var raw) && raw.StartsWith("s:"))
                return raw[2..];
            return defaultValue;
        }

        public static bool HasKey(string key) => _data.ContainsKey(key);
        public static void DeleteKey(string key) => _data.Remove(key);
        public static void DeleteAll() => _data.Clear();

        public static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);

            var sb = new StringBuilder();
            foreach (var (key, value) in _data)
                sb.AppendLine($"{key}={value}");

            byte[] plain = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] encrypted = XorEncrypt(plain);

            File.WriteAllBytes(SavePath, encrypted);
        }

        public static void Load()
        {
            _data.Clear();

            if (!File.Exists(SavePath)) return;

            byte[] encrypted = File.ReadAllBytes(SavePath);
            byte[] plain = XorEncrypt(encrypted); 
            string text = Encoding.UTF8.GetString(plain);

            foreach (var line in text.Split('\n'))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                int sep = trimmed.IndexOf('=');
                if (sep < 0) continue;

                string key = trimmed[..sep];
                string value = trimmed[(sep + 1)..];
                _data[key] = value;
            }
        }

        private static byte[] XorEncrypt(byte[] data)
        {
            var result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = (byte)(data[i] ^ XorKey);
            return result;
        }
    }
}