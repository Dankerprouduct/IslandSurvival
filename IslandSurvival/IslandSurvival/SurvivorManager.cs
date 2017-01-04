using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IslandSurvival
{
    public class SurvivorManager
    {

        public List<Survivor> survivors = new List<Survivor>();
        public List<Group> groups = new List<Group>();
        IslandSurvivalLibrary.Name[] names; 
        Texture2D texture;
        List<Line> lines = new List<Line>();

        SpriteFont font;
        
        int numOfSurvivors = 3;
        public SurvivorManager()
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("orangeGuy");

            groups.Add(new Group("Group1", content));

            // survivors.Add(new Character("Adam").NewSurvivor());

            names = content.Load<IslandSurvivalLibrary.Name[]>("Xml/Names");

            for(int i  = 0; i < numOfSurvivors; i++)
            {
                Random random = new Random(i);
                
                survivors.Add(new Character(names[i % names.Length].name).NewSurvivor());
                survivors[i].LoadContent(content);
            }
                       

            for(int i = 0; i < survivors.Count; i++)
            {
                survivors[i].SetGroup(groups[0]);
            }
            //survivors[1].SetGroup(groups[0]); 
            font = content.Load<SpriteFont>("SpriteFont2"); 


            for (int i = 0; i < survivors.Count(); i++)
            {
                
                survivors[i].SetPosition(TerrainGenerator.GetStartPositions()); 
            }


            
            CompileLua(); 
        }

        public void Update()
        {

            for (int i = 0; i < groups.Count(); i++)
            {
                groups[i].Update();
                
            }

            for (int i = 0; i < survivors.Count(); i++)
            {
                survivors[i].Update(); 
                
            }

            
        }
        public void CompileLua()
        {
            for (int i = 0; i < groups.Count(); i++)
            {
                groups[i].CompileLua();
            }

            for (int i = 0; i < survivors.Count(); i++)
            {
                survivors[i].CompileLua(); 
                
            }

            
            Console.WriteLine("Updated Lua Scripts"); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < survivors.Count(); i++)
            {
                spriteBatch.Draw(texture, survivors[i].GetPosition(), Color.White);

                spriteBatch.DrawString(font, survivors[i].name, new Vector2(survivors[i].GetPosition().X - (font.MeasureString(survivors[i].name.ToString())).X / 2,
                    survivors[i].GetPosition().Y - 50), Color.White); 

                spriteBatch.DrawString(font, survivors[i].GetMapPosition().ToString(),
                    new Vector2(survivors[i].GetPosition().X - (font.MeasureString(survivors[i].GetMapPosition().ToString()).X / 2) ,
                    survivors[i].GetPosition().Y - 25), Color.OrangeRed);
            }
        }
    }
}
