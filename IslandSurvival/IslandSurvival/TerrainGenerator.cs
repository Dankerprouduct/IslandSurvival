using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content; 

namespace IslandSurvival
{
    public class TerrainGenerator
    {
        private int width;
        private int  height;
        private int seed;

        static int[,] mapData;

        public float light; 
        static List<Material> materials = new List<Material>();
        static List<Material> rawMaterials = new List<Material>(); 
        
        public List<Texture2D> textures = new List<Texture2D>();

        public static List<Vector2> startPositions = new List<Vector2>(); 

        public static int[,] GetMap()
        {

            return mapData; 
        }

        public static Vector2 GetStartPositions()
        {
            Random random = new Random();
            return startPositions[random.Next(0, startPositions.Count())]; 
        }
        public static List<Material> GetMaterials()
        {
            return materials; 
        }

        public static void DamageMaterial(int index, int ammount)
        {
            if (materials.Count > 0)
            {
                materials[index].Damage((byte)ammount);
            }
        }

        public static void DropMaterial(Vector2 position, string name)
        {
            rawMaterials.Add(new MaterialType(name, 10).NewMaterial(position)); 
        }

        public TerrainGenerator(int width, int height, int seed)
        {
            this.width = width;
            this.height = height;
            this.seed = seed;
            mapData = new int[width, height];

        }

        public void LoadContent(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>("Water1"));
            textures.Add(content.Load<Texture2D>("SandTile1"));
            textures.Add(content.Load<Texture2D>("Grass1"));
            textures.Add(content.Load<Texture2D>("DryTile2"));
            textures.Add(content.Load<Texture2D>("tree"));

            textures.Add(content.Load<Texture2D>("raw_wood")); 

            MapGeneration(width, height);

            for(int x =0; x < mapData.GetLength(0); x++)
            {
                for(int y =0; y < mapData.GetLength(1); y++)
                {
                    if(mapData[x,y] != 0)
                    {
                        startPositions.Add(new Vector2(x * 32, y * 32)); 
                        break;
                    }
                }
            }

            Console.WriteLine("Finished Generating Terrain");
            Console.WriteLine("Tile Count: "+mapData.Length.ToString());
        }

        public void Update(GameTime gameTime)
        {
            light = 1;// ((float)Math.Sin((float)gameTime.TotalGameTime.Seconds + 80) * 5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x =0; x < width; x++)
            {
                for(int y = 0; y< height; y++)
                {
                    
                    if (mapData[x, y] != 0)
                    {
                        spriteBatch.Draw(textures[mapData[x, y]], new Vector2(x * 32, y * 32), Color.White * light);
                    }
                    
                }
            }
        }
                
        public void DrawTrees(SpriteBatch spriteBatch)
        {
            if (materials.Count > 0)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    
                    if (materials[i].GetId() == 1)
                    {
                        spriteBatch.Draw(this.textures[4], materials[i].position, Color.White * light);
                    }

                    if (!materials[i].alive)
                    {
                        materials.RemoveAt(i);
                    }
                }
            }
        }
        public void DrawMaterials(SpriteBatch spritebatch)
        {
            if(rawMaterials.Count > 0)
            {
                for(int i = 0; i < rawMaterials.Count; i++)
                {
                    spritebatch.Draw(this.textures[rawMaterials[i].GetId()], rawMaterials[i].position, Color.White); 
                }
            }
        }
        
        #region // noise 

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
                    
                    if (postGenMap[x, y] <= 0 && postGenMap[x, y] >= .3f)
                    {
                        mapData[x, y] = 0;
                    }
                    if (postGenMap[x, y] <= .6f && postGenMap[x, y] >= .3f)
                    {
                        mapData[x, y] = 1;
                        
                        if (postGenMap[x, y] <= .65f && postGenMap[x, y] >= .55f)
                        {
                             mapData[x, y] = 3;
                        }
                        
                    }
                    if (postGenMap[x, y] <= 1f && postGenMap[x, y] >= .6f)
                    {
                        mapData[x, y] = 2;


                        // loads trees
                        if (random.Next() % 3 == 0)
                        {
                            materials.Add(new MaterialType("Tree", (byte)random.Next(5, 20)).NewMaterial(new Vector2(x * 32 + (random.Next(-25, 25)) , y * 32 + (random.Next(-25, 25))))); 
                            
                            trees++; 
                            
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
