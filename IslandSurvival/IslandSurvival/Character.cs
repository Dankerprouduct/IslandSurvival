using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua; 

namespace IslandSurvival
{
    public class Character
    {
        public string name;

        public byte maxHealth;
        public byte health;

        public byte maxHunger;
        public byte hunger;

        public byte maxStamina;
        public byte stamina;

        public byte maxThirst;
        public byte thirst;

        public byte maxSickness;
        public byte sickness;

        public byte happiness;

        Lua lua = new Lua();
        public Character(string name)
        {
            Random random = new Random();
            health = (byte)random.Next(75, 250);
           
            lua.DoFile("Lua/Character.lua");

            maxHealth = (byte)(double)lua["maxHealth"];
            health = (byte)(double)lua["health"];

            maxHunger = (byte)(double)lua["maxHunger"];
            hunger = (byte)(double)lua["hunger"];

            maxStamina = (byte)(double)lua["maxStamina"];
            stamina = (byte)(double)lua["stamina"];

            maxThirst = (byte)(double)lua["maxThirst"];
            thirst = (byte)(double)lua["thirst"];

            maxSickness = (byte)(double)lua["maxSickness"];
            sickness = (byte)(double)lua["sickness"];    
            
                                           
        
        }

        public Survivor NewSurvivor()
        {
            Character refCharacter = this;
            return new Survivor(ref refCharacter); 
        }

        

    }
}
