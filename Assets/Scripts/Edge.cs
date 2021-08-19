using UnityEngine;

public abstract class Edge : MonoBehaviour
{
    private MazeCell FirstCell, SecondCell;
    private MazeDirection Direction;

    public void Initialize(MazeCell firstCell, MazeCell secondCell, MazeDirection direction)
    {
        this.FirstCell = firstCell;
        this.SecondCell = secondCell;
        this.Direction = direction;
        firstCell.SetEdge(direction, this);
        transform.parent = firstCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}
