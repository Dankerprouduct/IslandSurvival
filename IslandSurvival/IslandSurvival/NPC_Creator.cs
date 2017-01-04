using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using Microsoft.Xna.Framework; 
namespace IslandSurvival
{
    public class NPC_Creator
    {
        public string name;
        public byte strenth;
        public byte speed;
        public byte intellect;
        public byte dexterity; 

        public NPC_Creator(string name)
        {
            this.name = name;
            Lua lua = new Lua();
            lua.DoFile("Lua/GenerateNPC.lua");
            speed = (byte)(double)lua["speed"];
            strenth = (byte)(double)lua["strength"];
            intellect = (byte)(double)lua["intellect"];
            dexterity = (byte)(double)lua["dexterity"]; 
        }

        public NPC SpawnNPC(Vector2 position)
        {
            return new NPC(this, position); 
        }
        
    }
}
