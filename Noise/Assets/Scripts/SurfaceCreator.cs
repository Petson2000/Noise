using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{
    [Range(1, 200)]
    public int resolution = 10;

    private int currentResolution;

    private Mesh mesh;

    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }

        Refresh();
    }

    public void Refresh()
    {
        if(resolution != currentResolution)
        {
            CreateGrid();
        }
    }

    private void CreateGrid()
    {
        currentResolution = resolution;

        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];

        float stepSize = 1f / resolution;

       for (int v = 0, y = 0; y <= resolution; y++)
       {
            for (int x = 0; x <= resolution; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f, y * stepSize - 0.5f);
            }
       }

        mesh.vertices = vertices;

        mesh.triangles = new int[]
        {
            0, 2, 1, 1, 2, 3
        };
    }
}
