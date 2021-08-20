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

    public static int[] GetDirectionVector (MazeDirection direction)
    {
        switch (direction)
        {
            case MazeDirection.North:
                return new int[2] { 0, 1 };
            case MazeDirection.East:
                return new int[2] { 1, 0 };
            case MazeDirection.South:
                return new int[2] { 0, -1 };
            default:
                return new int[2] { -1, 0 };
        }
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
