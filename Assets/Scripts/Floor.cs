using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private int SizeX, SizeZ;
    // public float stepDelay = 0.000000001f;
    MazeCell[,] maze;
    List<MazeCell> frontier;
    List<MazeCell> visited;
    public MazeCell cellPrefab;
    public Wall wallPrefab;
    public Passage passagePrefab;
    private float FloorNumber;

    public void Generate(float floorNumber, int sizeX, int sizeZ)
    {
        // WaitForSeconds delay = new WaitForSeconds(stepDelay);
        this.FloorNumber = floorNumber;
        this.SizeX = sizeX;
        this.SizeZ = sizeZ;
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
            CreatePassage(currentCell, null, MazeDirections.Directions[neighborData[2]]);
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
		newCell.transform.localPosition = new Vector3(x - SizeX * 0.5f + 0.5f, FloorNumber, z - SizeZ * 0.5f + 0.5f);
        return newCell;
    }

    void CreatePassage(MazeCell cell, MazeCell neighbor, MazeDirection direction)
    {
        if (cell.GetEdge(direction) == null) {
            Passage newPassage = Instantiate(passagePrefab) as Passage;
            newPassage.Initialize(cell, neighbor, direction);
            if (neighbor != null) {
                newPassage = Instantiate(passagePrefab) as Passage;
                newPassage.Initialize(neighbor, cell, direction.GetOpposite());
            }
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

    public MazeCell[] GetMazeBorder (MazeDirection direction)
    {
        MazeCell[] border;
        switch (direction)
        {
            case MazeDirection.North:
                border = new MazeCell[SizeX];
                for (int col = 0; col < SizeX; col++)
                {
                    border[col] = maze[col, 0];
                }
                return border;
            
            case MazeDirection.East:
                border = new MazeCell[SizeZ];
                for (int row = 0; row < SizeZ; row++)
                {
                    border[row] = maze[SizeX-1, row];
                }
                return border;

            case MazeDirection.South:
                border = new MazeCell[SizeX];
                for (int col = 0; col < SizeX; col++)
                {
                    border[col] = maze[col, SizeZ-1];
                }
                return border;

            default:
                border = new MazeCell[SizeZ];
                for (int row = 0; row < SizeZ; row++)
                {
                    border[row] = maze[0, row];
                }
                return border;
        }
    }
}
