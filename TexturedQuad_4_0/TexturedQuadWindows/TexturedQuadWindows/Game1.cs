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

        LevelEditor form;

        Texture2D texture; //The texture that has ground plan
        List<BasicEffect> Effects; //The list of basic effects that shades objects in the game
        
        
        TileMap myMap;
        Model Guy,Tree;
        CustomModel cm;
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

            myMap = new TileMap("", this);

            gameCamera = new Camera();

            form = new LevelEditor();
            form.Activate();
            
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

            

            Guy = Content.Load<Model>("guy");
            Tree = Content.Load<Model>("Tree");

            Effects.Add(new BasicEffect(graphics.GraphicsDevice));


            //Setting the world settings
            Effects[(int)GameEffect.GROUND_PLANE].World = Matrix.Identity;
            Effects[(int)GameEffect.GROUND_PLANE].Projection = gameCamera.Projection;
            Effects[(int)GameEffect.GROUND_PLANE].TextureEnabled = true;
            Effects[(int)GameEffect.GROUND_PLANE].Texture = texture;

            
         }
        Vector3 pointToput = Vector3.Zero;
        
        protected override void Update(GameTime gameTime)
        {
            MouseState st = Mouse.GetState();
            Vector3 dir;

            #if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            //finds out the intersecting point of your left click. for picking

            Vector3 pt1 = new Vector3(st.X,st.Y,1);
            Vector3 pt2 = new Vector3(st.X,st.Y,500);
            Vector3 minPointSource = GraphicsDevice.Viewport.Unproject(pt1
                , gameCamera.Projection
                , gameCamera.ViewMatrix, Matrix.Identity);
            Vector3 maxPointsource = GraphicsDevice.Viewport.Unproject(pt2
                , gameCamera.Projection, gameCamera.ViewMatrix, Matrix.Identity);
            dir = maxPointsource - minPointSource;
            dir.Normalize();

            float t = -maxPointsource.Y / dir.Y;
            pointToput = new Vector3(maxPointsource.X + t * dir.X, 0.0f, maxPointsource.Z + t * dir.Z);
           //}
            if (st.LeftButton == ButtonState.Pressed)
            {
                Point tileClicked = myMap.GetTileIndex(pointToput);

            }
            
                
            #endif

            Effects[(int)GameEffect.GROUND_PLANE].World = gameCamera.ViewMatrix;

            
            gameCamera.Update();
           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            //plane.Draw(gameTime, Effects[0],graphics);
            myMap.DrawTileMap(Effects[(int)GameEffect.GROUND_PLANE], graphics);

            //DrawModel(Guy);
            DrawTree(Tree);
            
            
               
            base.Draw(gameTime);
        }

        private void DrawTree(Model m)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection = gameCamera.Projection;
            Matrix view = gameCamera.ViewMatrix;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateScale(1, 1, 1)
                        * Matrix.CreateRotationX(-90)
                        * Matrix.CreateTranslation(new Vector3(pointToput.X,0,pointToput.Z));
                        //* Matrix.CreateScale(1, 1, 1) 
                        //* Matrix.CreateRotationX(0)
                        //* Matrix.CreateTranslation(new Vector3(pointToput.X, 2f, pointToput.Z))
                        ;
                }
                mesh.Draw();
            }
        }


        private void DrawModel(Model m)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection = gameCamera.Projection;
            Matrix view = gameCamera.ViewMatrix;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateScale(0.5f, 0.5f, 0.5f)
                        * Matrix.CreateRotationX(MathHelper.ToRadians(-90))
                        * Matrix.CreateTranslation(new Vector3(pointToput.X,2f,pointToput.Z))
                        ;
                }
                mesh.Draw();
            }
        }


    }
}
