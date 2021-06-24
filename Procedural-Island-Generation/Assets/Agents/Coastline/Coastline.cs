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
        public float centerX;
        public float centerZ;
        public float xSize;
        public float zSize;
        // Start is called before the first frame update
        void Start()
        {
            //TODO: WE WANT 3 LISTS/ARRAYS; ALL POSSIBLE VERTICES (THIS IS THE ALLPOSSIBLEVERTICES),
            //LIST OF ALL VERTICES OF THE ISLAND (THIS IS VERTICES) AND THE VERTICES OF THE COAST.
            //VERTICES OF THE COAST WILL GET CHOSEN FROM, THEN NON-ISLAND THINGS GET ADDED TO THE ADJACENT
            //VERTICES ARRAY, GET HIGHEST SCORE FROM ALL OF THE ADJACENT VERTICES ARRAY, ADD THAT ONE TO THE
            //COAST LIST AND REMOVE THE 'CURRENT COAST VECTOR3 FROM SAID VECTOR3 ARRAY'
            GameObject ocean = GameObject.Find("Ocean");
            //islandHeight = ocean.transform.position.y + 0.01f;
            islandHeight = 0;
            xSize = 50;
            zSize = 50;
            centerX = 25;
            centerZ = 25;
            /*for (float i = centerZ - zSize / 2; i < centerZ + zSize / 2 + 1; i++)
            {
                for (float j = centerX - xSize / 2; j < centerX + xSize / 2 + 1; j++)
                {
                    allPossibleVertices.Add(new Vector3(i, -1, j));
                }
            }*/

            generateallpossiblevertices();
            
            mesh = new Mesh();
            //tempvertices = new List<Vector3>();
            Vector3 centerpoint = new Vector3(centerX, islandHeight, centerZ);
            //tempvertices.Add(centerpoint);
            //borderlist = new List<Vector3>();
            //borderlist.Add(centerpoint);
            //GenerateCoastline();
            //vertices = tempvertices.OrderBy(item => item.x).ThenBy(item => item.z).ToArray();
            vertices = allPossibleVertices.ToArray();
            GenerateMesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        void generateallpossiblevertices()
        {
            int i = 0;
            for (int z = 0; z <= zSize; z++) {
                for (int x = 0; x <= xSize; x++) {
                // Create a new variable for the height of the vertex
                // Access each vertex and give a new array of position points
                    allPossibleVertices.Add(new Vector3(x, 0, z));
                // Count a vertex
                    i++;
                }
            }
        }

        void GenerateCoastline()
        {
            //First iteration LocationToExpand is the centerpoint
            int tokens = 1250;
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
                if (!isBorder(LocationToExpand, tempvertices))
                {
                    borderlist.Remove(LocationToExpand);
                }
                borderlist.Add(BestPoint);
                tempvertices.Add(BestPoint);
            }   
        }

        public Vector3 GenerateSeed(List<Vector3> verticesList)
        {
            int index = Random.Range(0, verticesList.Count);
            Vector3 Random_Selected = verticesList[index];
            return Random_Selected;
        }

        void GenerateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
        }

        private bool isBorder(Vector3 LocationToCheck, List<Vector3> vertices)
        {
            Vector3[] tempLocations = new Vector3[]{
                new Vector3(LocationToCheck.x - 1, islandHeight, LocationToCheck.z),
                new Vector3(LocationToCheck.x + 1, islandHeight, LocationToCheck.z),
                new Vector3(LocationToCheck.x, islandHeight, LocationToCheck.z - 1),
                new Vector3(LocationToCheck.x, islandHeight, LocationToCheck.z + 1)
            };
            foreach (Vector3 elem in vertices)
            {
                if (!allPossibleVertices.Any(vector => elem.x == vector.x && elem.z == vector.z))
                {
                    return false;
                }
                if (!vertices.Any(vector => elem.x == vector.x && elem.z == vector.z))
                {
                    return true;
                }
            }
            return false;
        }

        public float ScoreFunction(Vector3 EvaluationPoint, Vector3 attractor, Vector3 repulsor)
        {
            return Vector3.Distance(repulsor, EvaluationPoint) - Vector3.Distance(attractor, EvaluationPoint);
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