using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameFramework
{
    class AudioManager
    {
        // Singleton Instance
        static AudioManager instance;

        // List of Sounds
        List<Sound> lSoundList;

        // Master Volume
        float fMasterVolume;
        // Sound Volume
        float fSoundVolume;
        // Music Volume
        float fMusicVolume;

        // Mute flag
        bool bMute;

        #region Public Attributes

        // Instance
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }

                return instance;
            }
        }

        // Master Volume
        public float MasterVolume
        {
            get { return fMasterVolume; }
            set 
            {
                fMasterVolume = MathHelper.Clamp(value, 0, 1);
                
                // Update the volume for all sounds
                UpdateVolume();
                // Update the MusicPlayer Volume
                MusicPlayer.Instance.UpdateVolume();
            }
        }

        // Sound Volume
        public float SoundVolume
        {
            get { return fSoundVolume; }
            set 
            {
                fSoundVolume = MathHelper.Clamp(value, 0, 1);

                // Update the volume for all sounds
                UpdateVolume();
            }
        }

        // Music Volume
        public float MusicVolume
        {
            get { return fMusicVolume; }
            set 
            { 
                fMusicVolume = MathHelper.Clamp(value, 0, 1);

                // Update the MusicPlayer Volume
                MusicPlayer.Instance.UpdateVolume();
            }
        }

        // Mute
        public bool Mute
        {
            get { return bMute; }
            set 
            { 
                bMute = value;

                MusicPlayer.Instance.Mute = bMute;
            }
        }

        #endregion

        public AudioManager()
        {
            lSoundList = new List<Sound>();

            fMasterVolume = 1;
            fSoundVolume = 1;
            fMusicVolume = 1;
        }

        // Sets up Volume and Mute variables
        public void Setup(float masterVol, float soundVol, float musicVol, bool mute)
        {
            fMasterVolume = masterVol;
            fSoundVolume = soundVol;
            fMusicVolume = musicVol;
            bMute = mute;
        }

        // Adds a sound to the list
        public void AddSound(Sound sound)
        {
            lSoundList.Add(sound);
        }

        // Removes a sound from the list
        public void RemoveSound(Sound sound)
        {
            lSoundList.Remove(sound);
        }

        // Update the volume for all sounds
        void UpdateVolume()
        {
            foreach (Sound sound in lSoundList)
            {
                sound.UpdateVolume();
            }
        }
    }
}
