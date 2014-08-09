using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace GameFramework
{
    class AnimationStateManager
    {
        // Animation Player
        AnimationPlayer animationPlayer;
        // Dictionary of animation states <Name of State, Sprite Animation>
        Dictionary<string, Sprite> dAnimationState;

        // Current State Name
        string sCurrentState;

        #region Public Attributes

        // Animation Player
        public AnimationPlayer AnimationPlayer
        {
            get { return animationPlayer; }
        }

        // Current State Name
        public string CurrentAnimationState
        {
            get { return sCurrentState; }
        }

        #endregion

        public AnimationStateManager()
        {
            animationPlayer = new AnimationPlayer();
            dAnimationState = new Dictionary<string, Sprite>();

            sCurrentState = "";
        }

        // Adds an animation state if it doesn't already exist
        public void AddAnimationState(string name, Sprite animation)
        {
            if (dAnimationState.ContainsKey(name))
            {
                return;
            }

            dAnimationState.Add(name, animation);
        }

        // Sets the current animation state if it exists
        public void SetAnimationState(string name)
        {
            if (dAnimationState.ContainsKey(name))
            {
                animationPlayer.SetAnimation(dAnimationState[name]);
            }
        }

        // Updates the animation player is there is a state available
        public void Update(Vector2 position, float rotation)
        {
            if (dAnimationState.Count > 0)
            {
                animationPlayer.Update(position, rotation);
            }
        }

        // Draws the animation if there is a state available
        public void Draw()
        {
            if (dAnimationState.Count > 0)
            {
                animationPlayer.Draw();
            }
        }
    }
}
