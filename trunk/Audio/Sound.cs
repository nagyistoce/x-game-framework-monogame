using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameFramework
{
    class Sound
    {
        // File Path
        string sPath;

        // Sound 
        SoundEffect sound;
        // Sound Instance
        SoundEffectInstance soundInstance;

        // Sound Instance Volume (Set to 0 when muted)
        float fVolume;
        // Stored Volume for the Sound (Used when the sound is muted)
        float fSetVolume;

        // Mute flag
        bool bMute;

        #region Public Attributes

        // Volume
        public float Volume
        {
            get { return fVolume; }
            set { fVolume = MathHelper.Clamp(value, 0, 1); }
        }

        // Pan
        public float Pan
        {
            get { return soundInstance.Pan; }
            set { soundInstance.Pan = MathHelper.Clamp(value, -1, 1); }
        }

        // Pitch
        public float Pitch
        {
            get { return soundInstance.Pitch; }
            set { soundInstance.Pitch = MathHelper.Clamp(value, -1, 1); }
        }

        // Looping
        public bool Looping
        {
            get { return soundInstance.IsLooped; }
            set { soundInstance.IsLooped = value; }
        }

        // Mute
        public bool Mute
        {
            get { return bMute; }
            set 
            { 
                bMute = value;

                // If muted, volume is 0
                if (bMute)
                {
                    fVolume = 0;
                }
                else // Else, volume is restored to original value
                {
                    fVolume = fSetVolume;
                }

                // Update Volume
                UpdateVolume();
            }
        }

        #endregion

        public Sound(string path, float volume = 1)
        {
            // Add this instance to the AudioManager SoundList
            AudioManager.Instance.AddSound(this);

            sPath = path;

            fVolume = volume;
            fSetVolume = fVolume;
            bMute = false;

            Load();
        }

        ~Sound()
        {
            // Remove this instance from the AudioManager SoundList
            AudioManager.Instance.RemoveSound(this);

            soundInstance.Dispose();
            sound.Dispose();
        }

        // Load
        void Load()
        {
            // Load the Sound
            sound = Game1.ContentManager.Load<SoundEffect>(sPath);
            // Create an instance to play the sound
            soundInstance = sound.CreateInstance();

            UpdateVolume();
        }

        // Updates the Sound Volume
        public void UpdateVolume()
        {
            soundInstance.Volume = fVolume * AudioManager.Instance.MasterVolume * AudioManager.Instance.SoundVolume;
        }

        // Play Sound
        public void Play()
        {
            soundInstance.Play();
        }

        // Stop Sound
        public void Stop()
        {
            soundInstance.Stop();
        }

        // Pause Sound
        public void Pause()
        {
            soundInstance.Pause();
        }

        // Resume Sound
        public void Unpause()
        {
            soundInstance.Resume();
        }
    }
}
