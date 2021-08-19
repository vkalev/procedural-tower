using UnityEngine;

[RequireComponent(typeof(MazeGenerator))]
public class Floor : MonoBehaviour
{
    private MazeGenerator generator;

    void Start()
    {
        generator = GetComponent<MazeGenerator>();
        generator.GenerateMaze();
    }
}
