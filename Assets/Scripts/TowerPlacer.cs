using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public static PurchaseButton SelectedTower;


    private void Update()
    {
        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Placing " + SelectedTower.TowerPrefab.name);
            }
        }
    }

}
