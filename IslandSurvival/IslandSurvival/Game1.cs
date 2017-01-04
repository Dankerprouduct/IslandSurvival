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


namespace IslandSurvival
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        World world; 
        public int SCREEN_WIDTH = 1280;
        public int SCREEN_HEIGHT;

        float fps;
        SpriteFont spriteFont;
        public Player player;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        public static Vector2 worldPos;
        Vector2 mousePos;
        MouseState mouseState;

        public static int sizeX = 500;
        public static int sizeY = 300;

        bool paused = false;


       // SurvivorManager survivors;
        Line[] lines;
        Line[] groupLines; 
        public static Vector2 GetMousePosition()
        {
            return worldPos;
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            SCREEN_HEIGHT = (SCREEN_WIDTH / 16) * 9;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            IsMouseVisible = true;


        }




        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            string seed = "iuhygfdxs";//"kljkbj";
            Console.WriteLine(seed.GetHashCode());
            world = new World();
            world.LoadContent(Content); 

            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player(new Vector2((0 * 2) / 2, (0 * 32) / 2));

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            //survivors = new SurvivorManager();
            //survivors.LoadContent(Content);

            // lines = new Line[survivors.survivors.Count];
            // groupLines = new Line[survivors.survivors.Count]; 

            /*
            for (int i = 0; i < lines.Length; i++)
            {
             //   lines[i] = new Line(survivors.survivors[i].GetPosition(), survivors.survivors[i].objective, 5, Color.Red, GraphicsDevice);
              //  groupLines[i] = new Line(survivors.survivors[i].GetPosition(), survivors.survivors[i].group.position, 5, Color.Indigo, GraphicsDevice); 
            }
            */
            
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            Game1 game = this;

            

            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            worldPos = Vector2.Transform(mousePos, Matrix.Invert(camera.transform));

            /*
            for (int i = 0; i < lines.Length; i++)
            {
               // lines[i] = new Line(survivors.survivors[i].GetPosition(), survivors.survivors[i].objective, 5, Color.Red, GraphicsDevice);
               // lines[i].Update();
               // groupLines[i] = new Line(survivors.survivors[i].GetPosition(), survivors.survivors[i].group.position, 5, Color.Indigo, GraphicsDevice);
               // groupLines[i].Update(); 
            }
            */
            if (keyboardState.IsKeyDown(Keys.C) && oldKeyboardState.IsKeyUp(Keys.C))
            {
              //  survivors.CompileLua();
            }
            if (keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
                paused = !paused;
            }

            if (!paused)
            {
                //survivors.Update();
            }


            // Adam.Update(); 
            player.Update();
            camera.Update(ref game);
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null,
                null, null, null, camera.transform);


            //terrainGenerator.Draw(spriteBatch);

            world.DrawLayer3(spriteBatch); 
            //spriteBatch.Draw(texture, Adam.GetPosition(), Color.White);

            /*
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Draw(spriteBatch);
                groupLines[i].Draw(spriteBatch); 
            }

            */

            // terrainGenerator.DrawMaterials(spriteBatch); 
            world.DrawLayer2(spriteBatch);
            //survivors.Draw(spriteBatch);

            // terrainGenerator.DrawTrees(spriteBatch);
            world.DrawLayer1(spriteBatch); 
            fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;




            spriteBatch.End();


            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, "FPS: " + ((int)fps).ToString(), new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(spriteFont, "Mouse Position: " + ((int)worldPos.X / 32).ToString() + " - " + ((int)worldPos.Y / 32).ToString(), new Vector2(50, 75), Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
