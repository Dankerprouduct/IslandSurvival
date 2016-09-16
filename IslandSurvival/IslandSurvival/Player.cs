using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; 

namespace IslandSurvival
{
    public class Player
    {
        Vector2 position;
        Rectangle rectangle;
        KeyboardState keyboardState;
        float speed = 50f;
       
        public Player(Vector2 startPosition)
        {
            position = startPosition; 
        }

        public Vector2 GetPosition()
        {
            return position; 
        }
        public Rectangle GetRect()
        {
            return rectangle; 
        }

        public void Update()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
            {
                position.Y -= speed; 
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                position.Y += speed; 
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                position.X += speed; 
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                position.X -= speed; 
            }

            if (keyboardState.IsKeyDown(Keys.L))
            {
                Material mat = new MaterialType("Tree", 2).NewMaterial(); 

                
            }
            
        }
    }
}
