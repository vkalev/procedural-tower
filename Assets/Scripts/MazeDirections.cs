using UnityEngine;

public static class MazeDirections
{
    public const int Count = 4;

    public static readonly MazeDirection[] Directions = { 
        MazeDirection.North, 
        MazeDirection.East, 
        MazeDirection.South, 
        MazeDirection.West 
    };

    private static MazeDirection[] opposites = { 
        MazeDirection.South, 
        MazeDirection.West, 
        MazeDirection.North, 
        MazeDirection.East 
    };

    public static MazeDirection GetOpposite (this MazeDirection direction) {
		return opposites[(int) direction];
	}

    private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};
	
	public static Quaternion ToRotation (this MazeDirection direction) {
		return rotations[(int) direction];
	}
}
