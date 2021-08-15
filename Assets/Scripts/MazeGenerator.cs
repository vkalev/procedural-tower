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
                if (z != 0) {
                    CreateWall(newCell, maze[x, z-1], MazeDirection.North);
                    CreateWall(newCell, maze[x-1, z], MazeDirection.West);
                }
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
        newWall = Instantiate(wallPrefab) as Wall;
        newWall.Initialize(neighbor, cell, direction.GetOpposite());
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
        MazeCell leftNeighbor = maze[x, z-1];
        if (leftNeighbor.Z >= 0 && !visited.Contains(leftNeighbor)) validNeighbors.Add(leftNeighbor);
        MazeCell rightNeighbor = maze[x, z+1];
        if (rightNeighbor.Z < SizeZ && !visited.Contains(rightNeighbor)) validNeighbors.Add(rightNeighbor);
        MazeCell downNeighbor = maze[x-1, z];
        if (downNeighbor.X >= 0 && !visited.Contains(downNeighbor)) validNeighbors.Add(downNeighbor);
        MazeCell upNeighbor = maze[x+1, z];
        if (upNeighbor.X < SizeX && !visited.Contains(upNeighbor)) validNeighbors.Add(upNeighbor);
        return validNeighbors;
    }
}
