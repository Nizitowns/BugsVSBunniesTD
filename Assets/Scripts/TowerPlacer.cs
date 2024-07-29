using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class TowerPlacer : MonoBehaviour
{
    public static PurchaseButton SelectedTower;

    public GameObject PreviewPlacerPrefab;

    GameObject previewPlacer;
    MeshFilter previewPlacerMeshFilter;

    [SerializeField]
    private Material canPlace;
    [SerializeField]
    private Material cannotPlace;
    void Start()
    {
        previewPlacer=Instantiate(PreviewPlacerPrefab).gameObject;
        previewPlacerMeshFilter=previewPlacer.GetComponentInChildren<MeshFilter>();
    }
    public void UpdatePreview()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Tower")));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            previewPlacer.transform.position = hit.point+new Vector3(0,1,0);
            previewPlacerMeshFilter.transform.position = SnapToGrid(previewPlacer.transform.position);

        }

        if (SelectedTower != null)
        {
            if (isClearToPlace(SnapToGrid(previewPlacer.transform.position)))
            {
                previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = canPlace;
            }
            else
            {
                previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = cannotPlace;
            }
            previewPlacerMeshFilter.mesh = SelectedTower.TowerPrefab.GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            previewPlacerMeshFilter.mesh = null;
        }
    }
    public bool isClearToPlace(Vector3 curPos)
    {
        bool isClear = !IsPointOnNavMesh(curPos, 2) && !IsTowerInRange(curPos, 7f);

        RaycastHit hit;
        bool hasColliderBelow = Physics.Raycast(curPos, Vector3.down, out hit,Mathf.Infinity, LayerMask.GetMask("Placeable"));
        
        return isClear && hasColliderBelow;
    }
    bool IsTowerInRange(Vector3 point, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(point, radius, LayerMask.GetMask("Tower"));
        foreach(Collider collider in hitColliders)
        {
            if(Vector3.Distance(collider.transform.position,point)< radius)
            {
                return true;
            }
        }
        return false;
    }
    bool IsPointOnNavMesh(Vector3 point, float distance)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, distance, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    public Vector3 SnapToGrid(Vector3 curPos)
    {
        //TODO: Replace with actually good grid snapping system.
        RaycastHit hit;
        if(Physics.Raycast(curPos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Placeable")))
        {
            return hit.collider.transform.position + new Vector3(0, 5.1f, 0);
        }

        return curPos;
    }
    public void BuyTower(PurchaseButton selectedTower)
    {
        if (isClearToPlace(SnapToGrid(previewPlacer.transform.position)))
        {
            if (MoneyManager.instance.RemoveMoney(selectedTower.PurchaseCost))
            {
                Instantiate(selectedTower.TowerPrefab, SnapToGrid(previewPlacer.transform.position), Quaternion.identity);
            }
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

            if (Input.GetMouseButtonDown(1))
            {
                EventSystem.current.SetSelectedGameObject(null);
                SelectedTower = null;
            }
        }
    }

}
