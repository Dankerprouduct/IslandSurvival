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
            Move,
            Pickup, 
            Build,
            Destroy,
            Drop,
            Kill
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
        public Command(int index, CommandType type)
        {
            this.index = index;
            this.type = type;
        }
    }
}
