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
        private float rotation;
        private bool Alive;
        private byte health = 25;
        public byte treeType; 

        public Tree(Vector2 position, int treeType ,float rotation)
        {
            this.treeType = (byte)treeType; 
            this.position = position;
            this.rotation = rotation; 
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
