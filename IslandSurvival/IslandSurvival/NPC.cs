using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua;
using Microsoft.Xna.Framework.Content; 
namespace IslandSurvival
{
    public class NPC
    {

        public Vector2 position;
        private string name;
        private byte strength;
        private byte speed;
        private byte intellect;
        private byte dexterity;
        private byte health;
        private byte maxHealth;
        public Group group; 
        private Inventory inventory;
        private List<Group.Task> tasks;
        public List<Command> taskQueue;
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
            navPoints = new List<Point>(); 
            this.position = position; 
            


        }

        public  void LoadContent(ContentManager content)
        {
            inventory.LoadContent(content);            
            inventory.npcInventory[0].npcObject = inventory.objects[6];
            LoadLua(); 
        }

        public void LoadLua()
        {
            lua = new Lua();
            lua.RegisterFunction("RefreshTasks", this, GetType().GetMethod("RefreshTasks"));
            lua.RegisterFunction("GetTask", this, GetType().GetMethod("GetTask"));
            lua.RegisterFunction("MoveTo", this, GetType().GetMethod("MoveTo"));
            lua.RegisterFunction("NavigateTo", this, GetType().GetMethod("NavigateTo"));
            lua.RegisterFunction("GetNavigation", this, GetType().GetMethod("GetNavigation")); 
            lua.RegisterFunction("InventoryContains", this, GetType().GetMethod("InventoryContains"));
            lua.RegisterFunction("AddCommand", this, GetType().GetMethod("AddCommand"));
            lua.RegisterFunction("RemoveCommand", this, GetType().GetMethod("RemoveCommand"));
            lua.RegisterFunction("GetCommand", this, GetType().GetMethod("GetCommand"));
            lua.RegisterFunction("GetCommandNum", this, GetType().GetMethod("GetCommandNum"));
            lua.RegisterFunction("GetCommandEnum", this, GetType().GetMethod("GetCommandEnum"));
            lua.RegisterFunction("Distance", this, GetType().GetMethod("Distance"));
            lua.RegisterFunction("GetPosition", this, GetType().GetMethod("GetPosition"));
            lua.RegisterFunction("GetPointNum", this, GetType().GetMethod("GetPointNum"));
            lua.RegisterFunction("RemovePoint", this, GetType().GetMethod("RemovePoint")); 
            lua.RegisterFunction("Pickup", this, this.GetType().GetMethod("Pickup"));
            lua.RegisterFunction("Destroy", this, this.GetType().GetMethod("Destroy"));
            lua.RegisterFunction("Build", this, this.GetType().GetMethod("Build"));
            lua.RegisterFunction("Drop", this, this.GetType().GetMethod("Drop"));
            lua.RegisterFunction("Kill", this, this.GetType().GetMethod("Kill"));
            

            lua.DoFile("Lua/NPCLogic.lua"); 
        }

        public void Update()
        {
            lua.GetFunction("Update").Call();
        }

        #region lua functions
        public void RefreshTasks()
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

        public Group.Task GetTask()
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

        public void NavigateTo(int x, int y)
        {
            
            AStar navigate = new AStar();
            AStar.Grid grid = new AStar.Grid(Game1.sizeX, Game1.sizeY, 1);
            List<Point> points = grid.Pathfind(new Point((int)position.X / 32, (int)position.Y / 32), new Point(x, y));

            //this.points = points.Count;
            navPoints = points;
        }

        public Vector2 GetNavigation(int x, int y, int index)
        {


            if (index <= navPoints.Count - 1)
            {
                return new Vector2(navPoints[index].X * 32, navPoints[index].Y * 32);
            }
            return new Vector2(navPoints[navPoints.Count - 1].X * 32, navPoints[navPoints.Count - 1].Y * 32);
        }
        
        public void MoveTo(int x, int y, float ammount)
        {
            position = Vector2.Lerp(position, new Vector2(x, y), ammount); 
        }

        public bool InventoryContains(string name)
        {
            for(int i =0; i < inventory.npcInventory.Count(); i++)
            {
                if(inventory.npcInventory[i].npcObject.name == name)
                {
                    return true; 
                }
            }
            return false; 
        }

        public void AddCommand(string type, int x = 0, int y = 0, int i = 0)
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
                        command = new Command(new Point(x, y), Command.CommandType.Drop, i); 
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

        public void RemoveCommand(int index)
        {
           
            taskQueue.RemoveAt(index); 
        }

        public Command GetCommand(int index)
        {
            return taskQueue[index]; 
        }

        public int GetCommandNum()
        {
            return taskQueue.Count(); 
        }

        public int GetCommandEnum(Command.CommandType type)
        {
            return (int)type; 
        }

        public float Distance(int x, int y)
        {
            return Vector2.Distance(position, new Vector2(x, y));
        }

        public int GetPointNum()
        {
            return navPoints.Count(); 
        }

        public void RemovePoint(int index)
        {
            navPoints.RemoveAt(index);
        }

        public Vector2 GetPosition()
        {
            return position;
        }


        #region world interation
        public int Pickup(int x, int y)
        {
            group.inventory.AddToInventory(World.Pickup(x, y), new Point(x, y));  
            return World.Pickup(x, y); 
        }

        public void Build(int x, int y, int i)
        {
            World.Build(x, y, i); 
        }

        public void Destroy(int x, int y)
        {
            World.Destroy(x, y);
        }
        
        public void Drop(int x, int y, int i)
        {
            World.Drop(x, y, i); 
        }

        public void Kill(int i)
        {

        }
        #endregion
        #endregion
              
        
    }
}
