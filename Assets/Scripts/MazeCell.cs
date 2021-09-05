using UnityEngine;

public abstract class MazeCell : MonoBehaviour
{
    public int X { get; set; }
    public int Z { get; set; }
    
    private Edge[] edges = new Edge[MazeDirections.Count];
    private int initializedEdgeCount;

    public override bool Equals(object obj)
    {
        return Equals(obj as MazeCell);
    }

    public bool Equals(MazeCell other)
    {
        return other != null &&
            this.X == other.X &&
            this.Z == other.Z;
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Edge GetEdge (MazeDirection direction) 
    {
		return edges[(int) direction];
	}

	public void SetEdge (MazeDirection direction, Edge edge) 
    {
		edges[(int) direction] = edge;
        initializedEdgeCount++;
	}

	public bool IsFullyInitialized() {
		return initializedEdgeCount == MazeDirections.Count;
	}

    public int[] GetRandomNeighborData() {
		int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
        int index;
        for (index = 0; index < MazeDirections.Count; index++) {
            if (edges[index] == null) {
                if (skips == 0) {
                    break;
                }
                skips -= 1;
            }
        }
        int[] directionVector = MazeDirections.GetDirectionVector(MazeDirections.Directions[index]);
        return new int[3] { this.X + directionVector[0], this.Z + directionVector[1], index };
	}
}
