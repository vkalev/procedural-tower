using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public int X { get; set; }
    public int Z { get; set; }
    
    private Edge[] edges = new Edge[MazeDirections.Count];

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
	}

    public void DestroyEdge (MazeDirection direction)
    {
        Destroy(edges[(int) direction].gameObject);
        // edges[(int) direction] = null;
    }
}
