using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework
{
    // Static Image with a position, scale, origin, rotation, tint and layer depth.
    class Sprite
    {
        // File Path
        protected string sPath;

        // Texture
        protected Texture2D t2Texture;

        // Position
        protected Vector2 v2Position;
        // Origin Position 
        protected Vector2 v2Origin;

        // Source Rectangle - Used to show a portion of the sprite
        Rectangle? rSource = null;

        // Rotation
        protected float fRotation;
        // Scale
        protected float fScale;

        // Flip Effect - None, Horizontal, Vertical
        protected SpriteEffects flipEffect;

        // Tint Colour
        protected Color cTint;

        // Layer Depth
        protected float fDepth;
        
        // Active flag
        protected bool bActive;

        #region Public Attributes

        // Width
        public float Width
        {
            get { return t2Texture.Width * fScale; }
        }

        // Height
        public float Height
        {
            get { return t2Texture.Height * fScale; }
        }

        // Position
        public Vector2 Position
        {
            get { return v2Position; }
            set { v2Position = value; }
        }

        // Origin
        public Vector2 Origin
        {
            get { return v2Origin; }
            set { v2Origin = value; }
        }

        // Source Rectangle
        public Rectangle? SourceRect
        {
            get { return rSource; }
            set { value = rSource; }
        }

        // Rotation
        public float Rotation
        {
            get { return MathHelper.ToDegrees(fRotation); }
            set { fRotation = MathHelper.ToRadians(value); }
        }

        // Scale
        public float Scale
        {
            get { return fScale; }
            set { fScale = value; }
        }

        // Flip Effect
        public SpriteEffects FlipEffect
        {
            get { return flipEffect; }
            set { flipEffect = value; }
        }

        // Tint
        public Color Tint
        {
            get { return cTint; }
            set { cTint = value; }
        }

        // Layer Depth
        public float Depth
        {
            get { return fDepth; }
            set { fDepth = value; }
        }

        // Active
        public bool Active
        {
            get { return bActive; }
            set { bActive = value; }
        }

        #endregion

        #region Constructors

        // Path only
        public Sprite(string path)
        {
            sPath = path;

            v2Position = Vector2.Zero;
            v2Origin = Vector2.Zero;
            fRotation = 0f;
            fScale = 1f;
            flipEffect = SpriteEffects.None;
            cTint = Color.White;
            fDepth = 0f;
            bActive = true;

            Load();
        }

        // Path, Position 
        public Sprite(string path, Vector2 position)
        {
            sPath = path;

            v2Position = position;
            v2Origin = Vector2.Zero;
            fRotation = 0f;
            fScale = 1f;
            flipEffect = SpriteEffects.None;
            cTint = Color.White;
            fDepth = 0f;
            bActive = true;

            Load();
        }

        // Path, Position, Origin
        public Sprite(string path, Vector2 position, Vector2 origin)
        {
            sPath = path;

            v2Position = position;
            v2Origin = origin;
            fRotation = 0f;
            fScale = 1f;
            flipEffect = SpriteEffects.None;
            cTint = Color.White;
            fDepth = 0f;
            bActive = true;

            Load();
        }

        // Path, Position, Origin, Rotation
        public Sprite(string path, Vector2 position, Vector2 origin, float rotation)
        {
            sPath = path;

            v2Position = position;
            v2Origin = origin;
            fRotation = rotation;
            fScale = 1f;
            flipEffect = SpriteEffects.None;
            cTint = Color.White;
            fDepth = 0f;
            bActive = true;

            Load();
        }

        // Path, Position, Origin, Rotation, Scale
        public Sprite(string path, Vector2 position, Vector2 origin, float rotation, float scale)
        {
            sPath = path;

            v2Position = position;
            v2Origin = origin;
            fRotation = rotation;
            fScale = scale;
            flipEffect = SpriteEffects.None;
            cTint = Color.White;
            fDepth = 0f;
            bActive = true;

            Load();
        }

        // Full Initialization
        public Sprite(string path, Vector2 position, Vector2 origin, float rotation, float scale, 
            SpriteEffects effect, Color tint, float depth)
        {
            sPath = path;

            v2Position = position;
            v2Origin = origin;
            fRotation = rotation;
            fScale = scale;
            flipEffect = effect;
            cTint = tint;
            fDepth = depth;
            bActive = true;

            Load();
        }

        #endregion

        ~Sprite()
        {
            t2Texture.Dispose();
        }

        // Loads the texture
        void Load()
        {
            t2Texture = Game1.ContentManager.Load<Texture2D>(sPath);
        }

        public virtual void Update()
        {
        }

        // Draws the sprite to the screen
        public virtual void Draw()
        {
            if (bActive)
            {
                Game1.SpriteBatch.Draw(t2Texture, v2Position, rSource, cTint, fRotation, v2Origin, fScale, flipEffect, fDepth);
            }
        }
    }
}