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

        public static int sizeX = 250;
        public static int sizeY = 250;

        bool paused = false;

        NPC npc1;
        Group group1;
        Texture2D group1Texture; 
        Texture2D npc1Texture; 
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

            string seed = "Default";//"kljkbj";
            world = new World(sizeX, sizeY, seed.GetHashCode());
            world.LoadContent(Content);
            Console.WriteLine("Generated world with size of : " + sizeX * sizeY);
            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player(new Vector2((0 * 2) / 2, (0 * 32) / 2));
            
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            group1 = new Group("group1", Content);
            group1Texture = Content.Load<Texture2D>("RedErrorTexture"); 
            group1.CompileLua(); 
            npc1 = new NPC_Creator("David").SpawnNPC(new Vector2 (2784, 2592));
            npc1.group = group1;
            npc1.LoadContent(Content); 
            npc1Texture = Content.Load<Texture2D>("orangeGuy");

            Point point = world.FindGroupLocation(); 
            group1.position = new Vector2(point.X * 32, point.Y * 32);
            Console.WriteLine("GROUPS LOCATION: " + group1.position); 
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

            if (keyboardState.IsKeyDown(Keys.C) && oldKeyboardState.IsKeyUp(Keys.C))
            {
                //  survivors.CompileLua();
                npc1.LoadLua(); 
            }
            if (keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
                paused = !paused;
            }

            if (!paused)
            {
                group1.Update();
                npc1.Update(); 
            }

             
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

            world.DrawLayer3(spriteBatch); 
            
            world.DrawLayer2(spriteBatch);
            //survivors.Draw(spriteBatch);

            
            spriteBatch.Draw(npc1Texture, npc1.position, Color.White); 
            world.DrawLayer1(spriteBatch);

            spriteBatch.Draw(group1Texture, group1.position, Color.White);
            fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.End();


            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, "FPS: " + ((int)fps).ToString(), new Vector2(50, 50), Color.AliceBlue);
            spriteBatch.DrawString(spriteFont, "Mouse Position: " + ((int)worldPos.X / 32).ToString() + " - " + ((int)worldPos.Y / 32).ToString(), new Vector2(50, 75), Color.Red);
            spriteBatch.DrawString(spriteFont, "TaskQueue: " + npc1.taskQueue.Count(), new Vector2(50, 100), Color.AliceBlue);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
