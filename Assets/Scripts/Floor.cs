using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public int SizeX = 40, SizeZ = 30;
    // public float stepDelay = 0.000000001f;
    MazeCell[,] maze;
    // Stack<MazeCell> frontier;
    List<MazeCell> frontier;
    List<MazeCell> visited;
    public MazeCell cellPrefab;
    public Wall wallPrefab;
    public Passage passagePrefab;

    public void Generate()
    {
        // WaitForSeconds delay = new WaitForSeconds(stepDelay);
		maze = new MazeCell[SizeX, SizeZ];
		frontier = new List<MazeCell>();
        int randomX = Random.Range(0, SizeX);
        int randomZ = Random.Range(0, SizeZ);
        MazeCell initial = CreateCell(randomX, randomZ);
        frontier.Add(initial);
		while (frontier.Count > 0) {
			// yield return delay;
			GrowingTreeStep();
		}
    }

    private void GrowingTreeStep()
    {
        int currentIndex = frontier.Count - 1;
        MazeCell currentCell = frontier[currentIndex];
        if (currentCell.IsFullyInitialized()) {
            frontier.RemoveAt(currentIndex);
            return;
        }
        int[] neighborData = currentCell.GetRandomNeighborData();
        if (InBoundary(neighborData[0], neighborData[1])) {
            MazeCell neighbor = maze[neighborData[0], neighborData[1]];
            if (neighbor == null) {
                neighbor = CreateCell(neighborData[0], neighborData[1]);
				CreatePassage(currentCell, neighbor, MazeDirections.Directions[neighborData[2]]);
				frontier.Add(neighbor);
            } else {
                CreateWall(currentCell, neighbor, MazeDirections.Directions[neighborData[2]]);
            }
        } else {
            CreateWall(currentCell, null, MazeDirections.Directions[neighborData[2]]);
        }
    }

    // public void Generate()
    // {
    //     maze = new MazeCell[SizeX, SizeZ];
    //     frontier = new Stack<MazeCell>();
    //     visited = new List<MazeCell>();
    //     for (int x = 0; x < SizeX; x++) {
    //         for (int z = 0; z < SizeZ; z++) {
    //             MazeCell newCell = CreateCell(x, z);
    //             MazeCell westNeighbor = null, southNeighbor = null;
    //             if (x > 0) westNeighbor = maze[x-1, z];
    //             if (z > 0) southNeighbor = maze[x, z-1];
    //             CreateWall(newCell, westNeighbor, MazeDirection.West);
    //             CreateWall(newCell, southNeighbor, MazeDirection.North);
    //             if (x == SizeX-1) CreateWall(newCell, null, MazeDirection.East);
    //             if (z == SizeZ-1) CreateWall(newCell, null, MazeDirection.North);
    //         }
    //     }
    //     RandomizedDFS();
    // }
    
    private MazeCell CreateCell(int x, int z)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        maze[x, z] = newCell;
        newCell.X = x;
        newCell.Z = z;
        newCell.name = "Cell " + x + ", " + z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(x - SizeX * 0.5f + 0.5f, 0f, z - SizeZ * 0.5f + 0.5f);
        return newCell;
    }

    void CreatePassage(MazeCell cell, MazeCell neighbor, MazeDirection direction)
    {
        if (cell.GetEdge(direction) == null) {
            Passage newPassage = Instantiate(passagePrefab) as Passage;
            newPassage.Initialize(cell, neighbor, direction);
            newPassage = Instantiate(passagePrefab) as Passage;
            newPassage.Initialize(neighbor, cell, direction.GetOpposite());
        }
    }

    void CreateWall(MazeCell cell, MazeCell neighbor, MazeDirection direction)
    {
        if (cell.GetEdge(direction) == null) {
            Wall newWall = Instantiate(wallPrefab) as Wall;
            newWall.Initialize(cell, neighbor, direction);
            if (neighbor != null) {
                newWall = Instantiate(wallPrefab) as Wall;
                newWall.Initialize(neighbor, cell, direction.GetOpposite());
            }
        }
    }

    private bool InBoundary (int x, int z)
    {
        return x >= 0 && x < SizeX && z >= 0 && z < SizeZ;
    }

    // void RandomizedDFS() 
    // {
    //     int randomX = Random.Range(0, SizeX);
    //     int randomZ = Random.Range(0, SizeZ);
    //     MazeCell initial = maze[randomX, randomZ];
    //     // visited.Add(initial);
    //     frontier.Push(initial);

    //     while (frontier.Count > 0) {
    //         MazeCell cell = frontier.Pop();
    //         // visited.Add(cell);
    //         if (!visited.Contains(cell)) {
    //             visited.Add(cell);
    //             List<MazeCell> neighbors = GetNeighbors(cell.X, cell.Z);
    //             if (neighbors.Count > 0) {
    //                 int position = Random.Range(0, neighbors.Count);
    //                 for (int i = 0; i < neighbors.Count; i++) {
    //                     MazeCell currNeighbor = neighbors[i];
    //                     CreatePassage(cell, currNeighbor, MazeDirections.Directions[i]);
    //                     // visited.Add(currNeighbor);
    //                     if (i != position && !frontier.Contains(currNeighbor)) frontier.Push(currNeighbor);
    //                 }
    //                 if (!frontier.Contains(neighbors[position])) frontier.Push(neighbors[position]);
    //             }
    //         }
    //     }
    // }

    // List<MazeCell> GetNeighbors(int x, int z)
    // {
    //     List<MazeCell> validNeighbors = new List<MazeCell>();
    //     MazeCell northNeighbor = null, eastNeighbor = null, southNeighbor = null, westNeighbor = null;
        
    //     if (z < SizeZ-1) northNeighbor = maze[x, z+1];
    //     if (x < SizeX-1) eastNeighbor = maze[x+1, z];
    //     if (z > 0) southNeighbor = maze[x, z-1];
    //     if (x > 0) westNeighbor = maze[x-1, z];

    //     if (northNeighbor != null && !visited.Contains(northNeighbor)) {
    //         validNeighbors.Add(northNeighbor);
    //     } else {
    //         CreateWall(maze[x, z], northNeighbor, MazeDirection.North);
    //     }

    //     if (eastNeighbor != null && !visited.Contains(eastNeighbor)) {
    //         validNeighbors.Add(eastNeighbor);
    //     } else {
    //         CreateWall(maze[x, z], eastNeighbor, MazeDirection.East);
    //     }

    //     if (southNeighbor != null && !visited.Contains(southNeighbor)) {
    //         validNeighbors.Add(southNeighbor);
    //     } else {
    //         CreateWall(maze[x, z], southNeighbor, MazeDirection.South);
    //     }

    //     if (westNeighbor != null && !visited.Contains(westNeighbor)) {
    //         validNeighbors.Add(westNeighbor);
    //     } else {
    //         CreateWall(maze[x, z], westNeighbor, MazeDirection.West);
    //     }

    //     return validNeighbors;
    // }
}
