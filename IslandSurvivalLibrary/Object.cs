using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IslandSurvivalLibrary
{
    public class Object
    {
        public enum ObjectType
        {
            Weapon, 
            Tool,
            Nothing
        }

        public string name;
        public int id;
        public ObjectType type; 
    }
}
