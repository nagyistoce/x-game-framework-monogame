#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GameFramework
{
    public class Game1 : Game
    {
        public static ContentManager ContentManager;
        public static SpriteBatch SpriteBatch;
        public static float DeltaTime;

        GraphicsDeviceManager graphics;

        TestGameObject gameObject;

        Music testMusic1;
        Music testMusic2;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            ContentManager = Content;
        }

        protected override void Initialize()
        {
            AudioXMLMapper.Instance.LoadAudioConfig();
            InputXMLMapper.Instance.LoadInputConfig();

            gameObject = new TestGameObject();

            GameObjectManager.Instance.AddObject(gameObject);

            GameObjectManager.Instance.AddObject(new TestGameObject());
            GameObjectManager.Instance.GetObject(1).Position = new Vector2(80, 0);

            testMusic1 = new Music("01 Main Theme");
            testMusic2 = new Music("32 Fighting with All of Our Might");

            MusicPlayer.Instance.AddMusic(testMusic1, "Track 1");
            MusicPlayer.Instance.AddMusic(testMusic2, "Track 2");

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GameObjectManager.Instance.Load();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.InputButtonPressed("Exit"))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            InputManager.Instance.Update();
            GameObjectManager.Instance.Update();

            if (InputManager.Instance.InputButtonPressed("Music1"))
            {
                MusicPlayer.Instance.Play("Track 1");
            }

            if (InputManager.Instance.InputButtonPressed("Music2"))
            {
                MusicPlayer.Instance.Play("Track 2");
            }

            if (InputManager.Instance.InputButtonPressed("Stop"))
            {
                MusicPlayer.Instance.Stop();
            }

            AudioManager.Instance.MasterVolume += InputManager.Instance.GetAxis("Volume") * 0.5f * Game1.DeltaTime;

            if (InputManager.Instance.InputButtonPressed("Mute"))
            {
                AudioManager.Instance.Mute = !AudioManager.Instance.Mute;
            }

            if (InputManager.Instance.InputButtonPressed("AudioSave"))
            {
                AudioXMLMapper.Instance.SaveAudioConfig();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            GameObjectManager.Instance.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
