using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace DynamicGrid
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DynamicGrid Grid;
        int GridSpacing = 6;
        float MouseStrength = 1.3f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)Values.WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)Values.WindowSize.Y;
            Grid = new DynamicGrid(new Rectangle(50, 50, (int)Values.WindowSize.X - 100, (int)Values.WindowSize.Y - 100), GridSpacing);
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Control.Update();
            FPSCounter.Update(gameTime);

            Grid.Update();

            if (Control.CurMS.LeftButton == ButtonState.Pressed)
            {
                Task.Factory.StartNew(() => Grid.ApplyForce(Control.GetMouseVector(), MouseStrength));
                //Grid.ApplyForce(Control.GetMouseVector(), MouseStrength);
            }
            if (Control.CurMS.RightButton == ButtonState.Pressed)
            {
                Task.Factory.StartNew(() => Grid.ApplyForce(Control.GetMouseVector(), -MouseStrength));
                //Grid.ApplyForce(Control.GetMouseVector(), -MouseStrength);
            }
            if (Control.CurMS.MiddleButton == ButtonState.Pressed)
            {
                Task.Factory.StartNew(() => Grid.Twist(Control.GetMouseVector(), -MouseStrength * 3, (float)Math.PI / 3f));
                //Grid.Twist(Control.GetMouseVector(), -MouseStrength * 3, (float)Math.PI / 3f);
            }

            if (Control.WasKeyJustPressed(Keys.D))
            {
                Grid = new DynamicGrid(new Rectangle(100, 100, (int)Values.WindowSize.X - 200, (int)Values.WindowSize.Y - 200), GridSpacing);
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            Grid.Draw(spriteBatch);

            FPSCounter.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
