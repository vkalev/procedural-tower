using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    MeshFilter meshFilter;
    public Floor floorPrefab;
    private List<Floor> floors;
    public int floorSizeX = 70, floorSizeZ = 50;
    public int numFloors = 3;

    void Start()
    {
        GameObject meshObj = new GameObject("mesh");
        meshObj.transform.parent = transform;

        meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        meshFilter = meshObj.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();

        floors = new List<Floor>();
        for (int i = 0; i < numFloors; i++)
        {
            Floor currFloor = Instantiate(floorPrefab) as Floor;
            currFloor.Generate((float) i * 5, floorSizeX, floorSizeZ);
            floorSizeX -= 2;
            floorSizeZ -= 2;
            // StartCoroutine(currFloor.Generate());
            floors.Add(currFloor);
        }
        GenerateOuterMesh();
    }

    public void GenerateOuterMesh()
    {
        int[] meshCounts = GetMeshCounts();
        Vector3[] vertices = new Vector3[meshCounts[0]];
        int[] triangles = new int[meshCounts[1]];
        int triIndex = 0;

        for (int i = 0; i < numFloors-1; i++)
        {
            Floor currFloor = floors[i];
            Floor topFloor = floors[i + 1];
            foreach (MazeDirection direction in MazeDirections.Directions)
            {
                MazeCell[] currBorder = currFloor.GetMazeBorder(direction);
                MazeCell[] topBorder = topFloor.GetMazeBorder(direction);
                Vector3[] offsets = GetVertexOffsets(direction);
                int currIndex = 0;
                for (int topIndex = 0; topIndex < topBorder.Length; topIndex++)
                {
                    int directionIndexOffset = GetDirectionOffset(direction);
                    int vertexIndex = topIndex + directionIndexOffset + i * (floorSizeX * 2 + floorSizeZ * 2);
                    // Vector3 topLeftVertex, topRightVertex, bottomLeftVertex, botttomRightVertex;
                    // Vector3 topCellPosition = topBorder[topIndex].transform.position;
                    // topLeftVertex = new Vector3(topCellPosition.x + offsets[0].x, topCellPosition.y, topCellPosition.z + offsets[0].z);
                    // topRightVertex = new Vector3(topCellPosition.x + offsets[1].x, topCellPosition.y, topCellPosition.z + offsets[1].z);
                    // if (topIndex == 0 || topIndex == topBorder.Length-2) {
                    //     Vector3 bottomLeftCellPosition = currBorder[currIndex].transform.position;
                    //     Vector3 bottomRightCellPosition = currBorder[currIndex+1].transform.position;
                    //     bottomLeftVertex = new Vector3(bottomLeftCellPosition.x + offsets[0].x, bottomLeftCellPosition.y, bottomLeftCellPosition.z + offsets[0].z);
                    //     botttomRightVertex = new Vector3(bottomRightCellPosition.x + offsets[1].x, bottomRightCellPosition.y, bottomRightCellPosition.z + offsets[1].z);
                    //     currIndex++;
                    // } else {
                    //     Vector3 bottomCellPosition = currBorder[currIndex].transform.position;
                    //     bottomLeftVertex = new Vector3(bottomCellPosition.x + offsets[0].x, bottomCellPosition.y, bottomCellPosition.z + offsets[0].z);
                    //     botttomRightVertex = new Vector3(bottomCellPosition.x + offsets[1].x, bottomCellPosition.y, bottomCellPosition.z + offsets[1].z);
                    //     currIndex++;
                    // }
                    Vector3 currCellPosition = currBorder[currIndex].transform.position;
                    Vector3 currVertex = new Vector3(currCellPosition.x + offsets[0].x, currCellPosition.y, currCellPosition.z + offsets[0].z);
                    if (topIndex == 0) currIndex++;
                    if (topIndex == topBorder.Length-2) {
                        vertices[vertexIndex] = currVertex;
                        Vector3 endCellPosition = currBorder[currIndex+1].transform.position;
                        vertices[vertexIndex+1] = new Vector3(endCellPosition.x + offsets[1].x, endCellPosition.y, endCellPosition.z + offsets[1].z);
                        topIndex++;
                    } else {
                        vertices[vertexIndex] = currVertex;
                    }

                    if (topIndex != topBorder.Length-1 && i != numFloors-1)
                    {
                        triangles[triIndex] = vertexIndex;
                        triangles[triIndex+1] = vertexIndex + (floorSizeX * 2 + floorSizeZ * 2) + 1;
                        triangles[triIndex+2] = vertexIndex + (floorSizeX * 2 + floorSizeZ * 2);

                        triangles[triIndex+3] = vertexIndex;
                        triangles[triIndex+4] = vertexIndex + 1;
                        triangles[triIndex+5] = vertexIndex + (floorSizeX * 2 + floorSizeZ * 2) + 1;
                        triIndex += 6;
                    }
                    currIndex++;
                }
            }
        }
        meshFilter.sharedMesh.Clear();
        meshFilter.sharedMesh.vertices = vertices;
        meshFilter.sharedMesh.triangles = triangles;
        meshFilter.sharedMesh.RecalculateNormals();
    }

    private int[] GetMeshCounts ()
    {
        int vertexCount = 0;
        int triangleCount = 0;
        for (int i = 0; i < numFloors; i++)
        {
            if (i != numFloors-1)
            {
                triangleCount += ((floorSizeX * 2 - 1) + (floorSizeZ * 2 - 1)) * 6;
            }
            vertexCount += (floorSizeX * 2 + floorSizeZ * 2);
            floorSizeX += 2;
            floorSizeZ += 2;
        }
        return new int[2] {vertexCount, triangleCount};
    }

    private Vector3[] GetVertexOffsets (MazeDirection direction)
    {
        Vector3[] offsets = new Vector3[2];
        switch (direction)
        {
            case MazeDirection.North:
                offsets[0] = new Vector3((float) -0.5, 0, (float) 0.5);
                offsets[1] = new Vector3((float) 0.5, 0, (float) 0.5);
                break;
            case MazeDirection.East:
                offsets[0] = new Vector3((float) 0.5, 0, (float) -0.5);
                offsets[1] = new Vector3((float) 0.5, 0, (float) 0.5);
                break;
            case MazeDirection.South:
                offsets[0] = new Vector3((float) -0.5, 0, (float) -0.5);
                offsets[1] = new Vector3((float) 0.5, 0, (float) -0.5);
                break;
            default:
                offsets[0] = new Vector3((float) -0.5, 0, (float) 0.5);
                offsets[1] = new Vector3((float) -0.5, 0, (float) -0.5);
                break;
        }
        return offsets;
    } 

    private int GetDirectionOffset (MazeDirection direction)
    {
        switch (direction)
        {
            case MazeDirection.North:
                return 1;
            case MazeDirection.East:
                return floorSizeZ;
            case MazeDirection.South:
                return floorSizeX + floorSizeZ;
            default:
                return floorSizeX + floorSizeZ * 2;
        }
    }

    public void DestroyFloors()
    {
        foreach (Floor floor in floors)
        {
            Destroy(floor.gameObject);
        }
    }
}
