using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameFramework
{
    class GameObject
    {
        // Animation State Manager
        protected AnimationStateManager animationStateManager;

        // Position
        protected Vector2 v2Position;
        // Rotation
        protected float fRotation;

        // Active flag
        protected bool bActive;
        // Visible flag
        protected bool bVisible;

        // To Be Deleted flag
        protected bool bDeleted;

        #region Public Attributes

        // Position
        public Vector2 Position
        {
            get { return v2Position; }
            set { v2Position = value; }
        }

        // Rotation
        public float Rotation
        {
            get { return fRotation; }
            set { fRotation = value; }
        }

        // Active flag
        public bool Active
        {
            get { return bActive; }
            set { bActive = value; }
        }

        // Visible flag
        public bool Visible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        // To Be Deleted flag
        public bool ToBeDeleted
        {
            get { return bDeleted; }
            set { bDeleted = value; }
        }

        #endregion

        public GameObject()
        {
            animationStateManager = new AnimationStateManager();

            v2Position = Vector2.Zero;
            fRotation = 0;

            bActive = true;
            bVisible = true;

            bDeleted = false;
        }

        // Load
        public virtual void Load()
        {
            
        }

        // Update
        public virtual void Update()
        {
            animationStateManager.Update(v2Position, fRotation);
        }

        // Draw
        public virtual void Draw()
        {
            // Set sprite visibility
            animationStateManager.AnimationPlayer.CurrentAnimationSprite.Active = bVisible;

            animationStateManager.Draw();
        }
    }
}
