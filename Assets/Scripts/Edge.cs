using UnityEngine;

public abstract class Edge : MonoBehaviour
{
    public MazeCell FirstCell, SecondCell;
    public MazeDirection Direction;

    public void Initialize(MazeCell firstCell, MazeCell secondCell, MazeDirection direction)
    {
        this.FirstCell = firstCell;
        this.SecondCell = secondCell;
        this.Direction = direction;
        firstCell.SetEdge(direction, this);
        transform.parent = firstCell.Tile.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}
