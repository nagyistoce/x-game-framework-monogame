using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameFramework
{
    class TestGameObject : GameObject
    {
        Sprite standingSprite;
        Animation runningSprite;
        Animation waitingSprite;

        Sound sound;

        public TestGameObject() : base()
        {
            standingSprite = new Sprite("Megaman Standing", new Vector2(80, 0));
            runningSprite = new Animation("Megaman Running", 10, 88, 80, 0.075f, true);
            waitingSprite = new Animation("Megaman Waiting", 5, 68, 80, 0.2f, true);

            animationStateManager.AddAnimationState("Standing", standingSprite);
            animationStateManager.AddAnimationState("Running", runningSprite);
            animationStateManager.AddAnimationState("Waiting", waitingSprite);

            animationStateManager.SetAnimationState("Standing");

            sound = new Sound("Desert Eagle Shot");
        }

        public override void Load()
        {
        }

        public override void Update()
        {
            if (InputManager.Instance.InputButtonPressed("Animation1"))
            {
                animationStateManager.SetAnimationState("Standing");
            }
            
            if (InputManager.Instance.InputButtonReleased("Animation2"))
            {
                animationStateManager.SetAnimationState("Running");
            }
            
            if (InputManager.Instance.InputButtonSinglePress("Animation3"))
            {
                animationStateManager.SetAnimationState("Standing");
            }

            if (InputManager.Instance.InputButtonHeld("Animation3", 2))
            {
                animationStateManager.SetAnimationState("Waiting");
            }

            if (InputManager.Instance.GetAxis("Horizontal") > 0)
            {
                animationStateManager.AnimationPlayer.CurrentAnimationSprite.FlipEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            }
            else if (InputManager.Instance.GetAxis("Horizontal") < 0)
            {
                animationStateManager.AnimationPlayer.CurrentAnimationSprite.FlipEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            if (InputManager.Instance.InputButtonPressed("Sound"))
            {
                sound.Play();
            }

            base.Update();
        }
    }
}
