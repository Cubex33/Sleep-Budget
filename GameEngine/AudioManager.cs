using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubex33Engine.Debug;

namespace Cubex33Engine
{
    public static class AudioManager
    {
        private static Music currentMusic;

        public static void Play(string path, float value = 50f, bool loop = true)
        {
            currentMusic?.Stop();
            currentMusic?.Dispose();
            currentMusic = null;     

            currentMusic = new Music(path);
            currentMusic.IsLooping = loop;
            currentMusic.Play();
            currentMusic.Volume = value;
            Debug.Debug.Log(value.ToString());
        }

        public static void SetVolume(float volume)
        {
            if (currentMusic != null)
                currentMusic.Volume = volume;
        }


        public static void Stop()
        {
            currentMusic?.Stop();
            currentMusic?.Dispose(); 
            currentMusic = null;
        }
    }
}
