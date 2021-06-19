using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Coastline : MonoBehaviour
{
    Mesh mesh;
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
        GameObject ocean = GameObject.Find("Ocean");
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
        xSize = 10;
        zSize = 10;
        int count = (xSize + 1) * (zSize + 1);
        vertices = new Vector3[count];
        int counter = 0;
        //How many vertices to place on the coastline
        int token = 100;
        //Generate a first point
        while (counter < token)
        {
            vertices[counter] = new Vector3(Random.Range(centerX, centerX+xSize), islandHeight,
                Random.Range(centerZ, centerZ+zSize));
            counter++;

        }

        //While not out of tokens, generate points next to them
        
    }

    void GenerateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            return;
        }
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
