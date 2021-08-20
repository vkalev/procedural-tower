using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Floor floorPrefab;
    private List<Floor> floors;
    public int numFloors = 1;

    void Start()
    {
        floors = new List<Floor>();
        for (int i = 0; i < numFloors; i++)
        {
            Floor currFloor = Instantiate(floorPrefab) as Floor;
            currFloor.Generate();
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
