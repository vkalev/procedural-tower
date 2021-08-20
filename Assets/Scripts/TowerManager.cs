using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Tower towerPrefab;
    private Tower tower;

    void Start()
    {
        BuildTower();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
			RebuildTower();
		}
    }

    private void BuildTower()
    {
        tower = Instantiate(towerPrefab) as Tower;
    }

    private void RebuildTower()
    {
        // StopAllCoroutines();
        tower.DestroyFloors();
        Destroy(tower.gameObject);
        BuildTower();
    }
}
