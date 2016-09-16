using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; 

namespace IslandSurvival
{
    public class MaterialType
    {
        private string name;
        private byte ammount;
        
        public MaterialType(string name, byte ammount)
        {
            this.name = name;
            this.ammount = ammount;
        }

        public Material NewMaterial()
        {
            MaterialType materialRef = this; 
            return new Material(ref materialRef);
        }
        public Material NewMaterial(Vector2 position)
        {
            MaterialType materialRef = this;
            return new Material(ref materialRef, position); 
        }


        public string GetName ()
        {
            return name; 
        }
        public byte GetAmmount()
        {
            return ammount; 
        }
        



    }
}
