using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; 

namespace IslandSurvival
{
    public class Command
    {
        public enum CommandType
        {
            Move = 0,
            Pickup = 1, 
            Build = 2,
            Destroy = 3,
            Drop = 4,
            Kill = 5
        }

        public Point location;
        public CommandType type;
        public int index; 
        public Command()
        {

        }
        public Command(Point location, CommandType type)
        {
            this.location = location;
            this.type = type; 
        }
        public Command(Point location, CommandType type, int index)
        {
            this.location = location;
            this.type = type;
            this.index = index; 
        }
        public Command(int index, CommandType type)
        {
            this.index = index;
            this.type = type;
        }
    }
}
