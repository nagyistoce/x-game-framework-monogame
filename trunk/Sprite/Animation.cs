using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework
{
    // Animated Sprite - Inherits from Sprite
    class Animation : Sprite
    {
        // Frame Position and Size on the Sprite Sheet
        Rectangle rFrameRect;
        
        // Current Frame Index
        int iFrameCount;
        // Number of frames in the animation
        int iNoOfFrames;

        // Length of time to complete a frame
        float fFrameTime;
        // Timer used to count a frame time
        float fFrameTimer;

        // Looping Animation flag
        bool bLooping;
        // Is Animating flag
        bool bAnimating;

        #region Public Attributes

        // Frame Width
        public int FrameWidth
        {
            get { return rFrameRect.Width; }
        }

        // Frame Height
        public int FrameHeight
        {
            get { return rFrameRect.Height; }
        }

        // Current Frame Index
        public int CurrentFrame
        {
            get { return iFrameCount; }
            set { iFrameCount = MathHelper.Clamp(value, 0, iNoOfFrames); }
        }

        // Number of Frames
        public int NumberOfFrames
        {
            get { return iNoOfFrames; }
        }

        // Frame Time
        public float FrameTime
        {
            get { return fFrameTime; }
            set { fFrameTime = Math.Max(0, value); }
        }

        // Looping
        public bool Looping
        {
            get { return bLooping; }
            set { bLooping = value; }
        }

        // Animating
        public bool Animating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }

        #endregion

        #region Constructors

        public Animation(string path, int noOfFrames, int frameWidth, int frameHeight, float frameTime, 
            bool looping) : base (path)
        {
            rFrameRect = new Rectangle(0, 0, frameWidth, frameHeight);

            iFrameCount = 0;
            iNoOfFrames = noOfFrames;

            fFrameTime = frameTime;
            fFrameTimer = 0f;

            bLooping = looping;
            bAnimating = false;
            bActive = true;
        }

        public Animation(string path, int noOfFrames, int frameWidth, int frameHeight, float frameTime,
            bool looping, Vector2 position) : base(path, position)
        {
            rFrameRect = new Rectangle(0, 0, frameWidth, frameHeight);

            iFrameCount = 0;
            iNoOfFrames = noOfFrames;

            fFrameTime = frameTime;
            fFrameTimer = 0f;

            bLooping = looping;
            bAnimating = false;
            bActive = true;
        }

        public Animation(string path, Vector2 position, Vector2 origin, float rotation, int noOfFrames, 
            bool looping, int frameWidth, int frameHeight, float frameTime) : base(path, position, origin, rotation)
        {
            rFrameRect = new Rectangle(0, 0, frameWidth, frameHeight);

            iFrameCount = 0;
            iNoOfFrames = noOfFrames;

            fFrameTime = frameTime;
            fFrameTimer = 0f;

            bLooping = looping;
            bAnimating = false;
            bActive = true;
        }

        #endregion

        // Plays the animation
        public void Play()
        {
            bAnimating = true;
        }

        // Stops the animation
        public void Stop()
        {
            iFrameCount = 0;
            fFrameTimer = 0f;

            bAnimating = false;
        }

        // Pause the animation
        public void Pause()
        {
            bAnimating = false;
        }

        // Update
        public override void Update()
        {
            // If active
            if (bActive)
            {
                // If animating
                if (bAnimating)
                {
                    // Elapse frame timer
                    fFrameTimer += Game1.DeltaTime;

                    // If the frame timer has reached the frame time then
                    // increment the frame count and reset the timer
                    if (fFrameTimer > fFrameTime)
                    {
                        iFrameCount++;
                        fFrameTimer = 0f;
                    }

                    // If the end of the animation is reached
                    if (iFrameCount >= iNoOfFrames)
                    {
                        // If the animation loops then reset the frame count
                        if (bLooping)
                        {
                            iFrameCount = 0;
                        }
                        else // Freeze the animation
                        {
                            iFrameCount = iNoOfFrames - 1;
                            bAnimating = false;
                        }
                    }
                }
            }
        }

        // Draw
        public override void Draw()
        {
            // Set the frame rect position
            rFrameRect.X = iFrameCount * rFrameRect.Width;

            // If active then draw
            if (bActive)
            {
                Game1.SpriteBatch.Draw(t2Texture, v2Position, rFrameRect, cTint, fRotation, v2Origin, fScale, flipEffect, fDepth);
            }
        }
    }
}
