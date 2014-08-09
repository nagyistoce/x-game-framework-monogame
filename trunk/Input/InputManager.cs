using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameFramework
{
    class InputManager
    {
        // Singleton Instance
        static InputManager instance;

        // Input Dictionary
        Dictionary<string, InputNode> dInputs;

        // Current Keyboard State
        KeyboardState keyState;
        // Previous Frame Keyboard State
        KeyboardState prevKeyState;

        // Current Mouse State
        MouseState mouseState;
        // Previous Frame Mouse State
        MouseState prevMouseState;

        // Number of Player Controllers
        static int iNumberOfPlayers = 1;

        // Current Gamepad State Array (Can hold up to 4 players)
        GamePadState[] aGamepadStates;
        // Previous Frame Gamepad State (Can hold up to 4 players)
        GamePadState[] aPrevGamepadStates;

        // Length of time a input can be held and released to be considered a single press
        const float BUTTON_TAP_DURATION = 0.2f;

        public InputManager()
        {
            dInputs = new Dictionary<string, InputNode>();

            // Setup states for the number of players set
            aGamepadStates = new GamePadState[iNumberOfPlayers];
            aPrevGamepadStates = new GamePadState[iNumberOfPlayers];

            // Initialize Input States
            UpdateInputStates();
        }

        #region Public Attributes

        // Instance 
        public static InputManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }

                return instance;
            }
        }

        // Number of players
        public static int NumberOfPlayers
        {
            get { return iNumberOfPlayers; }
            set { iNumberOfPlayers = MathHelper.Clamp(value, 1, 4); }
        }

        // Input Dictionary
        public Dictionary<string, InputNode> InputNodes
        {
            get { return dInputs; }
        }

        #endregion

        // Adds a new input into the dictionary if it doesn't already exist
        public void AddInput(string name, InputNode input)
        {
            if (dInputs.Count > 0)
            {
                if (dInputs.ContainsKey(name))
                {
                    return;
                }
            }

            dInputs.Add(name, input);
        }

        // Checks if an input exists in the dictionary and returns the result
        private bool CheckInputExists(string name)
        {
            if (dInputs.Count > 0)
            {
                if (dInputs.ContainsKey(name))
                {
                    return true;
                }
            }

            return false;
        }

        // Update
        public void Update()
        {
            UpdateInputStates();
        }

        // Updates all input states
        private void UpdateInputStates()
        {
            // Previous Frame
            prevKeyState = keyState;
            prevMouseState = mouseState;

            for (int i = 0; i < iNumberOfPlayers; i++)
            {
                aPrevGamepadStates[i] = aGamepadStates[i];
            }

            // Current
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            for (int i = 0; i < iNumberOfPlayers; i++)
            {
                aGamepadStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        #region Button Inputs

        // Main Inputs 
        #region Public Inputs

        // Button Down (Held on this frame)
        public bool InputButtonDown(string inputName)
        {
            if (!CheckInputExists(inputName))
            {
                return false;
            }

            // If any of the buttons for this input are down then return true
            if (KeyDown(inputName) || MouseButtonDown(mouseState, inputName) || GamepadButtonDown(inputName))
            {
                return true;
            }

            return false;
        }

        // Button Up (Not down on this frame)
        public bool InputButtonUp(string inputName)
        {
            if (!CheckInputExists(inputName))
            {
                return false;
            }

            // If any of the buttons for this input are up then return true
            if (KeyUp(inputName) && MouseButtonUp(mouseState, inputName) && GamepadButtonUp(inputName))
            {
                return true;
            }

            return false;
        }

        // Button Pressed (Pressed this frame and not in the previous frame)
        public bool InputButtonPressed(string inputName)
        {
            if (!CheckInputExists(inputName))
            {
                return false;
            }

            // If any of the buttons for this input have been pressed then return true
            if (KeyPressed(inputName) || MouseButtonPressed(inputName) || GamepadButtonPressed(inputName))
            {
                return true;
            }

            return false;
        }

        // Button Released (Up on this frame and not in the previous frame)
        public bool InputButtonReleased(string inputName)
        {
            if (!CheckInputExists(inputName))
            {
                return false;
            }

            // If any of the buttons for this input are down then return true
            if (KeyReleased(inputName) || MouseButtonReleased(inputName) || GamepadButtonReleased(inputName))
            {
                return true;
            }

            return false;
        }

        // Button quickly pressed and released (Button Tap) (Window of 0.2 seconds) 
        public bool InputButtonSinglePress(string inputName)
        {
            if (!dInputs.ContainsKey(inputName))
            {
                return false;
            }

            // Increment the Input's TapHoldDuration if the input is down
            if (InputButtonDown(inputName))
            {
                dInputs[inputName].TapHoldDuration += Game1.DeltaTime;
            }

            // If the TapHoldDuration is between 0 and the BUTTON_TAP_DURATION 
            // and the input is released then return true
            if (dInputs[inputName].TapHoldDuration > 0 && 
                dInputs[inputName].TapHoldDuration < BUTTON_TAP_DURATION)
            {
                if (InputButtonReleased(inputName))
                {
                    return true;
                }
            }

            // If the input is up then set TapHoldDuration to 0
            if (InputButtonUp(inputName))
            {
                dInputs[inputName].TapHoldDuration = 0;
            }

            return false;
        }

        // Button Held For a Set Period of Time (Set to true once hold time is reached)
        public bool InputButtonHeld(string inputName, float holdTime)
        {
            if (!dInputs.ContainsKey(inputName))
            {
                return false;
            }

            if (InputButtonDown(inputName))
            {
                // If the hold time has not been reached
                if (!dInputs[inputName].HoldTimeReached)
                {
                    // Increment hold time
                    dInputs[inputName].HoldDuration += Game1.DeltaTime;

                    // If the hold time has been reached then set HoldTimeReached to true
                    if (dInputs[inputName].HoldDuration >= holdTime)
                    {
                        dInputs[inputName].HoldTimeReached = true;
                    }
                }
            }
            else
            {
                // Reset the Hold Time
                dInputs[inputName].ResetHoldTime();
            }

            return dInputs[inputName].HoldTimeReached;
        }

        #endregion

        #region Key Inputs

        // Key Down
        private bool KeyDown(string inputName)
        {
            // If there is no key, then return false
            if (dInputs[inputName].PositiveKey == Keys.None)
            {
                return false;
            }

            // If the positive key is down then return true
            if (keyState.IsKeyDown(dInputs[inputName].PositiveKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Key Up
        private bool KeyUp(string inputName)
        {
            // If there is no key, then return false
            if (dInputs[inputName].PositiveKey == Keys.None)
            {
                return true;
            }

            // If the positive key is up then return true
            if (keyState.IsKeyUp(dInputs[inputName].PositiveKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Key Pressed
        private bool KeyPressed(string inputName)
        {
            // If there is no key, then return false
            if (dInputs[inputName].PositiveKey == Keys.None)
            {
                return false;
            }

            // If the positive key is pressed then return true
            if (keyState.IsKeyDown(dInputs[inputName].PositiveKey) &&
                prevKeyState.IsKeyUp(dInputs[inputName].PositiveKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Key Released
        private bool KeyReleased(string inputName)
        {
            // If there is no key, then return false
            if (dInputs[inputName].PositiveKey == Keys.None)
            {
                return false;
            }

            // If the positive key is released then return true
            if (keyState.IsKeyUp(dInputs[inputName].PositiveKey) &&
                prevKeyState.IsKeyDown(dInputs[inputName].PositiveKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Mouse Inputs

        // Mouse Button Down
        private bool MouseButtonDown(MouseState state, string inputName)
        {
            // If there is no mouse button then return false
            if (dInputs[inputName].MouseButton == eMouseButton.None)
            {
                return false;
            }

            // Check the eMouseButton input and return true if it is down
            switch (dInputs[inputName].MouseButton)
            {
                case eMouseButton.LeftButton:
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                break;

                case eMouseButton.MiddleButton:
                    if (state.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                break;

                case eMouseButton.RightButton:
                    if (state.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                break;
            }

            return false;
        }

        // Mouse Button Up
        private bool MouseButtonUp(MouseState state, string inputName)
        {
            // If there is no mouse button then return false
            if (dInputs[inputName].MouseButton == eMouseButton.None)
            {
                return true;
            }

            // Check the eMouseButton input and return true if it is up
            switch (dInputs[inputName].MouseButton)
            {
                case eMouseButton.LeftButton:
                    if (state.LeftButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;

                case eMouseButton.MiddleButton:
                    if (state.MiddleButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;

                case eMouseButton.RightButton:
                    if (state.RightButton == ButtonState.Released)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        // Mouse Button Pressed
        public bool MouseButtonPressed(string inputName)
        {
            // Check if the current mouse button is down and the previous frame button is up and return true
            if (MouseButtonDown(mouseState, inputName) && MouseButtonUp(prevMouseState, inputName))
            {
                return true;
            }

            return false;
        }

        // Mouse Button Released
        public bool MouseButtonReleased(string inputName)
        {
            // Check if the current mouse button is up and the previous frame button is down and return true
            if (MouseButtonUp(mouseState, inputName) && MouseButtonDown(prevMouseState, inputName))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Gamepad Inputs

        // Gamepad Button Down
        private bool GamepadButtonDown(string inputName)
        {
            // If there is no gamepad button then return false
            if (dInputs[inputName].PositiveButton == 0)
            {
                return false;
            }

            // If the gamepad button is down then return true
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonDown(dInputs[inputName].PositiveButton))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Gamepad Button Up
        private bool GamepadButtonUp(string inputName)
        {
            // If there is no gamepad button then return false
            if (dInputs[inputName].PositiveButton == 0)
            {
                return true;
            }

            // If the gamepad button is up then return true
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonUp(dInputs[inputName].PositiveButton))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Gamepad Button Pressed
        private bool GamepadButtonPressed(string inputName)
        {
            // If there is no gamepad button then return false
            if (dInputs[inputName].PositiveButton == 0)
            {
                return false;
            }

            // If the gamepad button is pressed then return true
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonDown(dInputs[inputName].PositiveButton) &&
                aPrevGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonUp(dInputs[inputName].PositiveButton))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Gamepad Button Released
        private bool GamepadButtonReleased(string inputName)
        {
            // If there is no gamepad button then return false
            if (dInputs[inputName].PositiveButton == 0)
            {
                return false;
            }

            // If the gamepad button is released then return true
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonUp(dInputs[inputName].PositiveButton) &&
                aPrevGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonDown(dInputs[inputName].PositiveButton))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Axis Inputs

        // Returns the current position on the input axis
        public float GetAxis(string inputName)
        {
            if (!CheckInputExists(inputName))
            {
                return 0;
            }

            // Key Axis
            float key = KeyAxis(inputName);
            // Button Axis
            float button = GamepadButtonAxis(inputName);
            // Analog Axis - (Thumbsticks, Triggers, Accelerometers)
            float analog = GamepadAnalogAxis(inputName);

            // If any of the inputs are at 0, then set to -2 
            // to avoid conflict in calculating the currently used axis
            if (key == 0)
            {
                key = -2;
            }

            if (button == 0)
            {
                button = -2;
            }

            if (analog == 0)
            {
                analog = -2;
            }

            // Find the max axis value
            float maxAxis = Math.Max(key, Math.Max(button, analog));

            // If the max axis value is -2 then set to back to 0
            if (maxAxis == -2)
            {
                maxAxis = 0;
            }

            // If the axis is inverted then multiply by -1
            if (dInputs[inputName].AxisInvert)
            {
                maxAxis *= -1;
            }

            return maxAxis;
        }

        // Key Axis
        private float KeyAxis(string inputName)
        {
            // Output Position
            float outputPosition = 0;

            // If the positive key is down then add 1
            if (keyState.IsKeyDown(dInputs[inputName].PositiveKey))
            {
                outputPosition += 1;
            }

            // If the negative key is down then subtract 1
            if (keyState.IsKeyDown(dInputs[inputName].NegativeKey))
            {
                outputPosition -= 1;
            }

            return outputPosition;
        }

        // Gamepad Button Axis
        private float GamepadButtonAxis(string inputName)
        {
            // Output Position
            float outputPosition = 0;

            // If the positive button is down then add 1
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonDown(dInputs[inputName].PositiveButton))
            {
                outputPosition += 1;
            }

            // If the negative button is down then subtract 1
            if (aGamepadStates[dInputs[inputName].GamepadPlayerIndex].IsButtonDown(dInputs[inputName].NegativeButton))
            {
                outputPosition -= 1;
            }

            return outputPosition;
        }

        // Gamepad Analog Axis
        public float GamepadAnalogAxis(string inputName)
        {
            // Output Position
            float outputPosition = 0;

            // Check the ePadAxis input and set the Output Position to that axis
            switch (dInputs[inputName].GamePadAxis)
            {
                case ePadAxis.LeftStickX:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].ThumbSticks.Left.X;
                break;

                case ePadAxis.LeftStickY:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].ThumbSticks.Left.Y;
                break;

                case ePadAxis.RightStickX:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].ThumbSticks.Right.X;
                break;

                case ePadAxis.RightStickY:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].ThumbSticks.Right.Y;
                break;

                case ePadAxis.LeftTrigger:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].Triggers.Left;
                break;

                case ePadAxis.RightTrigger:
                    outputPosition = aGamepadStates[dInputs[inputName].GamepadPlayerIndex].Triggers.Right;
                break;
            }

            // if the absolute Output Position is less than the dead zone then set the Output Position to 0
            if (Math.Abs(outputPosition) < dInputs[inputName].AxisDeadZone)
            {
                outputPosition = 0;
            }

            return outputPosition;
        }

        #endregion
    }
}