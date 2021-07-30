using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFace
{
    Mesh mesh;
    int resolution;
    Vector3 normalVector; 
    Vector3 axisA;
    Vector3 axisB;

    public TowerFace(Mesh mesh, int resolution, Vector3 normalVector) 
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.normalVector = normalVector;

        axisA = new Vector3(normalVector.y, normalVector.z, normalVector.x);
        axisB = Vector3.Cross(normalVector, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = normalVector + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                vertices[i] = pointOnUnitCube;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
