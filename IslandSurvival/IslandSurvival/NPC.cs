using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua; 

namespace IslandSurvival
{
    public class NPC
    {

        private Vector2 position;
        private string name;
        private byte strength;
        private byte speed;
        private byte intellect;
        private byte dexterity;
        private byte health;
        private byte maxHealth;
        private Group group; 
        private Inventory inventory;
        private List<Group.Task> tasks;
        private List<Command> taskQueue;
        private List<Point> navPoints; 
        private Lua lua; 
        
        

        public NPC(NPC_Creator npcCreator, Vector2 position)
        {
            name = npcCreator.name;
            strength = npcCreator.strenth;
            speed = npcCreator.speed;
            intellect = npcCreator.intellect;
            dexterity = npcCreator.dexterity;
            inventory = new Inventory(4);
            taskQueue = new List<Command>();
            tasks = new List<Group.Task>(); 
        }
        private void LoadLua()
        {
            lua = new Lua();
            lua.RegisterFunction("RefreshTasks", this, GetType().GetMethod("RefreshTasks"));
            lua.RegisterFunction("GetTask", this, GetType().GetMethod("GetTask"));
            lua.RegisterFunction("MoveTo", this, GetType().GetMethod("MoveTo"));
            lua.RegisterFunction("NavigateTo", this, GetType().GetMethod("NavigateTo"));
            lua.RegisterFunction("GetNavigation", this, GetType().GetMethod("GetNavigation")); 
            lua.RegisterFunction("GetItem", this, GetType().GetMethod("GetItem"));
            lua.RegisterFunction("AddCommand", this, GetType().GetMethod("AddCommand"));
            lua.RegisterFunction("RemoveCommand", this, GetType().GetMethod("RemoveCommand")); 
            lua.RegisterFunction("Distance", this, GetType().GetMethod("Distance"));
            lua.RegisterFunction("GetPosition", this, this.GetType().GetMethod("GetPosition"));
            lua.RegisterFunction("Pickup", this, this.GetType().GetMethod("Pickup"));
            lua.RegisterFunction("Destroy", this, this.GetType().GetMethod("Destroy"));
            lua.RegisterFunction("Build", this, this.GetType().GetMethod("Build"));
            lua.RegisterFunction("Drop", this, this.GetType().GetMethod("Drop"));
            lua.RegisterFunction("Kill", this, this.GetType().GetMethod("Kill"));

        }
        
        #region lua functions
        private void RefreshTasks()
        {
            for(int i = 0; i < group.TaskNum(); i++)
            {
                if(strength >= group.tasks[i].job.strength)
                {
                    if(intellect >= group.tasks[i].job.intellect)
                    {
                        if(dexterity >= group.tasks[i].job.dexterity)
                        {
                            if(speed >= group.tasks[i].job.speed)
                            {
                                tasks.Add(group.tasks[i]);
                                group.tasks.RemoveAt(i); 
                                break; 
                            }
                        }
                    }
                }
            }
        }

        private Group.Task GetTask()
        {
            if(tasks.Count > 0)
            {
                Group.Task tempTask = tasks[0];
                tasks.RemoveAt(0);
                return tempTask; 
            }
            else
            {
                Group.Task idleTask = new Group.Task(); 
                idleTask.job = group.jobs[5];
                return idleTask; 
            }
        }

        private void NavigateTo(int x, int y)
        {

            AStar navigate = new AStar();
            AStar.Grid grid = new AStar.Grid(Game1.sizeX, Game1.sizeY, 1);
            List<Point> points = grid.Pathfind(new Point((int)position.X / 32, (int)position.Y / 32), new Point(x, y));

            //this.points = points.Count;
            navPoints = points;
        }

        private Vector2 GetNavigation(int x, int y, int index)
        {


            if (index <= navPoints.Count - 1)
            {
                return new Vector2(navPoints[index].X * 32, navPoints[index].Y * 32);
            }
            return new Vector2(navPoints[navPoints.Count - 1].X * 32, navPoints[navPoints.Count - 1].Y * 32);
        }

        private void MoveTo(int x, int y, float ammount)
        {
            position = Vector2.Lerp(position, new Vector2(x, y), ammount); 
        }

        private Object GetItem(int i)
        {
            return inventory.inventory[i]; 
        }

        private void AddCommand(string type, int x = 0, int y = 0, int i = 0)
        {
            Command command = new Command(); 
            switch (type)
            {
                case "Move":
                    {
                        command = new Command(new Point(x, y), Command.CommandType.Move); 
                        break;
                    }
                case "Pickup":
                    {
                        command = new Command(new Point(x, y), Command.CommandType.Pickup); 
                        break;
                    }
                case "Destroy":
                    {
                        command = new Command(new Point(x, y), Command.CommandType.Destroy); 
                        break; 
                    }
                case "Drop":
                    {
                        command = new Command(new Point(x, y), Command.CommandType.Drop); 
                        break; 
                    }
                case "Kill":
                    {
                        command = new Command(i, Command.CommandType.Kill); 
                        break; 
                    }
                default:
                    {
                        Console.WriteLine("COMMAND TYPE COULD NOT BE FOUND");
                        break; 
                    }
            }
            taskQueue.Add(command); 
        }

        private void RemoveCommand(int index)
        {
            taskQueue.RemoveAt(index); 
        }

        private float Distance(int x, int y)
        {
            return Vector2.Distance(position, new Vector2(x, y));
        }

        private int GetPointNum()
        {
            return navPoints.Count(); 
        }

        private Vector2 GetPosition()
        {
            return position;
        }


        #region world interation
        private int Pickup(int x, int y)
        {
            return World.Pickup(x, y); 
        }

        private void Build(int x, int y, int i)
        {
            World.Build(x, y, i); 
        }

        private void Destroy(int x, int y)
        {
            World.Destroy(x, y);
        }
        
        private void Drop(int x, int y, int i)
        {
            World.Drop(x, y, i); 
        }

        private void Kill(int i)
        {

        }
        #endregion
        #endregion
              



    }
}
