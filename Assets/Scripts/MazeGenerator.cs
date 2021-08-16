using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int SizeX = 40, SizeZ = 30;
    MazeCell[,] maze;
    Stack<MazeCell> frontier;
    List<MazeCell> visited;
    public Tile tilePrefab;
    public Wall wallPrefab;
    public Passage passagePrefab;

    void Start()
    {
        maze = new MazeCell[SizeX, SizeZ];
        frontier = new Stack<MazeCell>();
        visited = new List<MazeCell>();
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                MazeCell newCell = CreateCell(x, z);
                MazeCell westNeighbor = null, northNeighbor = null;
                if (x != 0) {
                    westNeighbor = maze[x-1, z];
                }
                if (z != 0) {
                    northNeighbor = maze[x, z-1];
                }
                CreateWall(newCell, westNeighbor, MazeDirection.West);
                CreateWall(newCell, northNeighbor, MazeDirection.North);
                if (x == SizeX-1) {
                    CreateWall(newCell, null, MazeDirection.East);
                }
                if (z == SizeZ-1) {
                    CreateWall(newCell, null, MazeDirection.South);
                }

                // if (x == SizeX-1) {
                //     CreateWall(newCell, null, MazeDirection.South);
                //     CreateWall(newCell, null, MazeDirection.East);
                // } else {
                //     if (x == 0 || z == 0) {
                //         CreateWall(newCell, null, MazeDirection.North);
                //         CreateWall(newCell, null, MazeDirection.West);
                //     } else if (z == SizeZ-1) {
                //         CreateWall(newCell, maze[x, z-1], MazeDirection.North);
                //         CreateWall(newCell, maze[x-1, z], MazeDirection.West);
                //         CreateWall(newCell, null, MazeDirection.East);
                //     } else {
                //         CreateWall(newCell, maze[x, z-1], MazeDirection.North);
                //         CreateWall(newCell, maze[x-1, z], MazeDirection.West);
                //     }
                // }
            }
        }
    }
    
    private MazeCell CreateCell(int x, int z)
    {
        Tile newTile = Instantiate(tilePrefab) as Tile;
        maze[x, z] = new MazeCell(x, z, newTile);
        newTile.name = "Floor Tile " + x + ", " + z;
		newTile.transform.parent = transform;
		newTile.transform.localPosition = new Vector3(x - SizeX * 0.5f + 0.5f, 0f, z - SizeZ * 0.5f + 0.5f);
        return maze[x, z];
    }

    void CreatePassage(MazeCell cell, MazeCell neighbor, MazeDirection direction)
    {
        Passage newPassage = Instantiate(passagePrefab) as Passage;
        newPassage.Initialize(cell, neighbor, direction);
        newPassage = Instantiate(passagePrefab) as Passage;
        newPassage.Initialize(neighbor, cell, direction.GetOpposite());
    }

    void CreateWall(MazeCell cell, MazeCell neighbor, MazeDirection direction)
    {
        Wall newWall = Instantiate(wallPrefab) as Wall;
        newWall.Initialize(cell, neighbor, direction);
        if (neighbor != null) {
            newWall = Instantiate(wallPrefab) as Wall;
            newWall.Initialize(neighbor, cell, direction.GetOpposite());
        }
    }

    public void GenerateMaze()
    {
        // RandomizedDFS();
    }

    void RandomizedDFS() 
    {
        int randomX = Random.Range(0, SizeX);
        int randomZ = Random.Range(0, SizeZ);
        MazeCell initial = maze[randomX, randomZ];
        frontier.Push(initial);
        visited.Add(initial);

        while (frontier.Count > 0) {
            MazeCell cell = frontier.Pop();
            List<MazeCell> neighbors = GetNeighbors(cell.X, cell.Z);
            if (neighbors.Count > 0) {
                int position = Random.Range(0, neighbors.Count);
                for (int i = 0; i < neighbors.Count; i++) {
                    MazeCell currNeighbor = neighbors[i];
                    visited.Add(currNeighbor);
                    CreatePassage(cell, currNeighbor, MazeDirections.Directions[i]);
                    if (i != position) frontier.Push(currNeighbor);
                }
                frontier.Push(neighbors[position]);
            }
        }
    }

    List<MazeCell> GetNeighbors(int x, int z)
    {
        List<MazeCell> validNeighbors = new List<MazeCell>();
        MazeCell northNeighbor = maze[x, z-1];
        if (northNeighbor.Z >= 0 && !visited.Contains(northNeighbor)) validNeighbors.Add(northNeighbor);
        MazeCell eastNeighbor = maze[x+1, z];
        if (eastNeighbor.X < SizeX && !visited.Contains(eastNeighbor)) validNeighbors.Add(eastNeighbor);
        MazeCell southNeighbor = maze[x, z+1];
        if (southNeighbor.Z < SizeZ && !visited.Contains(southNeighbor)) validNeighbors.Add(southNeighbor);
        MazeCell westNeighbor = maze[x-1, z];
        if (westNeighbor.X >= 0 && !visited.Contains(westNeighbor)) validNeighbors.Add(westNeighbor);
        return validNeighbors;
    }
}
