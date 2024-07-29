using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public static PurchaseButton SelectedTower;

    public GameObject PreviewPlacerPrefab;

    GameObject previewPlacer;
    MeshFilter previewPlacerMeshFilter;
    void Start()
    {
        previewPlacer=Instantiate(PreviewPlacerPrefab).gameObject;
        previewPlacerMeshFilter=previewPlacer.GetComponentInChildren<MeshFilter>();
    }
    public void UpdatePreview()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            previewPlacer.transform.position = hit.point+new Vector3(0,1,0);
        }

        previewPlacerMeshFilter.transform.position = SnapToGrid(previewPlacer.transform.position);
        if (SelectedTower != null)
        {
            previewPlacerMeshFilter.mesh = SelectedTower.TowerPrefab.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    public Vector3 SnapToGrid(Vector3 curPos)
    {
        return curPos;
    }
    public void BuyTower(PurchaseButton selectedTower)
    {
        if (MoneyManager.instance.RemoveMoney(selectedTower.PurchaseCost))
        {
            Instantiate(selectedTower.TowerPrefab, SnapToGrid(previewPlacer.transform.position), Quaternion.identity);
        }
    }
    private void Update()
    {
        UpdatePreview();
        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuyTower(SelectedTower);
                Debug.Log("Placing " + SelectedTower.TowerPrefab.name);
            }
        }
    }

}
