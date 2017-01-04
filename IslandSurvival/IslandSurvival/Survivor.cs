using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLua;
using Microsoft.Xna.Framework.Content;

namespace IslandSurvival
{
    public class Survivor
    {
        #region global 
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

        private byte strength;
        private byte speed;
        private byte dexterity;
        private byte intellect;


        //private IslandSurvivalLibrary.Job job; 
        Inventory inventory; 
        
        #endregion


        Vector2 position; 
        Lua lua;

        int points;
        List<Point> navPoints;
        List<Group.Task> mTask;
        public Vector2 objective; 
        
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

            strength = character.strength;
            speed = character.speed;
            dexterity = character.dexterity;
            intellect = character.intellect;

            mTask = new List<Group.Task>();

            inventory = new Inventory(5);

            Console.WriteLine(name+": " + "str: " + strength + " spd: " + speed + " int: " + intellect + " dex: " + dexterity);
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
            lua.RegisterFunction("GetGroupWood", this, this.GetType().GetMethod("GetGroupWood"));
            lua.RegisterFunction("GetGroupStone", this, this.GetType().GetMethod("GetGroupStone"));
            lua.RegisterFunction("CollectWood", this, this.GetType().GetMethod("CollectWood"));
            lua.RegisterFunction("CollectStone", this, this.GetType().GetMethod("CollectStone"));
            lua.RegisterFunction("UpdateTasks", this, this.GetType().GetMethod("UpdateTasks"));
            lua.RegisterFunction("FindTask", this, this.GetType().GetMethod("FindTask"));
            lua.RegisterFunction("GetTask", this, this.GetType().GetMethod("GetTask"));
            lua.RegisterFunction("GetMapPosition", this, this.GetType().GetMethod("GetMapPosition"));
            lua.RegisterFunction("DamageMaterial", this, this.GetType().GetMethod("DamageMaterial"));
            lua.RegisterFunction("GetName", this, this.GetType().GetMethod("GetName"));

            lua.DoFile("Lua/SurvivorLogic.lua");
            //lua.GetFunction("Start").Call();
            Console.WriteLine("Compiled Lua for " + name); 
        }
        public void LoadContent(ContentManager content)
        {
            inventory.LoadContent(content); 
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

        }



        #region Lua Functions

        public Vector2 GetMapPosition()
        {
            return new Vector2((int)(position.X / 32), (int)(position.Y / 32)); 
        }

        public void SetGroup(Group g)
        {
            group = g;
           // Console.WriteLine(name + "'s group has be en set to " + g.name); 
        }

        public void UpdateTasks()
        {
            // maybe something here limiting how many task can be held at once
            //  mTask.Add(group.GetTask());  <-- could now be deprecated...

            
            for(int i = 0; i < group.TaskNum(); i++)
            {
                if(group.tasks[i].job.strength <= strength)
                {
                    if(group.tasks[i].job.speed <= speed)
                    {
                        if(group.tasks[i].job.dexterity <= dexterity)
                        {
                            if(group.tasks[i].job.intellect <= intellect)
                            {
                                mTask.Add(group.tasks[i]);
                                 
                               // group.tasks.RemoveAt(i); 
                                //Console.WriteLine(name + ": Updated Task");
                                                  
                            }
                        }
                    }
                }
            }

        }

        public Group.Task GetTask()
        {
            if (mTask.Count > 0)
            {                                                                
                Group.Task tempTask = mTask[0];
                mTask.RemoveAt(0);
               // Console.WriteLine(name + ": Get Task");
                return tempTask;
            }

            Group.Task idleTask = new Group.Task();
            idleTask.job = group.jobs[5];
            Console.WriteLine(name + ": Default task given"); 
            return idleTask;  
        }

        public void CompleteTask(Group.Task task)
        {
            
            bool navigated = false;
            // loops through inventory to search for prefered or needed object
            if (!navigated)
            {
                for (int i = 0; i < inventory.inventory.Count(); i++)
                {
                    if (task.job.PreferedObject == inventory.inventory[i].name)
                    {
                        // go to job location 
                        NavigateTo(task.location.x, task.location.y);
                        return;
                    }
                    else if (task.job.Objecttype == Enum.GetName(typeof(IslandSurvivalLibrary.Object.ObjectType), 2)) // required object is nothing
                    {
                        // Go to job location
                        NavigateTo(task.location.x, task.location.y);
                        return;
                    }

                }

                // if could not find object then go find object

                for (int i = 0; i < group.inventory.inventory.Count(); i++)
                {
                    if (task.job.Objecttype == Enum.GetName(typeof(IslandSurvivalLibrary.Object.ObjectType), 0))
                    {
                        // go to inventory location
                    }
                    else if (task.job.Objecttype == Enum.GetName(typeof(IslandSurvivalLibrary.Object.ObjectType), 1))
                    {

                    }
                    else if (task.job.Objecttype == Enum.GetName(typeof(IslandSurvivalLibrary.Object.ObjectType), 2))
                    {

                    }
                }
            }


        }

        /// <summary>
        /// Find Tasks gives the location to the job
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public Material FindTask(Group.Task task) 
        {
            
            switch (task.job.name)
            {
                case "FreeTime":
                    {
                        

                        mTask.RemoveAt(0);  
                        return new MaterialType("idle", 0).NewMaterial(position); 
                        
                    }
                case "Forestry":
                    {
                        /*
                        // wood

                        // locate wood

                        Material[] materials = new Material[TerrainGenerator.GetMaterials().Count]; 
                        for(int i = 0; i < materials.Count(); i++)
                        {
                            materials[i] = TerrainGenerator.GetMaterials()[i]; 
                        }

                        float lowestDist = 9999999999;
                        int index = 0; 
                        for(int i = 0; i < materials.Count(); i++)
                        {
                            if (materials[i].GetId() == 1)
                            {
                                float cDist = Distance((int)materials[i].position.X,
                                    (int)materials[i].position.Y);

                                if (cDist < lowestDist)
                                {
                                    lowestDist = cDist;
                                    index = i;
                                    
                                }
                                
                            }


                        }


                        // goto wood   
                        if (materials.Length > 0)
                        {
                            materials[index].index = index;
                            return materials[index];
                        }
                        return new MaterialType("idle", 0).NewMaterial(position); 

                        */
                        break;
                    }
            }

            return null; 
        }
        
        public void DamageMaterial(int i, int ammount)
        {
            TerrainGenerator.DamageMaterial(i, ammount);
            group.AddMaterials(TerrainGenerator.GetMaterials()[i].GetId()); 
        }

        public Vector2 FindTree()
        {
            return Vector2.Zero; 
        }
        public bool CollectWood()
        {
            return group.collectWood; 
        }
        public bool CollectStone()
        {
            
            return group.collectStone; 
        }
        public int GetGroupWood()
        {
            return group.GetWood();
        }
        public int GetGroupStone()
        {
            return group.GetStone();
        }
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

        public string GetName()
        {
            return name; 
        }
        #endregion 

        public void Update()
        {
            lua.GetFunction("Update").Call();
            objective = new Vector2((float)(double)lua["objectiveX"] * 32, (float)(double)lua["objectiveY"] * 32); 
        }

    }
}
