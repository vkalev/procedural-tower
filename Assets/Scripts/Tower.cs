using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    Mesh outerMesh;
    public Floor floorPrefab;
    private List<Floor> floors;
    public int floorSizeX = 70, floorSizeZ = 50;
    public int numFloors = 3;

    void Start()
    {
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
        for (int i = 0; i < numFloors-1; i++)
        {
            Floor currFloor = floors[i];
            Floor topFloor = floors[i + 1];
            foreach (MazeDirection direction in MazeDirections.Directions)
            {
                MazeCell[] currBorder = currFloor.GetMazeBorder(direction);
                MazeCell[] topBorder = topFloor.GetMazeBorder(direction);
                for (int c = 0; c < topBorder.Length; c++)
                {
                    if (c == 0 || c == topBorder.Length-1)
                    {
                        
                    }
                }
            }
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
