using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int SizeX = 40, SizeZ = 20;
    MazeCell[,] maze;
    Stack<MazeCell> frontier;
    List<MazeCell> visited;
    public Tile tilePrefab;

    void Start()
    {
        maze = new MazeCell[SizeX, SizeZ];
        frontier = new Stack<MazeCell>();
        visited = new List<MazeCell>();
        for (int x = 0; x < SizeX; x++) {
            for (int z = 0; z < SizeZ; z++) {
                CreateCell(x, z);
            }
        }
    }
    
    private void CreateCell(int x, int z)
    {
        Tile newTile = Instantiate(tilePrefab) as Tile;
        maze[x, z] = new MazeCell(x, z, newTile);
        newTile.name = "Floor Tile " + x + ", " + z;
		newTile.transform.parent = transform;
		newTile.transform.localPosition = new Vector3(x - SizeX * 0.5f + 0.5f, 0f, z - SizeZ * 0.5f + 0.5f);
    }

    public void GenerateMaze()
    {
        // RandomizedDFS();
    }

    // void RandomizedDFS() 
    // {
    //     int randomX = Random.Range(0, SizeX);
    //     int randomZ = Random.Range(0, SizeZ);
    //     MazeCell initial = maze[randomX, randomZ];
    //     frontier.Push(initial);
    //     visited.Add(initial);

    //     while (frontier.Count > 0) {
    //         MazeCell MazeCell = frontier.Pop();
    //         List<MazeCell> neighbors = GetNeighbors(MazeCell.X, MazeCell.Z);
    //         if (neighbors.Count > 0) {
    //             int position = Random.Range(0, neighbors.Count);
    //             for (int i = 0; i < neighbors.Count; i++) {
    //                 MazeCell currNeighbor = neighbors[i];
    //                 visited.Add(currNeighbor);
    //                 if (i != position) frontier.Push(currNeighbor);
    //             }
    //             frontier.Push(neighbors[position]);
    //             CreatePassages(neighbors);
    //         }
    //     }
    // }

    // void CreatePassages(List<MazeCell> neighbors)
    // {
    //     foreach (MazeCell neighbor in neighbors)
    //     {
    //         MazeCell leftMazeCell = new MazeCell(neighbor.y, neighbor.x-1);
    //         MazeCell rightMazeCell = new MazeCell(neighbor.y, neighbor.x+1);
    //         bool leftIsWall = leftMazeCell.x >= 0 && maze[leftMazeCell.y, leftMazeCell.x] == 1;
    //         bool rightIsWall = rightMazeCell.x < SizeZ && maze[rightMazeCell.y, rightMazeCell.x] == 1;
    //         if (leftIsWall && rightIsWall) maze[neighbor.y, neighbor.x] = 0;
    //     }
    // }

    // List<MazeCell> GetNeighbors(int x, int z)
    // {
    //     List<MazeCell> validNeighbors = new List<MazeCell>();
    //     MazeCell leftNeighbor = maze[x, z-1];
    //     if (leftNeighbor.Z >= 0 && !visited.Contains(leftNeighbor)) validNeighbors.Add(leftNeighbor);
    //     MazeCell rightNeighbor = new MazeCell(y, x+1);
    //     if (rightNeighbor.x < SizeZ && !visited.Contains(rightNeighbor)) validNeighbors.Add(rightNeighbor);
    //     MazeCell downNeighbor = new MazeCell(y-1, x);
    //     if (downNeighbor.y >= 0 && !visited.Contains(downNeighbor)) validNeighbors.Add(downNeighbor);
    //     MazeCell upNeighbor = new MazeCell(y+1, x);
    //     if (upNeighbor.y < SizeX && !visited.Contains(upNeighbor)) validNeighbors.Add(upNeighbor);
    //     return validNeighbors;
    //     // return new List<MazeCell> { new MazeCell(y, x-1), new MazeCell(y, x+1), new MazeCell(y-1, x), new MazeCell(y+1, x) };
    // }


// void PrintFrontier()
    // {
    //     foreach (int[] MazeCell in frontier)
    //     {
    //         Debug.Log(MazeCell[0]);
    //         Debug.Log(MazeCell[1]);
    //     }
    // }

    // int[] GetRandomNeighbor(ArrayList neighbors) {
    //     int position = Random.Range(0, neighbors.Count-1);
    //     int[] randomNeighbor = (int[]) neighbors[position];
    //     return randomNeighbor;
    // }

    // void VisitMazeCell(int y, int x) {
    //     visited.Add(new int[2] {y, x});
    //     int[,] neighbors = GetNeighbors(y, x);
    //     for (int i = 0; i < neighbors.GetLength(0); i++) {
    //         int[] currNeighbor = new int[2] { neighbors[i, 0], neighbors[i, 1] };
    //         bool inBounds = currNeighbor[1] >= 0 && currNeighbor[1] < SizeZ && currNeighbor[0] >= 0 && currNeighbor[0] < SizeX;
    //         if (inBounds && isValidNeighbor(currNeighbor[0], currNeighbor[1])) {
    //             frontier.Add(currNeighbor);
    //         }
    //     }
    // }

    // bool isValidNeighbor(int y, int x)
    // {
    //     bool isVisited = visited.Contains(new int[2] {y, x});
    //     bool inFrontier = frontier.Contains(new int[2] {y, x});
    //     return !isVisited && !inFrontier;
    // }

    // object[] CheckMazeCell(int y, int x) {
    //     int[] newMazeCell = new int[2] { y+1, x };
    //     if (x % 2 == 1) {
    //         newMazeCell = new int[] { y, x+1 };
    //     }

    //     bool shouldOpen = isValidNeighbor(newMazeCell[0], newMazeCell[1]);
    //     if (!shouldOpen) {
    //         newMazeCell = new int[2] { y-1, x };
    //         if (x % 2 == 1) {
    //             newMazeCell = new int[] { y, x-1 };
    //         }
    //         shouldOpen = isValidNeighbor(newMazeCell[0], newMazeCell[1]);
    //     }

    //     return new object[] { shouldOpen, newMazeCell };
    // }

    // int[,] Shuffle (int[,] neighbors)
    // {
    //     for (int i = 0; i < neighbors.Length-1; i++)
    //     {
    //         int[] temp = new int[2] { neighbors[i, 0], neighbors[i, 1] } ;
    //         int j = Random.Range(i, neighbors.Length-1);
    //         neighbors[i, 0] = neighbors[j, 0];
    //         neighbors[i, 1] = neighbors[j, 1];
    //         neighbors[j, 0] = temp[0];
    //         neighbors[j, 1] = temp[1];
    //     }
    //     return neighbors;
    // }

    // void OnGUI()
    // {
    //     if (!showDebug) return;

    //     int[,] maze = maze;
    //     string label = "";

    //     for (int i = 0; i < SizeX; i++)
    //     {
    //         for (int j = 0; j < SizeZ; j++)
    //         {
    //             label += maze[i, j].ToString();
    //         }
    //         label += "\n";
    //     }

    //     //4
    //     GUI.Label(new Rect(20, 20, 500, 500), label);
    // }
}
