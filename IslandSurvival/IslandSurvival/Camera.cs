using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace IslandSurvival
{
    public class Camera
    {
        KeyboardState keyboardState; 
        public Matrix transform;
        Viewport viewPort;
        public Vector2 center;
        float scale = .1f; 
        
        public Camera(Viewport vPort)
        {
            viewPort = vPort; 
        }

        public void Update(ref Game1 game)
        {
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                scale += .01f; 
            }
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                scale -= .01f; 
            }
            Tree tree = new Tree(Vector2.Zero, 0, 0);
             
            if(scale >= 1)
            {
                scale = 1; 
            }
            if(scale <= .1f)
            {
                scale = .1f; 
            }


            center = new Vector2((game.player.GetPosition().X - (game.player.GetRect().Width / 2) - (game.SCREEN_WIDTH / 2)),
                (game.player.GetPosition().Y - (game.player.GetRect().Height / 2) - (game.SCREEN_HEIGHT / 2)));

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) * Matrix.CreateScale(new Vector3(scale, scale, 1)) *
                Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0)); 
        }
    }

    
}
