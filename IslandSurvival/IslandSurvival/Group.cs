using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua; 


namespace IslandSurvival
{
    public class Group
    {

        private string name;
        private int wealth;
        private int wood = 0; 
        private int stone = 0;
        private int age = 1; 

        Lua lua; 

        public struct Task
        {
            public int taskType;
            public int priority;
            public int taskNumber; 
        }

        List<Task> tasks = new List<Task>(); 
        public Group(string group_name)
        {
            name = group_name;
            CompileLua(); 
        }
        public void CompileLua()
        {
            lua = new Lua();
            lua.RegisterFunction("GetWealth", this, this.GetType().GetMethod("GetWealth"));
            lua.RegisterFunction("GetWood", this, this.GetType().GetMethod("GetWood"));
            lua.RegisterFunction("GetStone", this, this.GetType().GetMethod("GetStone"));
            lua.RegisterFunction("GenerateNewTask", this, this.GetType().GetMethod("GenerateNewTask")); 

            lua.DoFile("Lua/Group.lua");
        }

        public void Update()
        {            
            lua.GetFunction("Update").Call();             
        }



        public void GenerateNewTask(int taskId, int priority = 1)
        {
            Task task = new Task();
            task.taskType = taskId;
            task.priority = priority;
            task.taskNumber = tasks.Count;
            
            
            tasks.Add(task);
            
        }
        public void RemoveTask(int id)
        {
            lua.GetFunction("ResetTask").Call();
            tasks.RemoveAt(id); 
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
