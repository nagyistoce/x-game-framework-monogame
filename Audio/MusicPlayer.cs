using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Media;

namespace GameFramework
{
    class MusicPlayer
    {
        // Singleton Instance
        static MusicPlayer instance;

        // Music List
        List<Music> lMusicList;
        // Dictionary of Track IDs
        Dictionary<string, int> dTrackIds;

        // Current Music 
        Music currentMusic;

        #region Public Attributes

        // Instance
        public static MusicPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MusicPlayer();
                }

                return instance;
            }
        }

        // Mute
        public bool Mute
        {
            get { return MediaPlayer.IsMuted; }
            set { MediaPlayer.IsMuted = value; }
        }

        #endregion

        public MusicPlayer()
        {
            lMusicList = new List<Music>();
            dTrackIds = new Dictionary<string, int>();
        }

        ~MusicPlayer()
        {
            // Clear All Music
            ClearMusic();
        }

        // Add Music to the Music List
        public void AddMusic(Music newMusic, string trackName)
        {
            // Loop through the music list to check for duplicate music
            for (int i = 0; i < lMusicList.Count; i++)
            {
                if (newMusic == lMusicList[i])
                {
                    return;
                }
            }

            // If there are music in the Music List
            if (dTrackIds.Count > 0)
            {
                // Check for a duplicate Track Name
                if (dTrackIds.ContainsKey(trackName))
                {
                    return;
                }
            }

            lMusicList.Add(newMusic);
            dTrackIds.Add(trackName, lMusicList.Count - 1);
        }

        // Remove Music from the Music List
        public void RemoveMusic(Music music)
        {
            if (lMusicList.Count > 0)
            {
                lMusicList.Remove(music);
            }
        }

        // Clear All Music from the Music List
        public void ClearMusic()
        {
            foreach (Music music in lMusicList)
            {
                music.Song.Dispose();
            }

            lMusicList.Clear();
        }

        // Update Volume
        public void UpdateVolume()
        {
            if (currentMusic != null)
            {
                MediaPlayer.Volume = currentMusic.Volume *
                    AudioManager.Instance.MasterVolume * AudioManager.Instance.MusicVolume;
            }
        }

        // Play Music
        public void Play(string trackName)
        {
            // If the track doesn't exist then return
            if (!dTrackIds.ContainsKey(trackName))
            {
                return;
            }

            // If there is Music in the Music List
            if (lMusicList.Count > 0)
            {
                // Change the track
                currentMusic = lMusicList[dTrackIds[trackName]];

                // Update Volume and Looping
                UpdateVolume();
                MediaPlayer.IsRepeating = currentMusic.Looping;

                // Stop the previous track
                MediaPlayer.Stop();
                // Play the new track
                MediaPlayer.Play(currentMusic.Song);
            }
        }

        // Stop Music
        public void Stop()
        {
            if (lMusicList.Count > 0)
            {
                MediaPlayer.Stop();
            }
        }

        // Pause Music
        public void Pause()
        {
            if (lMusicList.Count > 0)
            {
                MediaPlayer.Pause();
            }
        }

        // Resume Music
        public void Unpause()
        {
            if (lMusicList.Count > 0)
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
            }
        }
    }
}
