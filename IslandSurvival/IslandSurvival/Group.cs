using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework; 
namespace IslandSurvival
{
    public class Group
    {

        public string name;
        public bool collectWood;
        public bool collectStone;
        public struct Task
        {
            public IslandSurvivalLibrary.Job job;
            public int priority;
            public int index; 

            
            public Point location1;
            public Point location2; 
        }
        public IslandSurvivalLibrary.Job[] jobs;
        public Inventory inventory;
        public List<Task> tasks = new List<Task>();

        private int wealth;
        private int wood = 1; 
        private int stone = 0;
        private int age = 1;

        Lua lua;
        ContentManager content;
        public Vector2 position; 
        public Group(string group_name, ContentManager content)
        {
            this.content = content; 
            name = group_name;
            
        }
        public void CompileLua()
        {

            lua = new Lua();
            lua.RegisterFunction("GetWealth", this, this.GetType().GetMethod("GetWealth"));
            lua.RegisterFunction("GetWood", this, this.GetType().GetMethod("GetWood"));
            lua.RegisterFunction("GetStone", this, this.GetType().GetMethod("GetStone"));
            lua.RegisterFunction("GenerateNewTask", this, this.GetType().GetMethod("GenerateNewTask"));

            jobs = content.Load<IslandSurvivalLibrary.Job[]>("XML/Jobs");
            inventory = new Inventory((byte)20); 
            inventory.LoadContent(content);
            lua.DoFile("Lua/Group.lua");
            lua.GetFunction("Update").Call();

            Console.WriteLine("Compiled lua for " + name);
            
            position = Vector2.Zero;//TerrainGenerator.GetStartPositions();
            Console.WriteLine("Group position:" + position.X/32 + " "+position.Y/32);
        }
        
        
        public void Update()
        {
            lua.GetFunction("Update").Call();
                       
            

        }

        public int TaskNum()
        {
            return tasks.Count; 
        }
                 
        public void AddMaterials(int type = 1, int ammount = 10)
        {
            switch (type)
            {
                case 0:
                    {

                        break;
                    }
                case 1:
                    {
                        // wood
                        wood += ammount; 
                        break;
                    }
                case 2:
                    {
                        stone += ammount; 
                        break; 
                    }
            }
        }
        public void GenerateNewTask(string jobName, int priority = 1)
        {

            Task task = new Task();
            for(int i =0; i < jobs.Length; i++)
            {
                if(jobs[i].name == jobName)
                {
                    task.job = jobs[i]; 
                }
            }
            task.priority = priority;

            // TODO: if job requires location or anythhing else, assign that here
            if(jobName == "Forestry")
            {
                task.location1 = World.LocateLayer1Object(0, position);
                task.location2 = inventory.LocateObject(1, new Point((int)position.X / 32, (int)position.Y /32));
            }

            if (tasks.Count > 0)
            {
                if (tasks.Contains(task))
                {
                    //tasks[tasks.IndexOf(task)].Add();
                    return; 
                }
                else
                {
                    tasks.Add(task);
                   // Console.WriteLine("Existing Task not found. Added task with aa name of: " + task.job.name);
                }
            }
            else
            {
                tasks.Add(task);
               // Console.WriteLine("Existing Task not found. Added task with a name of: " + task.job.name);
            }


            
            //tasks.Add(task);
            
        }
        public float Distance(int x, int y)
        {
            return Vector2.Distance(position, new Vector2(x, y));
        }
        public void RemoveTask(Task task)
        {
            
        }
        public int GetWealth()
        {
            wealth = (int)((wood + stone) / age); 
            return wealth; 
        }
        public int GetWood()
        {
            return wood; 
        }
        public int GetStone()
        {
            return stone; 
        }
    }
}
