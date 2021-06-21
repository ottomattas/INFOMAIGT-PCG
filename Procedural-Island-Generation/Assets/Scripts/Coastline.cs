using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Coastline : MonoBehaviour
{
    Mesh mesh;
    public Vector3[] vertices;
    public List<Vector3> allPossibleVertices;
    int[] lines;
    public float islandHeight;
    public float centerX;
    public float centerZ;
    public float xSize;
    public float zSize;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public float maxXSize;
    public float maxZSize;
    // Start is called before the first frame update
    void Start()
    {
        GameObject ocean = GameObject.Find("Ocean");
        islandHeight = ocean.transform.position.y + 0.01f;
        centerX = 0;
        centerZ = 0;
        xSize = 10;
        zSize = 10;
        minX = centerX - xSize / 2;
        maxX = centerX + xSize / 2;
        minZ = centerZ - zSize / 2;
        maxZ = centerZ + zSize / 2;
        maxXSize = 50;
        maxZSize = 50;
        for (float i = centerX - maxXSize / 2; i < centerX + maxXSize / 2; i++)
        {
            for (float j = centerZ - maxZSize / 2; j < centerZ + maxZSize; j++)
            {
                allPossibleVertices.Add(new Vector3(i, islandHeight, j));
            }
        }
        mesh = new Mesh();
        GenerateCoastline();
        GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCoastline()
    {
        //TODO: 
        //
        List<Vector3> tempvertices = new List<Vector3>();
        for (int i = 0; i < allPossibleVertices.Count; i++)
        {
            if (allPossibleVertices[i].x >= minX && allPossibleVertices[i].x <= maxX &&
            allPossibleVertices[i].z >= minZ && allPossibleVertices[i].z <= maxZ)
            {
                tempvertices.Add(allPossibleVertices[i]);
            }
        }
        while (true)
        {
            int index = Random.Range(0, tempvertices.Count);
            Vector3 Random_Selected = tempvertices[index];
            //If the array is on the border, we can continue
            if (isBorder(Random_Selected, tempvertices))
            {
                break;
            }
        }
        vertices = tempvertices.ToArray();
    }

    void GenerateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
    }

    private bool isBorder(Vector3 LocationToCheck, List<Vector3> vertices)
    {
        Vector3[] tempLocations = new Vector3[]{
            new Vector3(LocationToCheck.x - 1, islandHeight, LocationToCheck.z);
            new Vector3(LocationToCheck.x + 1, islandHeight, LocationToCheck.z);
            new Vector3(LocationToCheck.x, islandHeight, LocationToCHeck.z - 1);
            new Vector3(LocationToCheck.x, islandheight, LocationToCheck.z + 1);
        };
        foreach (Vector3 elem in tempLocactions)
        {
            if (!allPossibleVertices.Contains(elem))
            {
                return false;
            }
            if (!vertices.Contains(elem))
            {
                return true;
            }
        }
        return false;
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

class CoastAgent
{
    Vector3 seed;
    Vector3 direction;
    public int tokens;
    Vector3 attractor;
    Vector3 repulsor;
    public CoastAgent(Vector3 seed, int tokens, float xSize, float minX, float islandHeight, float zSize, float minZ)
    {
        this.seed = seed;
        this.direction = Random.insideUnitCircle;
        this.tokens = tokens;
        this.attractor = new Vector3(Random.Range(minX,maxX), islandHeight, Random.Range(minZ, maxZ));
        while (true)
        {
            Vector3 temprepulsor = new Vector3(Random.Range(minX, maxX), islandHeight, Random.Range(minZ, maxZ));
            if (Vector3.Angle(attractor, temprepulsor) >= 15.0f)
            {
                this.repulsor = temprepulsor;
                break;
            }
        }

    }
    
    public float score(Vector3 EvaluationPoint)
    {
        return Vector3.Distance(repulsor, EvaluationPoint) - Vector3.Distance(attractor, EvaluationPoint);
    }
}
