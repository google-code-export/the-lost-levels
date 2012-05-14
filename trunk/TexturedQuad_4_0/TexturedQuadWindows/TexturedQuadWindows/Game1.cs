#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using RPGLibrary;

namespace TexturedQuadWindows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        int screenHeight = 600;
        int screenWidth = 800;


        Texture2D texture; //The texture that has ground plan
        List<BasicEffect> Effects; //The list of basic effects that shades objects in the game
        QuadPlane plane; //quadplane object to draw
        
        Camera gameCamera;


        public enum GameEffect
        {GROUND_PLANE};

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferMultiSampling = true;
            this.IsMouseVisible = true;

            gameCamera = new Camera();

        }

        protected override void Initialize()
        {
            Effects = new List<BasicEffect>();
            base.Initialize();
        }
       
        
       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("grass_plain");

            Effects.Add(new BasicEffect(graphics.GraphicsDevice));


            //Setting the world settings
            Effects[(int)GameEffect.GROUND_PLANE].World = Matrix.Identity;
            Effects[(int)GameEffect.GROUND_PLANE].Projection = gameCamera.Projection;
            Effects[(int)GameEffect.GROUND_PLANE].TextureEnabled = true;
            Effects[(int)GameEffect.GROUND_PLANE].Texture = texture;

            plane = new QuadPlane(this, 1, 1); 
         }

        
        protected override void Update(GameTime gameTime)
        {
            MouseState st = Mouse.GetState();         
            #if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (st.LeftButton == ButtonState.Pressed)
            {
                Vector3 pt1 = new Vector3(st.X,st.Y,1);
                Vector3 pt2 = new Vector3(st.X,st.Y,500);
                Vector3 minPointSource = GraphicsDevice.Viewport.Unproject(pt1
                    , gameCamera.Projection
                    , gameCamera.ViewMatrix, Matrix.Identity);
                Vector3 maxPointsource = GraphicsDevice.Viewport.Unproject(pt2
                    , gameCamera.Projection, gameCamera.ViewMatrix, Matrix.Identity);
                Vector3 dir = maxPointsource - minPointSource;
                dir.Normalize();
           }
                
            #endif

            Effects[(int)GameEffect.GROUND_PLANE].World = gameCamera.ViewMatrix;

            
            gameCamera.Update();
            plane.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            plane.Draw(gameTime, Effects[0],graphics);
                  

               
            base.Draw(gameTime);
        }
    }
}
