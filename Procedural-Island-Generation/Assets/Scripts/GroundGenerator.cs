using System.Collections;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    // Define a new mesh
    Mesh mesh;

    // Define arrays for vertices and triangles
    Vector3[] vertices;
    int[] triangles;

    // Define parameters for defining the grid size
    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start(){
        // Create a new mesh
        mesh = new Mesh();
        // Call it in the mesh filter
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateShape());
    }

    void Update(){
        UpdateMesh();
    }

    // Define a method for creating a shape
    IEnumerator CreateShape(){

        // Define a new Vector3 array with the size of the vertex count 
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // Loop over the vertices from left to right; top to bottom
        // to give the vertices a position in the grid.
        for (int i = 0, z = 0; z <= zSize; z++){
            for (int x = 0; x <= xSize; x++){
                // Define a y parameter
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        // Keep track of the vertices array
        int vert = 0;
        // Keep track of the triangle array
        int tris = 0;

        // Loop over all the squares to give triangle coordinates
        for (int z = 0; z < zSize; z++){
            for (int x = 0; x < xSize; x++){
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

                yield return new WaitForSeconds(.1f);
            }
            vert++;
        }
    }

    void UpdateMesh(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
