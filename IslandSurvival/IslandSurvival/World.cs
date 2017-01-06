using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace IslandSurvival
{
    public class World
    {
        private int width;
        private int height;
        private int seed;
        
        #region Layers
        /// <summary>
        /// Layer 1 is where all of the structures are kept (Buildings, Trees, Rocks, etc).
        /// </summary>
        static int[,] layer1;
        /// <summary>
        /// Layer 2 is where players, NPCs and structures from Layer 1 drop things to.
        /// </summary>
        static int[,] layer2;
        /// <summary>
        /// Layer 3 is where the terrain is kept (Regular Tiles)
        /// </summary>
        static int[,] layer3;
        #endregion

        private List<Texture2D> Layer3Textures;
        private List<Texture2D> Layer2Textures;
        private List<Texture2D> Layer1Textures; 


        public World(int width = 250, int height = 250, int seed = 0)
        {
            this.width = width;
            this.height = height;
            this.seed = seed;

            layer1 = new int[width, height];
            layer2 = new int[width, height];
            layer3 = new int[width, height]; 

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    layer1[x, y] = 101;
                    layer2[x, y] = 101;
                    layer3[x, y] = 101; 
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            #region Layer 3 Textures
            Layer3Textures = new List<Texture2D>();
            Layer3Textures.Add(content.Load<Texture2D>("Water1"));
            Layer3Textures.Add(content.Load<Texture2D>("SandTile1"));
            Layer3Textures.Add(content.Load<Texture2D>("Grass1"));
            Layer3Textures.Add(content.Load<Texture2D>("DryTile2"));
            
            #endregion

            #region Layer 2 Textures
            Layer2Textures = new List<Texture2D>();
            Layer2Textures.Add(content.Load<Texture2D>("raw_wood"));
            #endregion

            #region Layer 1 Textures
            Layer1Textures = new List<Texture2D>();
            Layer1Textures.Add(content.Load<Texture2D>("tree"));
            Layer1Textures.Add(content.Load<Texture2D>("stone")); // REPLACE THIS TREE WITH STONE
            Layer1Textures.Add(content.Load<Texture2D>("WoodWall"));
            Layer1Textures.Add(content.Load<Texture2D>("StoneWall"));
            Layer1Textures.Add(content.Load<Texture2D>("RedErrorTexture"));
            #endregion
            MapGeneration(width, height);
                                
        }

        public Point FindGroupLocation()
        {
            
            int tileX; 
            int tileY;
            int radius = 10;
            
            List<Vector3> points = new List<Vector3>(); 
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {

                    List<float> Values = new List<float>();
                    for (int r = 0; r < radius; r++)
                    {
                        
                        for (int z = 0; z < 360; z++)
                        {
                            tileX = r + x;
                            tileY = r + y;

                            float mRadius = (float)Math.Pow(tileX - x, 2) + (float)Math.Pow(tileY - y, 2);
                            mRadius = (float)Math.Sqrt(mRadius);

                            tileX = (int)(Math.Cos(z) * mRadius + x);
                            tileY = (int)(Math.Sin(z) * mRadius + y);

                            
                            if (tileX >= 0 && tileX < width)
                            {
                                if (tileY >= 0 && tileY < height)
                                {
                                    //Console.WriteLine(tileX + " , " + tileY);
                                    Values.Add(AddTempTile1(tileX, tileY));
                                    Values.Add(AddTempTile2(tileX, tileY));
                                }
                            }
                        }
                    }


                    float max = 0;
                    
                    for(int i = 0; i < Values.Count(); i++)
                    {
                        max += Values[i]; 
                    }

                    max /= Values.Count(); 
                                        
                    points.Add(new Vector3(x, y, max)); 
                }
            }


            Point point = new Point();
            float pointMax = 0;
            for (int i = 0; i < points.Count(); i++)
            {
                
                
                if(points[i].Z > pointMax)
                {
                    point = new Point((int)points[i].X, (int)points[i].Y);
                    pointMax = points[i].Z; 
                    //Console.WriteLine("Point Max: " + points[i].Z);  
                }

            }


            
            
            for (int z = 0; z < 360; z++)
            {
                tileX = radius + point.X;
                tileY = radius + point.Y;
                float mRadius = (float)Math.Pow(tileX - point.X, 2) + (float)Math.Pow(tileY - point.Y, 2);
                mRadius = (float)Math.Sqrt(mRadius);
                
                //Console.WriteLine(mRadius); 
                tileX = (int)(Math.Cos(z) * mRadius) + point.X;
                tileY = (int)(Math.Sin(z) * mRadius) + point.Y;
                if (tileX >= 0 && tileX < width)
                {
                    if (tileY >= 0 && tileY < height)
                    {
                        //Console.WriteLine("radius: "+ tileX + " , " + tileY);                         
                        layer1[tileX, tileY] = 4;                      

                    }
                }
            }

            Console.WriteLine("BEST POINT IS: " + point); 
            return point; 
        }

        public float AddTempTile1(int x, int y)
        {

            switch (layer1[x, y])
            {
                case 101:
                    {
                        return 1.6f;
                    }
                case 0: // tree
                    {
                        
                        return 0; 
                    }
                case 1: // stone
                    {
                        return .1f; 
                    }
            }
            
            
            return 0; 
        }

        public float AddTempTile2(int x, int y)
        {
            
            switch (layer3[x, y])
            {

                case 101:
                    {
                        //.WriteLine("101");
                        return 0;
                    }
                case 0: // water
                    {
                        return 1.205f;
                    }
                case 1: // Sand Tile
                    {
                        //Console.WriteLine("1");
                        return 1.1f;
                    }
                case 2: // Grass
                    {
                        //Console.WriteLine("2");
                        return 1.3f;
                    }
                case 3: // Dirt
                    {
                        //Console.WriteLine("3");
                        return 1.4f;
                    }

            }
            
            return 0; 
        }

        public static int[,] GetMap() // PathFinding uses this
        {
            return layer3;
        }

        #region world interation
        public static void Build(int x, int y, int i)
        {
            layer1[x,y] = i; 
        }
        public static void Destroy(int x, int y)
        {
            Drop(x, y, layer1[x, y]);
            layer1[x, y] = 101; 
        }
       
        public static void Drop(int x, int y, int i)
        {
            layer2[x, y] = i; 
        }
        public static int Pickup(int x, int y)
        {
            int tempId = layer2[x, y];
            layer2[x, y] = 101;
            return tempId;  
        }

        #region Locate
        public static Point LocateLayer1Object(int id, Vector2 postion)
        {
            float dist;
            float lowest = 9999999999;
            Point point = new Point(0, 0); 
            for (int x = 0; x < GetMap().GetLength(0); x++)
            {
                for(int y = 0; y < GetMap().GetLength(1); y++)
                {
                    if(layer1[x,y] == id)
                    {
                        dist = Vector2.Distance(postion, new Vector2(x * 32, y * 32));
                        
                        if(dist < lowest)
                        {
                            lowest = dist;
                            point = new Point(x, y);
                            
                        }
                                                
                    }
                }
            }
            return point;
           // return new Point();  
        }

        public static Point LocateLayer2Object(int id)
        {
            for (int x = 0; x < GetMap().GetLength(0); x++)
            {
                for (int y = 0; x < GetMap().GetLength(1); y++)
                {
                    if (layer2[x, y] == id)
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point();
        }

        public static Point LocateLayer3Object(int id)
        {
            for (int x = 0; x < GetMap().GetLength(0); x++)
            {
                for (int y = 0; x < GetMap().GetLength(1); y++)
                {
                    if (layer3[x, y] == id)
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point();
        }
        #endregion

        #endregion

        #region Drawing

        #region Layer 1
        public void DrawLayer1(SpriteBatch spriteBatch)
        {
            DrawStone(spriteBatch);
            DrawShelters(spriteBatch); 
            DrawTrees(spriteBatch);
            
        }

        private void DrawTrees(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layer1[x, y] == 0)
                    {
                        spriteBatch.Draw(Layer1Textures[layer1[x, y]],
                                new Rectangle(32 * x, 32 * y,
                                Layer1Textures[layer1[x, y]].Width,
                                Layer1Textures[layer1[x, y]].Height),
                                null, Color.White, 0f, Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                    }
                }
            }
        }
        private void DrawStone(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layer1[x, y] == 1)
                    {
                        spriteBatch.Draw(Layer1Textures[layer1[x, y]],
                                new Rectangle(32 * x, 32 * y,
                                Layer1Textures[layer1[x, y]].Width,
                                Layer1Textures[layer1[x, y]].Height),
                                null, Color.White, 0f, Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                    }
                }
            }
        }
        private void DrawShelters(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layer1[x, y] != 0 && layer1[x,y] != 1 && layer1[x,y] != 101)
                    {
                        spriteBatch.Draw(Layer1Textures[layer1[x, y]],
                                new Rectangle(32 * x, 32 * y,
                                Layer1Textures[layer1[x, y]].Width,
                                Layer1Textures[layer1[x, y]].Height),
                                null, Color.White, 0f, Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                    }
                }
            }
        }
        

        #endregion

        public void DrawLayer2(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layer2[x,y] != 101)
                    {
                        spriteBatch.Draw(Layer2Textures[layer2[x, y]],
                                new Rectangle(32 * x, 32 * y,
                                Layer2Textures[layer2[x, y]].Width,
                                Layer2Textures[layer2[x, y]].Height),
                                null, Color.White, 0f, Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                    }
                }
            }
        }

        public void DrawLayer3(SpriteBatch spriteBatch)
        {
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layer3[x, y] != 101 && layer3[x,y] != 0)
                    {
                        spriteBatch.Draw(Layer3Textures[layer3[x, y]],
                                new Rectangle(32 * x, 32 * y,
                                Layer3Textures[layer3[x, y]].Width,
                                Layer3Textures[layer3[x, y]].Height),
                                null, Color.White, 0f, Vector2.Zero,
                                SpriteEffects.None,
                                0f);
                    }
                }
            }
        }
        #endregion
        
        #region  World Generation

        void MapGeneration(int width, int height)
        {

            float[,] preGenMap = WhiteNoise(width, height);

            int octave = 6;

            float[,] postGenMap = GeneratePerlinNoise(preGenMap, octave);


            Random random = new Random(seed);

            int trees = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    if (postGenMap[x, y] <= .3 && postGenMap[x, y] >= 0f)
                    {
                        
                        layer3[x, y] = 0;
                    }
                    if (postGenMap[x, y] <= .6f && postGenMap[x, y] >= .3f)
                    {
                        layer3[x, y] = 1;
                                               
                        if (postGenMap[x, y] <= .65f && postGenMap[x, y] >= .55f)
                        {
                            layer3[x, y] = 3;
                            
                        }

                    }
                    if (postGenMap[x, y] <= 1f && postGenMap[x, y] >= .6f)
                    {
                        layer3[x, y] = 2;
                        // loads trees
                        if (random.Next() % 3 == 0)
                        {
                            
                            layer1[x, y] = 0;
                        }

                    }
                    if (postGenMap[x, y] <= 1f && postGenMap[x, y] >= .65f)
                    {
                        
                        // loads stone
                        if (random.Next() % 8 == 0)
                        {

                            layer1[x, y] = 1;
                        }

                    }


                }
            }
            Console.WriteLine(trees);



        }

        public float[,] WhiteNoise(int width, int height)
        {
            Random random = new Random(seed);

            float[,] noise = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noise[x, y] = (float)random.NextDouble() % 1;
                }
            }
            return noise;
        }

        float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
        {


            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[,] smoothNoise = new float[width, height];

            int samplePeriod = 1 << octave;

            float sampleFrequency = 1.0f / samplePeriod;

            for (int x = 0; x < width; x++)
            {
                int sample1 = (x / samplePeriod) * samplePeriod;
                int sample2 = (sample1 + samplePeriod) % width;
                float horizonalBlend = (x - sample1) * sampleFrequency;

                for (int y = 0; y < height; y++)
                {
                    int sampley1 = (y / samplePeriod) * samplePeriod;
                    int sampley2 = (sampley1 + samplePeriod) % height;
                    float verticleBlend = (y - sampley1) * sampleFrequency;


                    float top = Interpolate(baseNoise[sample1, sampley1], baseNoise[sample2, sampley1], horizonalBlend);

                    float bottotm = Interpolate(baseNoise[sample1, sampley2], baseNoise[sample2, sampley2], horizonalBlend);

                    smoothNoise[x, y] = Interpolate(top, bottotm, verticleBlend);
                }
            }
            return smoothNoise;
        }

        float[,] GeneratePerlinNoise(float[,] baseNoise, int OctaveCount)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[][,] smoothNoise = new float[OctaveCount][,];


            // reg value = .5f
            float persistence = .5f;

            for (int i = 0; i < OctaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);

            }

            float[,] perlinNoise = new float[width, height];
            float amplitude = .5f;
            float totalAmp = 0.0f;


            for (int octave = OctaveCount - 1; octave > 0; octave--)
            {
                amplitude *= persistence;
                totalAmp += amplitude;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        perlinNoise[x, y] += smoothNoise[octave][x, y] * amplitude;
                    }
                }
            }

            // normalization 
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    perlinNoise[x, y] /= totalAmp;
                    perlinNoise[x, y] = makeMask(width, height, x, y, perlinNoise[x, y]);
                }
            }


            return perlinNoise;
        }

        float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public float distance_squared(int x, int y)
        {
            float dx = 2 * x / width - 1;
            float dy = 2 * y / height - 1;
            // at this point 0 <= dx <= 1 and 0 <= dy <= 1
            return (float)Math.Sqrt(dx * dx + dy * dy);

        }

        public static float makeMask(int width, int height, int posX, int posY, float oldValue)
        {

            int minVal = (((height + width) / 2) / 100 * 2);
            int maxVal = (((height + width) / 2) / 100 * 25);
            if (getDistanceToEdge(posX, posY, width, height) <= minVal)
            {
                return 0;
            }
            else if (getDistanceToEdge(posX, posY, width, height) >= maxVal)
            {
                return oldValue;
            }
            else
            {
                float factor = getFactor(getDistanceToEdge(posX, posY, width, height), minVal, maxVal);
                return oldValue * factor;
            }
        }

        private static float getFactor(int val, int min, int max)
        {
            int full = max - min;
            int part = val - min;
            float factor = (float)part / (float)full;
            return factor;
        }

        public static int getDistanceToEdge(int x, int y, int width, int height)
        {
            int[] distances = new int[] { y, x, (width - x), (height - y) };
            int min = distances[0];
            foreach (var val in distances)
            {
                if (val < min)
                {
                    min = val;
                }
            }
            return min;
        }
        #endregion
    }
}
