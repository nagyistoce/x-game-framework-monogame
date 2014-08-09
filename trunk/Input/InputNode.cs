using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameFramework
{
    // Gamepad Axes
    public enum ePadAxis
    {
        None, 
        LeftStickX, 
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftTrigger,
        RightTrigger
    }

    // Mouse Buttons
    public enum eMouseButton
    {
        None,
        LeftButton,
        MiddleButton,
        RightButton
    }

    public class InputNode
    {
        // Positive Key
        Keys posKey;
        // Negative Key
        Keys negKey;

        // Mouse Button
        eMouseButton mouseButton;

        // Positive Gamepad Button
        Buttons posGamePadButton;
        // Negative Gamepad Button
        Buttons negGamePadButton;

        // Gamepad Player Index
        int iPlayerIndex;

        // Gamepad Axis
        ePadAxis gamePadAxis;

        // Gamepad Axis Dead Zone
        float fDeadZone;
        // Gamepad Axis Invert flag
        bool bInvert;

        // Time this input has been held for.
        float fHoldDuration;
        // Has this input been held for the required amount of time (Time determined by InputManager.InputButtonHeld())
        bool bHoldTimeReached;

        // Time this input has been held for a tap.
        float fTapHoldDuration;

        #region Public Attributes

        // Positive Key
        public Keys PositiveKey
        {
            get { return posKey; }
            set { posKey = value; }
        }

        // Negative Key
        public Keys NegativeKey
        {
            get { return negKey; }
            set { negKey = value; }
        }

        // Mouse Button
        public eMouseButton MouseButton
        {
            get { return mouseButton; }
            set { mouseButton = value; }
        }

        // Gamepad Positive Button
        public Buttons PositiveButton
        {
            get { return posGamePadButton; }
            set { posGamePadButton = value; }
        }

        // Negative Mouse Button
        public Buttons NegativeButton
        {
            get { return negGamePadButton; }
            set { negGamePadButton = value; }
        }

        // Gamepad Player Index
        public int GamepadPlayerIndex
        {
            get { return iPlayerIndex; }
            set { iPlayerIndex = MathHelper.Clamp(value, 0, 3); }
        }

        // Gamepad Axis
        public ePadAxis GamePadAxis
        {
            get { return gamePadAxis; }
            set { gamePadAxis = value; }
        }

        // Gamepad Axis Dead Zone
        public float AxisDeadZone
        {
            get { return fDeadZone; }
            set { fDeadZone = MathHelper.Clamp(value, -1, 1); }
        }

        // Gamepad Axis Invert flag
        public bool AxisInvert
        {
            get { return bInvert; }
            set { bInvert = value; }
        }

        // Hold Duration
        public float HoldDuration
        {
            get { return fHoldDuration; }
            set { fHoldDuration = value; }
        }

        // Hold Time Reached flag
        public bool HoldTimeReached
        {
            get { return bHoldTimeReached; }
            set { bHoldTimeReached = value; }
        }

        // Tap Hold Duration
        public float TapHoldDuration
        {
            get { return fTapHoldDuration; }
            set { fTapHoldDuration = Math.Max(0, value); }
        }

        #endregion

        // Resets the Hold Duration and Hold Time Reached variables
        public void ResetHoldTime()
        {
            fHoldDuration = 0;
            bHoldTimeReached = false;
        }
    }
}
