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

        Texture2D texture;
        List<Line> lines = new List<Line>(); 
        public SurvivorManager()
        {
            groups.Add(new Group("Group1")); 

            survivors.Add(new Character("Adam").NewSurvivor());
            survivors.Add(new Character("Eve").NewSurvivor());
            survivors[0].group = groups[0];
            survivors[1].group = groups[0];

            for (int i = 0; i < survivors.Count(); i++)
            {
                // survivors[i].SetPosition(TerrainGenerator.GetStartPositions()); 
               // lines.Add(new Line(Game1.GetMousePosition(), survivors[i].GetPosition(), ))
                survivors[i].SetPosition(new Vector2(90 * 32, 47 * 32)); 
            }

        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("orangeGuy");
        }

        public void Update()
        {
            for(int i = 0; i < survivors.Count(); i++)
            {
                survivors[i].Update(); 
                
            }

            for(int i =0; i < groups.Count(); i++)
            {
                groups[i].Update(); 
            }
        }
        public void CompileLua()
        {
            for(int i = 0; i < survivors.Count(); i++)
            {
                survivors[i].CompileLua(); 
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < survivors.Count(); i++)
            {
                spriteBatch.Draw(texture, survivors[i].GetPosition(), Color.White); 
            }
        }
    }
}
