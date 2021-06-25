using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agents
{

    [RequireComponent(typeof(MeshFilter))]
    public class Coastline : MonoBehaviour
    {
        Mesh mesh;
        public Vector3[] vertices;
        public List<Vector3> allPossibleVertices;
        public List<Vector3> tempvertices;
        public List<Vector3> borderlist;
        public float islandHeight;
        public int xSize = 100;
        public int zSize = 100;
        public float centerX;
        public float centerZ;
        // Start is called before the first frame update
        void Awake()
        {
            //TODO: WE WANT 3 LISTS/ARRAYS; ALL POSSIBLE VERTICES (THIS IS THE ALLPOSSIBLEVERTICES),
            //LIST OF ALL VERTICES OF THE ISLAND (THIS IS VERTICES) AND THE VERTICES OF THE COAST.
            //VERTICES OF THE COAST WILL GET CHOSEN FROM, THEN NON-ISLAND THINGS GET ADDED TO THE ADJACENT
            //VERTICES ARRAY, GET HIGHEST SCORE FROM ALL OF THE ADJACENT VERTICES ARRAY, ADD THAT ONE TO THE
            //COAST LIST AND REMOVE THE 'CURRENT COAST VECTOR3 FROM SAID VECTOR3 ARRAY'
            
            //Find the GameObject Ocean(which is just a simple blue plane), and save its y value as the height
            //where to place the island
            GameObject ocean = GameObject.Find("Ocean");
            islandHeight = ocean.transform.position.y + 0.01f;
            centerX = xSize / 2;
            centerZ = zSize / 2;

            //Create the list allPossibleVertices
            CreateAllVertices();
            

            //ALL THINGS RELATED TO MESHES IS COMMENTED OUT, IF STUFF DOES NOT WORK, TRY TO ADD IT IN FIRST
            //BEFORE DOING ANYTHING ELSE, STUFF IS WORKING NOW (WITH MESH COMMENTED OUT), IF UNNECESSARY, WE
            //SHOULD KEEP IT OUT TO SAVE SOME TIME
            //Create a mesh
            //mesh = new Mesh();
            tempvertices = new List<Vector3>();
            Vector3 centerpoint = new Vector3(centerX, islandHeight, centerZ);
            tempvertices.Add(centerpoint);
            borderlist = new List<Vector3>();
            borderlist.Add(centerpoint);
            GenerateCoastline();
            vertices = tempvertices.ToArray();
            //GenerateMesh();
            //GetComponent<MeshFilter>().mesh = mesh;
        }

        void CreateAllVertices()
        {
            //This function adds every vector between Vector3(0,0,0) to Vector3(xSize,0,zSize)
            //To the list allPossibleVertices. 
            for (int z = 0; z <= zSize; z++) {
                for (int x = 0; x <= xSize; x++) {
                    // Create a new Vector3 with height 0 and coordinates x and z
                    allPossibleVertices.Add(new Vector3(x, 0, z));
                }
            }
        }

        void GenerateCoastline()
        {
            //First iteration LocationToExpand is the centerpoint

            //Tokens = how many points will be raised above ground
            System.Random r = new System.Random();
            int LowerBound = System.Convert.ToInt32(allPossibleVertices.Count * 0.45);
            int UpperBound = System.Convert.ToInt32(allPossibleVertices.Count * 0.9);
            int tokens = r.Next(LowerBound, UpperBound);
            for (int t = 0; t < tokens; t++)
            {
                Vector3 LocationToExpand = GenerateSeed(borderlist);
                Vector3 attractor = new Vector3(Random.Range(0, xSize), islandHeight,
                    Random.Range(0, zSize));
                Vector3 repulsor;
                while (true)
                {
                    Vector3 temprepulsor = new Vector3(Random.Range(0, xSize), 
                        islandHeight, Random.Range(0, zSize));
                    if (Vector3.Angle(attractor, temprepulsor) >= 15.0f)
                    {
                        repulsor = temprepulsor;
                        break;
                    }
                }
                Vector3[] AdjacentPoints = new Vector3[]{
                    new Vector3(LocationToExpand.x - 1, islandHeight, LocationToExpand.z),
                    new Vector3(LocationToExpand.x + 1, islandHeight, LocationToExpand.z),
                    new Vector3(LocationToExpand.x, islandHeight, LocationToExpand.z - 1),
                    new Vector3(LocationToExpand.x, islandHeight, LocationToExpand.z + 1)
                };
                float score = float.MinValue;
                Vector3 BestPoint = Vector3.zero;
                foreach (Vector3 i in AdjacentPoints)
                {
                    float tempscore = ScoreFunction(i, attractor, repulsor);
                    if (tempscore > score)
                    {
                        score = tempscore;
                        BestPoint = i;
                    }
                }
                tempvertices.Add(BestPoint);
                if (!isBorder(LocationToExpand, tempvertices))
                {
                    borderlist.Remove(LocationToExpand);
                }
                borderlist.Add(BestPoint);
            }
        }

        public Vector3 GenerateSeed(List<Vector3> verticesList)
        {
            int index = Random.Range(0, verticesList.Count);
            Vector3 Random_Selected = verticesList[index];
            return Random_Selected;
        }

        /*void GenerateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
        }*/

        private bool isBorder(Vector3 LocationToCheck, List<Vector3> vertices)
        {
            //isBorder takes 2 arguemnts, a Vector3 and a list of Vector3s.
            //The Vector3 is the Vector for which it needs to be determined if it is on the edge of the island
            //This is the case if in all 4 directions, there is at least one point for which it is the case that:
            //it is part of allPossibleVertices and that the coordinates it not part of vertices
            Vector3[] directions = new Vector3[]{
                new Vector3(LocationToCheck.x - 1, islandHeight, LocationToCheck.z),
                new Vector3(LocationToCheck.x + 1, islandHeight, LocationToCheck.z),
                new Vector3(LocationToCheck.x, islandHeight, LocationToCheck.z - 1),
                new Vector3(LocationToCheck.x, islandHeight, LocationToCheck.z + 1)
            };

            //4 Vectors are created. We first need to check if any of the vectors do not already exist in vertices
            List<Vector3> tempLocations = new List<Vector3>();
            
            foreach (Vector3 direction in directions)
            {
                if(allPossibleVertices.Any(elem => elem.x == direction.x && elem.z == direction.z))
                {
                    tempLocations.Add(direction);
                }
            }
            foreach (Vector3 elem in tempLocations)
            {
                //We have to ensure that an element is part of allpossible vertices and that it is not
                //part yet of the list of vertices
                if (!vertices.Any(vector => elem.x == vector.x && elem.z == vector.z))
                {
                    return true;
                }
            }
            return false;
        }

        public float ScoreFunction(Vector3 EvaluationPoint, Vector3 attractor, Vector3 repulsor)
        {
            float tempdistancescore = 0;
            float[] distancetoalledges = new float[]{
                Vector3.Distance(EvaluationPoint, new Vector3(EvaluationPoint.x, islandHeight, zSize)),
                Vector3.Distance(EvaluationPoint, new Vector3(EvaluationPoint.x, islandHeight, 0)),
                Vector3.Distance(EvaluationPoint, new Vector3(xSize, islandHeight, EvaluationPoint.z)),
                Vector3.Distance(EvaluationPoint, new Vector3(0, islandHeight, EvaluationPoint.z))
            };
            foreach (float x in distancetoalledges)
            {
                if (x > tempdistancescore)
                {
                    tempdistancescore = x;
                }
            }
            float distancescore = (float)System.Math.Pow(tempdistancescore, 2);
            return Vector3.Distance(repulsor, EvaluationPoint) - Vector3.Distance(attractor, EvaluationPoint);// + 3*distancescore;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }

        // Update is called once per frame
        /*void Update()
        {
            
        }*/
    }
}