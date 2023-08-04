using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace BalloonInvasion
{
    public class GameInstance : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public int WindowWidth = 960;
        public int WindowHeight = 540;

        public int SceneWidth = 960;
        public int SceneHeight = 540;

        public int VirtualWidth = 960;
        public int VirtualHeight = 540;

        public GameTime LastRenderGameTime = new GameTime();
        public bool GameStarted = false;
        public void ApplyReolustion(int W, int H, bool Apply = true)
        {
            WindowWidth = W;
            WindowHeight = H;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.PreferredBackBufferWidth = WindowWidth;
            if (Apply)
            {
                _graphics.ApplyChanges();
            }
        }

        public GameInstance()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.ApplyChanges();
            GameManager.Game = this;
        }
        protected override void Initialize()
        {
            GameManager.PrepareLevels();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager.SpriteBatch = _spriteBatch;
            GameManager.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (!GameStarted)
            {
                GameManager.Start();
                GameStarted = true;
            }
            GameManager.Update(gameTime);
            IsMouseVisible = false;

            if (IsActive)
            {
                Input.Update();
            }
            GameObjectManager.Update(gameTime);
            base.Update(gameTime);
        }

        public void ForceDraw()
        {
            Draw(LastRenderGameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            LastRenderGameTime = gameTime;
            GameManager.Draw(gameTime);
            GraphicsDevice.Clear(Color.Orange);
            LayersManager.Render(gameTime);
            foreach (SpriteFont Font in SpriteFont.SpriteFonts)
            {
                Font.Render(_spriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}