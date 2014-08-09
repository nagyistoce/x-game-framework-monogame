using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameFramework
{
    class InputXMLMapper
    {
        // Singleton Instance
        static InputXMLMapper instance;

        // XML File Path
        const string S_INPUT_FILEPATH = "Config/Input.xml";

        #region Public Attributes

        // Instance
        public static InputXMLMapper Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new InputXMLMapper();
                }

                return instance;
            }
        }

        #endregion

        // Load current input setup from Input.xml
        public void LoadInputConfig()
        {
            // XML Document
            XmlDocument document = new XmlDocument();
            document.Load(S_INPUT_FILEPATH);

            // Setup the number of players
            int numberOfPlayers = 1;
            int.TryParse(document.SelectSingleNode("Inputs").SelectSingleNode("NumberOfPlayers").InnerText, out numberOfPlayers);
            InputManager.NumberOfPlayers = numberOfPlayers;

            // Find the Input Nodes
            XmlNodeList xmlNodes = document.SelectSingleNode("Inputs").SelectNodes("Input");

            // Loop through each input node and add the inputs to the InputManager
            foreach (XmlNode xmlNode in xmlNodes)
            {
                string name = xmlNode.Attributes["Name"].Value;

                InputNode inputNode = new InputNode();

                Keys positiveKey; 
                Keys negativeKey;
                eMouseButton mouseButton;
                Buttons positiveButton;
                Buttons negativeButton;
                int playerIndex;
                ePadAxis gamePadAxis;

                float deadZone;
                bool invert;

                Enum.TryParse<Keys>(xmlNode.SelectSingleNode("PositiveKey").InnerText, out positiveKey);
                Enum.TryParse<Keys>(xmlNode.SelectSingleNode("NegativeKey").InnerText, out negativeKey);
                Enum.TryParse<eMouseButton>(xmlNode.SelectSingleNode("MouseButton").InnerText, out mouseButton);
                Enum.TryParse<Buttons>(xmlNode.SelectSingleNode("PositiveGamePadButton").InnerText, out positiveButton);
                Enum.TryParse<Buttons>(xmlNode.SelectSingleNode("NegativeGamePadButton").InnerText, out negativeButton);
                int.TryParse(xmlNode.SelectSingleNode("GamepadPlayerIndex").InnerText, out playerIndex);
                Enum.TryParse<ePadAxis>(xmlNode.SelectSingleNode("GamepadAxis").InnerText, out gamePadAxis);

                float.TryParse(xmlNode.SelectSingleNode("AxisDeadZone").InnerText, out deadZone);
                bool.TryParse(xmlNode.SelectSingleNode("AxisInvert").InnerText, out invert);

                inputNode.PositiveKey = positiveKey;
                inputNode.NegativeKey = negativeKey;
                inputNode.MouseButton = mouseButton;
                inputNode.PositiveButton = positiveButton;
                inputNode.NegativeButton = negativeButton;
                inputNode.GamepadPlayerIndex = playerIndex - 1;
                inputNode.GamePadAxis = gamePadAxis;

                inputNode.AxisDeadZone = deadZone;
                inputNode.AxisInvert = invert;

                InputManager.Instance.AddInput(name, inputNode);
            }
        }

        // Save current input setup to Input.xml
        public void SaveInputConfig()
        {
            // New XML Document
            XmlDocument document = new XmlDocument();

            // Root Node
            XmlNode rootNode = document.CreateElement("Inputs");
            document.AppendChild(rootNode);

            // Number of Players Node
            XmlNode numberOfPlayers = document.CreateElement("NumberOfPlayers");
            numberOfPlayers.InnerText = InputManager.NumberOfPlayers.ToString();
            rootNode.AppendChild(numberOfPlayers);

            // Loop through input and create an XML Node for each
            foreach (KeyValuePair<string, InputNode> input in InputManager.Instance.InputNodes)
            {
                XmlNode inputNode = document.CreateElement("Input");
                XmlAttribute nameAttribute = document.CreateAttribute("Name");
                nameAttribute.Value = input.Key;
                rootNode.AppendChild(inputNode);

                XmlNode positiveKey = document.CreateElement("PositiveKey");
                positiveKey.InnerText = input.Value.PositiveKey.ToString();
                inputNode.AppendChild(positiveKey);

                XmlNode negativeKey = document.CreateElement("NegativeKey");
                negativeKey.InnerText = input.Value.NegativeKey.ToString();
                inputNode.AppendChild(negativeKey);

                XmlNode mouseButton = document.CreateElement("MouseButton");
                mouseButton.InnerText = input.Value.MouseButton.ToString();
                inputNode.AppendChild(mouseButton);

                XmlNode positiveButton = document.CreateElement("PositiveGamePadButton");
                positiveButton.InnerText = input.Value.PositiveButton.ToString();
                inputNode.AppendChild(positiveButton);

                XmlNode negativeButton = document.CreateElement("NegativeGamePadButton");
                negativeButton.InnerText = input.Value.NegativeButton.ToString();
                inputNode.AppendChild(negativeButton);

                XmlNode playerIndex = document.CreateElement("GamePadPlayerIndex");
                playerIndex.InnerText = (input.Value.GamepadPlayerIndex + 1).ToString();
                inputNode.AppendChild(playerIndex);

                XmlNode gamePadAxis = document.CreateElement("GamePadAxis");
                gamePadAxis.InnerText = input.Value.GamePadAxis.ToString();
                inputNode.AppendChild(gamePadAxis);

                XmlNode axisDeadZone = document.CreateElement("AxisDeadZone");
                axisDeadZone.InnerText = input.Value.AxisDeadZone.ToString();
                inputNode.AppendChild(axisDeadZone);

                XmlNode axisInvert = document.CreateElement("AxisInvert");
                axisInvert.InnerText = input.Value.AxisInvert.ToString();
                inputNode.AppendChild(axisInvert);
            }

            // Save the final document
            document.Save(S_INPUT_FILEPATH);
        }
    }
}
