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

        private byte MaterialId;
        public Vector2 position; 

        
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
            //lua.RegisterFunction("WriteToConsole", this, this.GetType().GetMethod("WriteToConsole"));
            
            lua.Close();
            lua.Dispose();
            
        }




    }
}
