using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Coastline : MonoBehaviour
{
    Mesh mesh;
    GameObject ocean;
    Vector3[] vertices;
    int[] lines;
    public float islandHeight;
    public float centerX;
    public float centerZ;
    public int xSize;
    public int zSize;
    // Start is called before the first frame update
    void Start()
    {
        ocean = GameObject.Find("Ocean");
        islandHeight = ocean.transform.position.y + 0.01f;
        centerX = 0;
        centerZ = 0;
        mesh = new Mesh();
        GenerateCoastline();
        GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCoastline()
    {
        xSize = 4;
        zSize = 3;
        //int count = 0;
        vertices = new Vector3[xSize*zSize];
        int count = 0;
        for (float i = centerX - xSize/2; i < centerX+ xSize/2; i++)
        {
            for (float j = centerZ - zSize/2; j < centerZ+zSize/2; j++)
            {
                vertices[count] = new Vector3(i,islandHeight,j);
                count++;
            }
        }
        lines = new int[]{
            0,1,1,3,3,5,5,7,7,9,9,11,11,10,10,8,8,6,6,4,4,2,2,0
        };
    }

    void GenerateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(lines, MeshTopology.Lines, 0);

    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
