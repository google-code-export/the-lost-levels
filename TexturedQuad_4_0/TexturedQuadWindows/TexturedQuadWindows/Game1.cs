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

        BasicEffect Camera;
        GroundPlane g;


        Camera gameCamera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            this.IsMouseVisible = true;
            gameCamera = new Camera();

        }

        Quad[,] quad;
        VertexDeclaration vertexDeclaration;
        Matrix View, Projection;
        protected override void Initialize()
        {

            quad = new Quad[100,100];

            
            Vector3 xoffset = new Vector3(1,0,0);
            Vector3 zoffset = new Vector3(0, 0, 1);

            for(int j=0;j<100;j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    quad[i,j] = new Quad(Vector3.Zero + i * xoffset + j*zoffset, new Vector3(0, 1, 0), new Vector3(0, 0, -1), 1, 1);
                }
            }

            //View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero,
             //   Vector3.Up);
           
           // View = Matrix.CreateLookAt(new Vector3(0, 2, 0), Vector3.Zero, new Vector3(1, 0, 0));
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,(float)screenWidth/screenHeight, 1, 500);
            
            g = new GroundPlane(50.0f, 0.5f, Color.Blue);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Texture2D texture;
        BasicEffect quadEffect;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("grass_plain");
            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            //quadEffect.EnableDefaultLighting();
            
            quadEffect.World = Matrix.Identity;
            //quadEffect.World = Matrix.CreateRotationZ((float)Math.PI / 4)*Matrix.CreateRotationX((float) -Math.PI/2);
            //quadEffect.World = Matrix.CreateRotationX((float) Math.PI/8);

            quadEffect.View = gameCamera.ViewMatrix;
            quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
         }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {




            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            #endif


            gameCamera.Update();
            quadEffect.View = gameCamera.ViewMatrix;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            

            quadEffect.World = Matrix.Identity;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {


                    foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad[i,j].Vertices, 0, 4,
                            quad[i,j].Indexes, 0, 2);

                    }
                
                }
            }

            
            
            
               
            base.Draw(gameTime);
        }
    }
}
