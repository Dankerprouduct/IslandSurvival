using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua; 

namespace IslandSurvival
{
    public class Survivor
    {
        public string name;

        private byte maxHealth;
        public byte health;

        private byte maxHunger;
        private byte hunger;

        private byte maxStamina;
        private byte stamina;

        private byte maxThirst;
        private byte thirst;

        private byte maxSickness;
        private byte sickness;

        private byte happiness;

        Vector2 position; 
        Lua lua;

        int points;
        List<Point> navPoints;

        public Group group
        {
            get;
            set; 
        }
        public Survivor(ref Character character)
        {
            
            name = character.name;
            maxHealth = character.maxHealth;
            health = character.health;
            maxHunger = character.maxHunger;
            hunger = character.hunger;
            maxStamina = character.maxStamina;
            stamina = character.stamina;
            maxThirst = character.maxThirst;
            thirst = character.thirst;
            maxSickness = character.maxSickness;
            sickness = character.sickness;


            CompileLua();

           // NavigateTo(2500, 2505);
        }

        public void CompileLua()
        {
            lua = new Lua(); 
            lua.RegisterFunction("MoveTo", this, this.GetType().GetMethod("MoveTo"));
            lua.RegisterFunction("GetPosition", this, this.GetType().GetMethod("GetPosition"));
            lua.RegisterFunction("Distance", this, this.GetType().GetMethod("Distance"));
            lua.RegisterFunction("NavigateTo", this, this.GetType().GetMethod("NavigateTo"));
            lua.RegisterFunction("GetNavigation", this, this.GetType().GetMethod("GetNavigation"));
            lua.RegisterFunction("GetPoints", this, this.GetType().GetMethod("GetPoints"));
            lua.DoFile("Lua/SurvivorLogic.lua");
        }

        public Vector2 GetNavigation(int x, int y, int index)
        {


            if (index <= navPoints.Count - 1 )
            {
                return new Vector2(navPoints[index].X * 32, navPoints[index].Y * 32);
            }
            return new Vector2(navPoints[navPoints.Count - 1].X * 32, navPoints[navPoints.Count - 1].Y * 32);
        }
        public void SetPosition(Vector2 position)
        {
            this.position = position;
            CompileLua(); 
        }
        
        #region Lua Functions
        public int GetPoints()
        {
            return points; 
        }
        public void NavigateTo(int x, int y )
        {
            
            AStar navigate = new AStar();
            AStar.Grid grid = new AStar.Grid(Game1.sizeX, Game1.sizeY, 1);
            List<Point> points =  grid.Pathfind(new Point((int)position.X / 32, (int)position.Y / 32), new Point(x,y));

            this.points = points.Count;
            navPoints = points; 
            //return points;
        }
        
        public Vector2 GetPosition()
        {
            return position; 
            
        }

        public float Distance(int x, int y)
        {
            return Vector2.Distance(position, new Vector2(x,y)); 
        }

        public void MoveTo(int x, int y, float ammount)
        {
            position = Vector2.Lerp(position, new Vector2(x, y), ammount); 
        }
        #endregion 

        public void Update()
        {
            lua.GetFunction("Update").Call();
        }

    }
}
