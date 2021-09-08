using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Floor floorPrefab;
    private List<Floor> floors;
    public int floorSizeX = 70, floorSizeZ = 50;
    public int numFloors = 3;
    private int stairX = -1;
    private int stairZ = -1;
    
    void Start()
    {
        floors = new List<Floor>();
        for (int i = 0; i < numFloors; i++)
        {
            Floor currFloor = Instantiate(floorPrefab) as Floor;
            MazeCell endCell = currFloor.Generate((float) i * 3 + (float) 0.05, floorSizeX, floorSizeZ, stairX, stairZ);
            stairX = endCell.X - 1;
            stairZ = endCell.Z - 1;
            floorSizeX -= 2;
            floorSizeZ -= 2;
            // StartCoroutine(currFloor.Generate());
            floors.Add(currFloor);
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
