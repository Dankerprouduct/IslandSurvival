using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content; 

namespace IslandSurvival
{
    public class Inventory
    {

        IslandSurvivalLibrary.Object[] objects; 
        public IslandSurvivalLibrary.Object[] inventory; 
        public Inventory(int size)
        {

            inventory = new IslandSurvivalLibrary.Object[size]; 
            
            
        }

        public void LoadContent(ContentManager content)
        {
            objects = content.Load<IslandSurvivalLibrary.Object[]>("Xml/Object"); 

            // setting each inventory slot to empty
            for(int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = objects[3];
            }

        }

        public void AddObject(IslandSurvivalLibrary.Object obj)
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i].id == 3) // 3 = "Empty"
                {
                    inventory[i] = obj;
                    break;
                }
            }
        }



    }
}
