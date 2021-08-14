using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public class Pair
    {
        public int y { get; set; }
        public int x { get; set; }

        public Pair(int y, int x)
        {
            this.y = y;
            this.x = x;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair);
        }

        public bool Equals(Pair other)
        {
            return other != null &&
                this.y == other.y &&
                this.x == other.x;
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public bool showDebug;
    public int rows = 20;
    public int cols = 40;
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    int[,] data;
    Stack<Pair> frontier;
    List<Pair> visited;

    void Awake()
    {
        data = new int[rows, cols];
        frontier = new Stack<Pair>();
        visited = new List<Pair>();
        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < cols; x++) {
                data[y, x] = 1;
            }
        }
    }
    
    public void GenerateMaze()
    {
        RandomizedDFS();
    }

    void RandomizedDFS() 
    {
        int randomX = Random.Range(0, cols);
        int randomY = Random.Range(0, rows);
        Pair initial = new Pair(randomY, randomX);
        data[randomY, randomX] = 0;
        frontier.Push(initial);
        visited.Add(initial);

        while (frontier.Count > 0) {
            Pair cell = frontier.Pop();
            List<Pair> neighbors = GetNeighbors(cell.y, cell.x);
            if (neighbors.Count > 0) {
                int position = Random.Range(0, neighbors.Count);
                for (int i = 0; i < neighbors.Count; i++) {
                    Pair currNeighbor = neighbors[i];
                    visited.Add(currNeighbor);
                    if (i != position) frontier.Push(currNeighbor);
                }
                frontier.Push(neighbors[position]);
                CreatePassages(neighbors);
            }
        }
    }

    void CreatePassages(List<Pair> neighbors)
    {
        foreach (Pair neighbor in neighbors)
        {
            Pair leftCell = new Pair(neighbor.y, neighbor.x-1);
            Pair rightCell = new Pair(neighbor.y, neighbor.x+1);
            bool leftIsWall = leftCell.x >= 0 && data[leftCell.y, leftCell.x] == 1;
            bool rightIsWall = rightCell.x < cols && data[rightCell.y, rightCell.x] == 1;
            if (leftIsWall && rightIsWall) data[neighbor.y, neighbor.x] = 0;
        }
    }

    List<Pair> GetNeighbors(int y, int x)
    {
        List<Pair> validNeighbors = new List<Pair>();
        Pair leftNeighbor = new Pair(y, x-1);
        if (leftNeighbor.x >= 0 && !visited.Contains(leftNeighbor)) validNeighbors.Add(leftNeighbor);
        Pair rightNeighbor = new Pair(y, x+1);
        if (rightNeighbor.x < cols && !visited.Contains(rightNeighbor)) validNeighbors.Add(rightNeighbor);
        Pair downNeighbor = new Pair(y-1, x);
        if (downNeighbor.y >= 0 && !visited.Contains(downNeighbor)) validNeighbors.Add(downNeighbor);
        Pair upNeighbor = new Pair(y+1, x);
        if (upNeighbor.y < rows && !visited.Contains(upNeighbor)) validNeighbors.Add(upNeighbor);
        return validNeighbors;
        // return new List<Pair> { new Pair(y, x-1), new Pair(y, x+1), new Pair(y-1, x), new Pair(y+1, x) };
    }


// void PrintFrontier()
    // {
    //     foreach (int[] cell in frontier)
    //     {
    //         Debug.Log(cell[0]);
    //         Debug.Log(cell[1]);
    //     }
    // }

    // int[] GetRandomNeighbor(ArrayList neighbors) {
    //     int position = Random.Range(0, neighbors.Count-1);
    //     int[] randomNeighbor = (int[]) neighbors[position];
    //     return randomNeighbor;
    // }

    // void VisitCell(int y, int x) {
    //     visited.Add(new int[2] {y, x});
    //     int[,] neighbors = GetNeighbors(y, x);
    //     for (int i = 0; i < neighbors.GetLength(0); i++) {
    //         int[] currNeighbor = new int[2] { neighbors[i, 0], neighbors[i, 1] };
    //         bool inBounds = currNeighbor[1] >= 0 && currNeighbor[1] < cols && currNeighbor[0] >= 0 && currNeighbor[0] < rows;
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

    // object[] CheckCell(int y, int x) {
    //     int[] newCell = new int[2] { y+1, x };
    //     if (x % 2 == 1) {
    //         newCell = new int[] { y, x+1 };
    //     }

    //     bool shouldOpen = isValidNeighbor(newCell[0], newCell[1]);
    //     if (!shouldOpen) {
    //         newCell = new int[2] { y-1, x };
    //         if (x % 2 == 1) {
    //             newCell = new int[] { y, x-1 };
    //         }
    //         shouldOpen = isValidNeighbor(newCell[0], newCell[1]);
    //     }

    //     return new object[] { shouldOpen, newCell };
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

    void OnGUI()
    {
        if (!showDebug) return;

        int[,] maze = data;
        string label = "";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                label += maze[i, j].ToString();
            }
            label += "\n";
        }

        //4
        GUI.Label(new Rect(20, 20, 500, 500), label);
    }
}
