using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameFramework
{
    class AudioXMLMapper
    {
        // Singleton Instance
        static AudioXMLMapper instance;

        // XML File Path
        const string S_AUDIO_FILEPATH = "Config/Audio.xml";

        #region Public Attributes

        // Instance
        public static AudioXMLMapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioXMLMapper();
                }

                return instance;
            }
        }

        #endregion

        // Load current audio setup from Audio.xml
        public void LoadAudioConfig()
        {
            // XML Document
            XmlDocument document = new XmlDocument();
            document.Load(S_AUDIO_FILEPATH);

            // Root Node
            XmlNode node = document.SelectSingleNode("AudioSettings");

            float masterVolume;
            float soundVolume;
            float musicVolume;
            bool mute;

            float.TryParse(node.SelectSingleNode("MasterVolume").InnerText, out masterVolume);
            float.TryParse(node.SelectSingleNode("SoundVolume").InnerText, out soundVolume);
            float.TryParse(node.SelectSingleNode("MusicVolume").InnerText, out musicVolume);
            bool.TryParse(node.SelectSingleNode("Mute").InnerText, out mute);

            AudioManager.Instance.Setup(masterVolume, soundVolume, musicVolume, mute);
        }

        // Save current audio setup to Audio.xml
        public void SaveAudioConfig()
        {
            // New XML Document
            XmlDocument document = new XmlDocument();

            // Root Node
            XmlNode rootNode = document.CreateElement("AudioSettings");
            document.AppendChild(rootNode);

            // Master Volume Node
            XmlNode masterVolume = document.CreateElement("MasterVolume");
            masterVolume.InnerText = AudioManager.Instance.MasterVolume.ToString();
            rootNode.AppendChild(masterVolume);

            // Sound Volume Node
            XmlNode soundVolume = document.CreateElement("SoundVolume");
            soundVolume.InnerText = AudioManager.Instance.SoundVolume.ToString();
            rootNode.AppendChild(soundVolume);

            // Music Volume Node
            XmlNode musicVolume = document.CreateElement("MusicVolume");
            musicVolume.InnerText = AudioManager.Instance.MusicVolume.ToString();
            rootNode.AppendChild(musicVolume);

            // Mute Node
            XmlNode mute = document.CreateElement("Mute");
            mute.InnerText = AudioManager.Instance.Mute.ToString();
            rootNode.AppendChild(mute);

            // Save the final document
            document.Save(S_AUDIO_FILEPATH);
        }
    }
}
