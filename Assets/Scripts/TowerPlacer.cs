using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DefaultNamespace.TowerSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Member;
public class TowerPlacer : MonoBehaviour
{
    public static bool PlacementDisabled;
    public static PurchaseButton SelectedTower;

    public GameObject PreviewPlacerPrefab;

    AudioSource source;
    GameObject previewPlacer;
    MeshFilter previewPlacerMeshFilter;

    public static TowerBehavior SelectedPlacedTower;

    [SerializeField]
    private Material canPlace;
    [SerializeField]
    private Material cannotPlace;
    void Start()
    {
        source = GetComponent<AudioSource>();
        PlacementDisabled = false;
        previewPlacer =Instantiate(PreviewPlacerPrefab).gameObject;
        previewPlacerMeshFilter=previewPlacer.GetComponentInChildren<MeshFilter>();
    }
    public void UpdatePreview()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        if(EventSystem.current.IsPointerOverGameObject())
        {//Dont let you place if you are over a UI element right now
            mouseScreenPosition = new Vector3(0, -9999, 0);
        }
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Tower")));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            previewPlacer.transform.position = hit.point+new Vector3(0,1,0);
            previewPlacerMeshFilter.transform.position = SnapToGrid(previewPlacer.transform.position);

        }



        if (SelectedTower != null&&!PlacementDisabled)
        {
            if (isClearToPlace(SnapToGrid(previewPlacer.transform.position)))
            {
                previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = canPlace;
            }
            else
            {
                previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = cannotPlace;
            }
            if (SelectedTower.TowerScriptable.prefab.GetComponent<MeshFilter>() != null)
            {
                previewPlacerMeshFilter.mesh = SelectedTower.TowerScriptable.prefab.GetComponent<MeshFilter>().sharedMesh;
            }
            else
            {
                previewPlacerMeshFilter.mesh = SelectedTower.TowerScriptable.previewMesh;
            }
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
        
        return isClear && hasColliderBelow&&!PlacementDisabled;
    }
    void SelectTowerInRange(Vector3 point, float radius)
    {
        SelectedPlacedTower = null;
        Collider[] hitColliders = Physics.OverlapSphere(point, radius, LayerMask.GetMask("Tower"));
        foreach (Collider collider in hitColliders)
        {
            if (Vector3.Distance(collider.transform.position, point) < radius)
            {
                if (collider.gameObject.GetComponent<TowerBehavior>() != null)
                {
                    SelectedPlacedTower = collider.gameObject.GetComponent<TowerBehavior>();
                }
                return;
            }
        }

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
            if (MoneyManager.instance.RemoveMoney(selectedTower.TowerScriptable.purchaseCost))
            {
                source?.Play();
                var go = Instantiate(selectedTower.TowerScriptable.prefab, SnapToGrid(previewPlacer.transform.position), Quaternion.identity);
                if (go.TryGetComponent(out NewTowerBase towerBase))
                {
                    towerBase.Config = SelectedTower.TowerScriptable;
                }
            }
        }
    }
    private void Update()
    {



        UpdatePreview();



        if (previewPlacer != null&& Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
            SelectTowerInRange(previewPlacer.transform.position, 3.5f);

        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuyTower(SelectedTower);
              //  Debug.Log("Placing " + SelectedTower.TowerPrefab.name);
            }

            if (Input.GetMouseButtonDown(1))
            {
                EventSystem.current.SetSelectedGameObject(null);
                SelectedTower = null;
            }
        }
    }

}
