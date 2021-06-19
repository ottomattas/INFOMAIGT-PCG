using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Coastline : MonoBehaviour
{
    Mesh mesh;
    public Vector3[] vertices;
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
        int count = 100;
        vertices = new Vector3[count];
        //How many vertices to place on the coastline
        //int token = 100;
        //Generate a first point
        vertices[0] = new Vector3(centerX- xSize / 2, islandHeight, Random.Range(centerZ - zSize/2, centerZ + zSize/2));
        int counter = 1;
        Vector3 Left = vertices[0];
        Vector3 Right = vertices[0];
        bool leftDir = true;
        while (counter < count)
        {
            if (leftDir)
            {
                Vector3 DirectionVector = new Vector3(Random.Range(0f,1f),islandHeight,Random.Range(0f,1f));
                Vector3 tempvector = Vector3.Scale(DirectionVector, Left);
                vertices[counter] = tempvector;
                Left = vertices[counter];
                counter++;
                leftDir = !leftDir;
            }
            else
            {
                Vector3 DirectionVector = new Vector3(Random.Range(0f,1f),islandHeight,Random.Range(0f,1f));
                Vector3 tempvector = Vector3.Scale(DirectionVector, Right);
                vertices[counter] = tempvector;
                Right = vertices[counter];
                counter++;
                leftDir = !leftDir;
            }
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
