using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework
{
    class AnimationPlayer
    {
        // Current animation
        Sprite spriteAnimation;

        #region Public Attributes

        // Current Animation
        public Sprite CurrentAnimationSprite
        {
            get { return spriteAnimation; }
        }

        // Animation Current Frame
        public int CurrentFrame
        {
            get 
            {
                // If the sprite is an animation then return the current frame
                if (spriteAnimation.GetType() == typeof(Animation))
                {
                    return (spriteAnimation as Animation).CurrentFrame;
                }

                // Otherwise return 0
                return 0;
            }
            set 
            {
                if (spriteAnimation.GetType() == typeof(Animation))
                {
                    (spriteAnimation as Animation).CurrentFrame = value;
                }
            }
        }

        #endregion

        // Sets a new current animation if it is not already set and plays it
        public void SetAnimation(Sprite newAnimation)
        {
            // If the animation is already set then return
            if (newAnimation == spriteAnimation)
            {
                return;
            }

            // If there is already an animation set
            if (spriteAnimation != null)
            {
                // If the animation is an animation type then stop it.
                if (spriteAnimation.GetType() == typeof(Animation))
                {
                    (spriteAnimation as Animation).Stop();
                }
            }

            // Set the new sprite animation
            spriteAnimation = newAnimation;

            // If the sprite is an animation then play it
            if (spriteAnimation.GetType() == typeof(Animation))
            {
                (spriteAnimation as Animation).Play();
            }
        }

        // Update
        public void Update(Vector2 position, float rotation)
        {
            // If there is a sprite then update it and its position
            if (spriteAnimation != null)
            {
                spriteAnimation.Update();
                spriteAnimation.Position = position;
                spriteAnimation.Rotation = rotation;
            }
        }

        // Draw
        public void Draw()
        {
            // If there isn't a sprite set then throw an exception
            if (spriteAnimation == null)
            {
                throw new NotSupportedException("No Animation currently set. Please set an animation.");
            }

            // Draw the sprite
            spriteAnimation.Draw();
        }
    }
}
