using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public bool showDebug;
    public int rows = 20;
    public int cols = 40;
    public int branchRate = 0;
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    public int[,] data
    {
        get; private set;
    }

    public ArrayList frontier
    {
        get; private set;
    }

    public ArrayList visited
    {
        get; private set;
    }

    void Awake()
    {
        data = new string[rows, cols];
        frontier = new ArrayList();
        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < cols; x++) {
                data[y, x] = 1;
            }
        }
    }
    
    public void GenerateMaze()
    {
        PrimJarnik();
    }

    void PrimJarnik() 
    {
        int randomX = (int) Random.Range(0, cols-1);
        int randomY = (int) Random.Range(0, rows-1);
        VisitCell(randomY, randomX);
        data[randomY, randomX] = 0;

        while(frontier.Count > 0) {
            int[] cell = GetRandomCell();

        }
    }

    int[] GetRandomCell() {
        float position = (float) Random.Range(0, 1);
        position = Mathf.Pow(position, Mathf.Pow(Mathf.Epsilon, branchRate));
        return (int[]) frontier[(int) position * frontier.Count];
    }

    void VisitCell(int x, int y) {
        visited.Add(new int[] {x, y});
        int[,] neighbors = GetNeighbors(x, y);

        for (int i = 0; i < neighbors.GetLength(0); i++) {
            if (isValidNeighbor(neighbors[i, 0], neighbors[i, 1])) {
                frontier.Add(new int[] { neighbors[i, 0], neighbors[i, 1] });
            }
        }
    }

    int[,] GetNeighbors(int x, int y)
    {
        return new int[4, 2] { { x+1, y }, { x-1, y }, { x, y+1 }, { x, y-1 } };
    }

    bool isValidNeighbor(int x, int y)
    {
        bool inBounds = x >= 0 && x < cols && y >= 0 && y < rows;
        bool isVisited = visited.Contains(new int[] {x, y});
        bool inFrontier = frontier.Contains(new int[] {x, y});
        return inBounds && !isVisited && !inFrontier;
    }

    void CheckCell(int x, int y) {
        
    }

    void CreateSpace(int y, int x)
    {
        ArrayList neighbors = new ArrayList();
        data[y, x] = ".";
        if (x > 0) {
            if (data[y, x-1] == "?") {
                data[y, x-1] = ",";
                int[] neighbor = {y, x-1};
                neighbors.Add(neighbor);
            }
        }
        if (x < cols-1) {
            if (data[y, x+1] == "?") {
                data[y, x+1] = ",";
                int[] neighbor = {y, x+1};
                neighbors.Add(neighbor);
            }
        }
        if (y > 0) {
            if (data[y-1, x] == "?") {
                data[y-1, x] = ",";
                int[] neighbor = {y-1, x};
                neighbors.Add(neighbor);
            }
        }
        if (y < rows-1) {
            if (data[y+1, x] == "?") {
                data[y+1, x] = ",";
                int[] neighbor = {y+1, x};
                neighbors.Add(neighbor);
            }
        }
        ArrayList shuffledNeighbors = shuffle(neighbors);
        foreach (var neighbor in shuffledNeighbors)
        {
            frontier.Add(neighbor);
        }
    }
    ArrayList shuffle(ArrayList neighbors)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int i = 0; i < neighbors.Count; i++ )
        {
            int[] tmp = (int[]) neighbors[i];
            int r = Random.Range(i, neighbors.Count);
            neighbors[i] = neighbors[r];
            neighbors[r] = tmp;
        }
        return neighbors;
    }

    void CreateWall(int y, int x)
    {
        data[y, x] = "#";
    }

    void OnGUI()
    {
        //1
        if (!showDebug)
        {
            return;
        }

        //2
        string[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        //3
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                msg += maze[i, j];
            }
            msg += "\n";
        }

        //4
        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
}
