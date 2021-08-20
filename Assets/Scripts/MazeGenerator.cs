using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int SizeX = 40, SizeZ = 30;
    MazeCell[,] maze;
    Stack<MazeCell> frontier;
    List<MazeCell> visited;
    public MazeCell cellPrefab;
    public Wall wallPrefab;
    public Passage passagePrefab;

    void Awake()
    {
        maze = new MazeCell[SizeX, SizeZ];
        frontier = new Stack<MazeCell>();
        visited = new List<MazeCell>();
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                MazeCell newCell = CreateCell(x, z);
                // MazeCell westNeighbor = null, southNeighbor = null;
                // if (x > 0) westNeighbor = maze[x-1, z];
                // if (z > 0) southNeighbor = maze[x, z-1];
                // CreateWall(newCell, westNeighbor, MazeDirection.West);
                // CreateWall(newCell, southNeighbor, MazeDirection.North);
                // if (x == SizeX-1) CreateWall(newCell, null, MazeDirection.East);
                // if (z == SizeZ-1) CreateWall(newCell, null, MazeDirection.North);
            }
        }
    }
    
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

    public void GenerateMaze()
    {
        RandomizedDFS();
    }

    void RandomizedDFS() 
    {
        int randomX = Random.Range(0, SizeX);
        int randomZ = Random.Range(0, SizeZ);
        MazeCell initial = maze[randomX, randomZ];
        visited.Add(initial);
        frontier.Push(initial);

        while (frontier.Count > 0) {
            MazeCell cell = frontier.Pop();
            visited.Add(cell);
            List<MazeCell> neighbors = GetNeighbors(cell.X, cell.Z);
            if (neighbors.Count > 0) {
                int position = Random.Range(0, neighbors.Count);
                for (int i = 0; i < neighbors.Count; i++) {
                    MazeCell currNeighbor = neighbors[i];
                    CreatePassage(cell, currNeighbor, MazeDirections.Directions[i]);
                    visited.Add(currNeighbor);
                    if (i != position) frontier.Push(currNeighbor);
                }
                frontier.Push(neighbors[position]);
            }
        }
    }

    List<MazeCell> GetNeighbors(int x, int z)
    {
        List<MazeCell> validNeighbors = new List<MazeCell>();
        MazeCell northNeighbor = null, eastNeighbor = null, southNeighbor = null, westNeighbor = null;
        
        if (z < SizeZ-1) northNeighbor = maze[x, z+1];
        if (x < SizeX-1) eastNeighbor = maze[x+1, z];
        if (z > 0) southNeighbor = maze[x, z-1];
        if (x > 0) westNeighbor = maze[x-1, z];

        if (northNeighbor != null && !visited.Contains(northNeighbor)) {
            validNeighbors.Add(northNeighbor);
        } else {
            CreateWall(maze[x, z], northNeighbor, MazeDirection.North);
        }

        if (eastNeighbor != null && !visited.Contains(eastNeighbor)) {
            validNeighbors.Add(eastNeighbor);
        } else {
            CreateWall(maze[x, z], eastNeighbor, MazeDirection.East);
        }

        if (southNeighbor != null && !visited.Contains(southNeighbor)) {
            validNeighbors.Add(southNeighbor);
        } else {
            CreateWall(maze[x, z], southNeighbor, MazeDirection.South);
        }

        if (westNeighbor != null && !visited.Contains(westNeighbor)) {
            validNeighbors.Add(westNeighbor);
        } else {
            CreateWall(maze[x, z], westNeighbor, MazeDirection.West);
        }

        return validNeighbors;
    }
}
