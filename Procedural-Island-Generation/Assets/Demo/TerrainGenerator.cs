using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Agents{
// Require a Mesh Filter on the same object as this script
[RequireComponent(typeof(MeshFilter))]

public class TerrainGenerator : MonoBehaviour
{   

    // Create an variable for an array of vertices with 3 points each
    Vector3[] vertices;
    Vector3[] coastvertices;

    // Create a variable for an array of triangles 
    int [] triangles;

    // Create a variable of type Mesh
    Mesh mesh;

    // Define parameters for the grid size
    public int xSize = 20;
    public int zSize = 20;

    // Use this for intialization
    void Start () {
        GameObject Coast = GameObject.Find("Coastline");
        Coastline coastline = Coast.GetComponent<Coastline>();
        coastvertices = coastline.vertices;

        // Create a new mesh object
        mesh = new Mesh();
        
        // Create a new shape regular or through coroutine; choose one
        CreateShape();
        UpdateMesh();
        // Store the mesh object in the Mesh Filter
        GetComponent<MeshFilter>().mesh = mesh;
        //StartCoroutine(CreateShape());
    }

    /*void Update () {
        // Update mesh with the new shape
        UpdateMesh();
    }*/

    // Function to create a new shape or through coroutine; choose one
    void CreateShape () {
    //IEnumerator CreateShape () {
        // Create a new array of vertices with the maximum size
        vertices = coastvertices;
        // Create an index for accessing a vertex

        // Assign positions for each of the points of the vertices,
        // starting from bottom left to right by row
        for (int i = 0; i < vertices.Length; i++)
        {
            float newy = Mathf.PerlinNoise(vertices[i].x * .3f, vertices[i].z* .3f) * 2f;
            vertices[i][1] = newy;
        }

        // Create a new triangles array with the maximum size
        triangles = new int[xSize * zSize * 6];
        // Create an index for accessing a vertex
        int vert = 0;
        // Create an index for accessing a triangle
        int tris = 0;

        // Iterate over the squares on the Z-axis
        for (int z = 0; z < zSize; z++) {
            // Iterate over the squares on the X-axis
            for (int x = 0; x < xSize; x++) {
                // Store coordinates for the corners; first triangle
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                // Store coordinates for the corners; second triangle
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                // Count a vertex and a triangle
                vert++;
                tris += 6;
                // Coroutine: Wait before each yield
                //yield return new WaitForSeconds(.1f);
            }
            // Count an extra vertex at the end of each row
            vert++;
        }        
    }

    // Function to update mesh with the new shape
    void UpdateMesh () {
        // Clear mesh from previous data
        mesh.Clear();
        // Input vertices array
        mesh.vertices = vertices;
        // Input triangles array
        mesh.triangles = triangles;
        // Recalculate normals for proper lighting
        mesh.RecalculateNormals();
    }

    // Function to draw spheres on each vertex in Editor
    private void OnDrawGizmosSelected () {

        // Return if no vertices exist
        if (vertices == null)
        return;

        // Draw a small sphere on each vertex
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], .01f);
        }
    }

}
}
