using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace IslandSurvival
{
    public class Inventory
    {


        public struct GroupInventory
        {
            public IslandSurvivalLibrary.Object iObject;
            public Point coordinate;
            public int stack; 
        }
        public struct NPCInventory
        {
            public IslandSurvivalLibrary.Object npcObject;
            public int stack; 
        }
        public NPCInventory[] npcInventory;
        public List<GroupInventory> groupInventory;
        public IslandSurvivalLibrary.Object[] objects;
        private int radius; 

        
        /// <summary>
        /// [GROUP] only use this constructor for groups
        /// </summary>
        public Inventory(byte objectRadius)
        {
            radius = objectRadius; 
            groupInventory = new List<GroupInventory>(); 
        }

        /// <summary>
        /// [NPC] Only use this constructor for npc & players
        /// </summary>
        /// <param name="size"></param>
        public Inventory(int size = 4)
        {
            npcInventory = new NPCInventory[size];
            
        }

        public void LoadContent(ContentManager content)
        {
            objects = content.Load<IslandSurvivalLibrary.Object[]>("Xml/Object");


            if (npcInventory != null)
            {
                Console.WriteLine("Loading NPC  Inventory"); 
                for (int i = 0; i < npcInventory.Length; i++)
                {
                    npcInventory[i] = new NPCInventory();
                    npcInventory[i].npcObject = objects[7];

                }
            }
                      

        }

        public void AddToInventory(int id)
        {

            // NPC inventory
            if(npcInventory != null)
            {
                for(int i = 0; i < npcInventory.Length; i++)
                {
                    // first check if object already exist, if it does add it to stack 
                    if(npcInventory[id].npcObject.id == id)
                    {
                        npcInventory[id].stack++;
                        return; 
                    }
                }

                for(int  i = 0; i < npcInventory.Length; i++)
                {
                    // if empy slot
                    if (npcInventory[i].npcObject.id == 7)
                    {
                        npcInventory[i].npcObject = objects[id];
                        npcInventory[i].stack = 1; 
                        return;
                    }
                }

                Console.WriteLine("could not add object with id of: " + id + " to NPC inventory");
                
            }
                                  

        }

        public void AddToInventory(int id, Point point)
        {
            if (groupInventory.Count() > 0)
            {
                for (int i = 0; i < groupInventory.Count; i++)
                {
                    if (groupInventory[i].iObject.id == id)
                    {
                        GroupInventory tempInventory = groupInventory[i];
                        tempInventory.stack++;
                        groupInventory[i] = tempInventory;
                    }
                }
            }
            else
            {
                GroupInventory tempInventory = new GroupInventory();
                tempInventory.iObject = objects[id];
                tempInventory.stack = 1;
                tempInventory.coordinate = point;
                groupInventory.Add(tempInventory); 
            }
        }

        public Point LocateObject(int id, Point groupLocation)
        {

            // search for avalible stack
            if (groupInventory.Count > 0)
            {
                for (int i = 0; i < groupInventory.Count(); i++)
                {
                    if (groupInventory[i].iObject.id == id)
                    {
                        return groupInventory[i].coordinate;
                    }
                    else
                    {
                        return EmptySpace(groupLocation);
                    }
                }
            }
            else
            {
                return groupLocation; 
            }

            Console.WriteLine("Could not locate object"); 
            return new Point(); 
        }

        private Point EmptySpace(Point origin)
        {

            for(int r = 0; r < radius; r++)
            {
                for (int z = 0; z < 360; z ++)
                {
                    int tileX = r + origin.X;
                    int tileY = r + origin.Y;

                    float mRadius = (float)Math.Pow(tileX - origin.X, 2) + 
                        (float)Math.Pow(tileY - origin.Y, 2);
                    mRadius = (float)Math.Sqrt(mRadius);

                    tileX = (int)(Math.Cos(z) * mRadius) + origin.X;
                    tileY = (int)(Math.Sin(z) * mRadius) + origin.Y;

                    Point point = new Point(tileX, tileY); 
                    if(tileX >= 0 && tileX < World.GetWidth())
                    {
                        if(tileY >= 0 && tileY < World.GetHeight())
                        {
                            for(int i = 0; i < groupInventory.Count(); i++)
                            {
                                if(groupInventory[i].coordinate != point)
                                {
                                    return point; 
                                }
                            }
                        }
                    }
                    
                }
            }

            Console.WriteLine("Could not locate empty space");
            return new Point(); 
        }
                
        public void Draw(SpriteBatch spriteBatch)
        {

            for(int i = 0; i < groupInventory.Count(); i++)
            {
                spriteBatch.Draw(World.Layer1Textures[3], new Rectangle(groupInventory[i].coordinate.X * 32,
                    groupInventory[i].coordinate.Y * 32, 16, 16), new Rectangle(0, 0, 32, 32), Color.White); 
            }

        }
    }
}
