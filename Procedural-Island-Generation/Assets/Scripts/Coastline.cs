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
    // Start is called before the first frame update
    void Start()
    {
        ocean = GameObject.Find("Ocean");
        islandHeight = ocean.transform.position.y + 0.01f;
        mesh = new Mesh();
        GenerateCoastline();
        GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCoastline()
    {
        //int xSize = 4;
        //int zSize = 3;
        //int count = 0;
        vertices = new Vector3[]{
            new Vector3(0,islandHeight,0),
            new Vector3(0,islandHeight,1),
            new Vector3(1,islandHeight,0),
            new Vector3(1,islandHeight,1)
        };
        lines = new int[]{
            0,1,1,3,3,2,2,0
        };
        /*for(int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                verticeslist.Add(new Vector3(xSize, islandHeight, zSize));
                lines.Add(count++);
            }
        }
        Vector3[] vertices = verticeslist.ToArray();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(lines.ToArray(), MeshTopology.Lines, 0);*/

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
