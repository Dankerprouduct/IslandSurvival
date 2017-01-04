using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; 


namespace IslandSurvival
{
    public class Tree
    {
        private Vector2 position;
        private Vector2 mapPosition; 
        private float rotation;

        public byte treeType; 

        public Tree(Vector2 position, int treeType ,float rotation)
        {
            this.treeType = (byte)treeType; 
            this.position = position;
            this.rotation = rotation; 
            this.mapPosition = new Vector2((int)position.X / 32, (int)position.Y / 32);
        }
        public byte TreeType
        {
            get
            {
                return treeType; 
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Vector2 MapPosition
        {
            get
            {
                return new Vector2((int)Position.X / 32, (int)Position.Y / 32); 
            }
        }

        public float Rotation
        {
            get
            {
                return rotation; 
            }
        }

        public void Update()
        {
            // Animation
            // if player in area then check collisions
            // then update health
            

        }


    }
}
