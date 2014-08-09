using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace GameFramework
{
    class Music
    {
        // File Path
        string sPath;

        // Music
        Song music;

        // Volume
        float fVolume;
        // Looping flag
        bool bLooping;

        #region Public Attributes

        // Music 
        public Song Song
        {
            get { return music; }
        }

        // Volume
        public float Volume
        {
            get { return fVolume; }
            set { fVolume = MathHelper.Clamp(value, 0, 1); }
        }

        // Looping
        public bool Looping
        {
            get { return bLooping; }
            set { bLooping = value; }
        }

        #endregion

        public Music(string path, float volume = 1, bool looping = false)
        {
            sPath = path;

            fVolume = volume;
            bLooping = looping;

            Load();
        }

        ~Music()
        {
            MusicPlayer.Instance.RemoveMusic(this);

            music.Dispose();
        }

        // Load
        void Load()
        {
            music = Game1.ContentManager.Load<Song>(sPath);
        }
    }
}
