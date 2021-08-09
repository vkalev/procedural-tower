using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeGenerator))]
public class Tower : MonoBehaviour
{
    private MazeGenerator generator;

    void Start()
    {
        generator = GetComponent<MazeGenerator>();
        generator.GenerateMaze();
    }

//    [Range(2, 256)]
//     public int resolution = 10;

//     [SerializeField, HideInInspector]
//     MeshFilter[] meshFilters;
//     TowerFace[] towerFaces;
     
// 	private void OnValidate()
// 	{
//         Initialize();
//         GenerateMesh();
// 	}

// 	void Initialize()
//     {
//         Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, (Vector3.left + Vector3.forward).normalized, (Vector3.right + Vector3.forward).normalized, Vector3.back, (Vector3.left + Vector3.back).normalized, (Vector3.right + Vector3.back).normalized};
//         if (meshFilters == null || meshFilters.Length == 0)
//         {
//             meshFilters = new MeshFilter[directions.Length];
//         }
//         towerFaces = new TowerFace[directions.Length];

//         for (int i = 0; i < directions.Length; i++)
//         {
//             if (meshFilters[i] == null)
//             {
//                 GameObject meshObj = new GameObject("Mesh " + i);
//                 meshObj.transform.parent = transform;

//                 meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
//                 meshFilters[i] = meshObj.AddComponent<MeshFilter>();
//                 meshFilters[i].sharedMesh = new Mesh();
//             }

//             towerFaces[i] = new TowerFace(meshFilters[i].sharedMesh, resolution, directions[i]);
//         }
//     }

//     void GenerateMesh()
//     {
//         foreach (TowerFace face in towerFaces)
//         {
//             face.ConstructMesh();
//         }
//     }
}
