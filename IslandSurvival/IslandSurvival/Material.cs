using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua; 


namespace IslandSurvival
{
    public class Material
    {
        
        private string name;
        private byte ammount;

        private int health; 
        private byte MaterialId;
        public Vector2 position;
        public bool alive;
        public int index; 

        public Material(ref MaterialType materialType)
        {
            name = materialType.GetName();
            ammount = materialType.GetAmmount();
            LoadLua(name); 
            // load materialid through NLua
            
        }

        public Material(ref MaterialType materialType, Vector2 position)
        {
            name = materialType.GetName();
            ammount = materialType.GetAmmount();
            this.position = position;
            LoadLua(name);   
            
        }
        public Material(ref MaterialType materialType, int type, Vector2 positon)
        {
           
        }


        public int GetId()
        {
            return MaterialId; 
        }
        public void WriteToConsole(string text)
        {
            Console.WriteLine(text); 
        }
        
        private void LoadLua(string fileName)
        {
            Lua lua = new Lua();
            lua.DoFile("Lua/" + fileName +".lua");
            MaterialId = (byte)(double)lua["Id"];
            health = (int)(double)lua["health"];
            alive = (bool)lua["alive"]; 
            
            lua.Close();
            lua.Dispose();
            
        }
        public Vector2 MapPosition
        {
            get
            {
                return new Vector2((int)position.X / 32, (int)position.Y / 32);
            }
        }
        public void Damage(int damage)
        {
            
            health = (int)(health - damage);
            
            if (health <= 0)
            {
                
                alive = false;
                TerrainGenerator.DropMaterial(position, "raw_"+name); 
            }
        }
               

    }
}
