using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;
using Runtime;

namespace Agents
{
    /// <summary>
    /// A simple low poly ground generator based on fractal noise
    /// </summary>
    public static class GroundGenerator
    {
        [Serializable]
        public class Config
        {
            public Vector3 groundSize = new Vector3(32, 5, 32);
            public float cellSize = 0.5f;
            public float noiseFrequency = 4;
            public Gradient gradient = ColorE.Gradient(Color.black, Color.white);
        }

        public static MeshDraft GroundDraft(Config config)
        {
            //Assert.IsTrue(config.groundSize.x > 0);
            //Assert.IsTrue(config.groundSize.z > 0);
            //Assert.IsTrue(config.cellSize > 0);

            // Create an variable for an array of vertices with 3 points each
            // Vector3[] coastlineVertices;
            List<Vector3> allPossibleVertices;

            // Get the coastline vertices
            GameObject Coast = GameObject.Find("Coastline");
            Coastline coastline = Coast.GetComponent<Coastline>();
            allPossibleVertices = coastline.allPossibleVertices;
            Debug.Log(allPossibleVertices.Count);

            

            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));

            GameObject Coast = GameObject.Find("Coastline");
            Coastline coastline = Coast.GetComponent<Coastline>();
            Vector3[] coastvertices = coastline.vertices;
            List<Vector3> allPossibleVertices = coastline.allPossibleVertices;
            int xSize = coastline.xSize;
            int zSize = coastline.zSize;

            

            float xStep = 1;
            float zStep = 1;
            int vertexCount = allPossibleVertices.Count;
            var draft = new MeshDraft
            {
                name = "Ground",
                //vrtices = allPossibleVertices,
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(vertexCount),
                normals = new List<Vector3>(vertexCount),
                colors = new List<Color>(vertexCount)
            };

            for (int i = 0; i < vertexCount; i++)
            {
                draft.vertices.Add(Vector3.zero);
                draft.triangles.Add(0);
                draft.normals.Add(Vector3.zero);
                draft.colors.Add(Color.black);
            }

            var noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
            noise.SetFrequency(config.noiseFrequency);

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    int index0 = 6*(x + z*xSize);
                    int index1 = index0 + 1;
                    int index2 = index0 + 2;
                    int index3 = index0 + 3;
                    int index4 = index0 + 4;
                    int index5 = index0 + 5;

                    float height00 = GetHeight(x + 0, z + 0, xSize, zSize, noiseOffset, noise);
                    float height01 = GetHeight(x + 0, z + 1, xSize, zSize, noiseOffset, noise);
                    float height10 = GetHeight(x + 1, z + 0, xSize, zSize, noiseOffset, noise);
                    float height11 = GetHeight(x + 1, z + 1, xSize, zSize, noiseOffset, noise);


                    Vector3 vertex00;
                    Vector3 vertex01;
                    Vector3 vertex10;
                    Vector3 vertex11;
                    if (coastvertices.Any(elem => elem.x == x + 0 && elem.z == z + 0))
                    {
                        vertex00 = new Vector3((x + 0)*xStep, height00*config.groundSize.y, (z + 0)*zStep);
                    }
                    else
                    {
                        vertex00 = new Vector3((x + 0)*xStep, -1, (z+0)*zStep);
                    }
                    if (coastvertices.Any(elem => elem.x == x + 0 && elem.z == z + 1))
                    {
                        vertex01 = new Vector3((x + 0)*xStep, height01*config.groundSize.y, (z + 1)*zStep);
                    }
                    else
                    {
                        vertex01 = new Vector3((x + 0)*xStep, -1, (z+1)*zStep);
                    }
                    if (coastvertices.Any(elem => elem.x == x + 1 && elem.z == z + 0))
                    {
                        vertex10 = new Vector3((x + 1)*xStep, height10*config.groundSize.y, (z + 0)*zStep);
                    }
                    else
                    {
                        vertex10 = new Vector3((x + 1)*xStep, -1, (z+0)*zStep);
                    }
                    if (coastvertices.Any(elem => elem.x == x + 1 && elem.z == z + 1))
                    {
                        vertex11 = new Vector3((x + 1)*xStep, height11*config.groundSize.y, (z + 1)*zStep);
                    }
                    else
                    {
                        vertex11 = new Vector3((x + 0)*xStep, -1, (z+0)*zStep);
                    }
                    
                    draft.vertices[index0] = vertex00;
                    Debug.Log(index1);
                    draft.vertices[index1] = vertex01;
                    draft.vertices[index2] = vertex11;
                    draft.vertices[index3] = vertex00;
                    draft.vertices[index4] = vertex11;
                    draft.vertices[index5] = vertex10;

                    draft.colors[index0] = config.gradient.Evaluate(height00);
                    draft.colors[index1] = config.gradient.Evaluate(height01);
                    draft.colors[index2] = config.gradient.Evaluate(height11);
                    draft.colors[index3] = config.gradient.Evaluate(height00);
                    draft.colors[index4] = config.gradient.Evaluate(height11);
                    draft.colors[index5] = config.gradient.Evaluate(height10);

                    Vector3 normal000111 = Vector3.Cross(vertex01 - vertex00, vertex11 - vertex00).normalized;
                    Vector3 normal001011 = Vector3.Cross(vertex11 - vertex00, vertex10 - vertex00).normalized;

                    draft.normals[index0] = normal000111;
                    draft.normals[index1] = normal000111;
                    draft.normals[index2] = normal000111;
                    draft.normals[index3] = normal001011;
                    draft.normals[index4] = normal001011;
                    draft.normals[index5] = normal001011;

                    draft.triangles[index0] = index0;
                    draft.triangles[index1] = index1;
                    draft.triangles[index2] = index2;
                    draft.triangles[index3] = index3;
                    draft.triangles[index4] = index4;
                    draft.triangles[index5] = index5;
                }
            }

            return draft;
        }

        private static float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, FastNoise noise)
        {
            float noiseX = x/(float) xSegments + noiseOffset.x;
            float noiseZ = z/(float) zSegments + noiseOffset.y;
            return noise.GetNoise01(noiseX, noiseZ);
        }
        /// <summary>
        /// Returns a noise value between 0.0 and 1.0
        /// </summary>
        public static float GetNoise01(this FastNoise noise, float x, float y)
        {
            return Mathf.Clamp01(noise.GetNoise(x, y)*0.5f + 0.5f);
        }
        /// <summary>
        /// Update the height value based on coastline
        /// </summary>
        // public static float AdjustHeight()
        // {
        //     foreach (Vector3 vector in allPossibleVertices){  
        //         Console.WriteLine(String.Format("{0},{0},{0}", vector.x, vector.y, vector.z));  
        //     }
        //     var query = (from vert in allPossibleVertices 
        //                  where vert.y == "0"  
        //                  select vert)  
        //                 .Update(st => { st.y = "-1";}); 
        //     Console.WriteLine("After update");
        // }
    }
}
