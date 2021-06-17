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
    public float startingX;
    public float startingZ;
    // Start is called before the first frame update
    void Start()
    {
        ocean = GameObject.Find("Ocean");
        islandHeight = ocean.transform.position.y + 0.01f;
        startingX = 0;
        startingZ = 0;
        mesh = new Mesh();
        GenerateCoastline();
        GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateCoastline()
    {
        int xSize = 4;
        int zSize = 3;
        //int count = 0;
        vertices = new Vector3[]{
            new Vector3(startingX,islandHeight,startingZ),
            new Vector3(startingX,islandHeight,startingZ + zSize),
            new Vector3(startingX + xSize,islandHeight,startingZ),
            new Vector3(startingX + xSize,islandHeight,startingZ + zSize)
        };
        lines = new int[]{
            0,1,1,3,3,2,2,0
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
