using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    public static bool PlacementDisabled;
    public static PurchaseButton SelectedTower;

    public GameObject PreviewPlacerPrefab;

    AudioSource source;
    GameObject previewPlacer;
    MeshFilter previewPlacerMeshFilter;

    public static TowerPlacementGrid TowerPlacementGrid;
    private eCurrentMode _currentMode = eCurrentMode.None;
    
    [SerializeField] private Material canPlace;
    [SerializeField] private Material cannotPlace;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
        PlacementDisabled = false;
        previewPlacer = Instantiate(PreviewPlacerPrefab).gameObject;
        previewPlacerMeshFilter = previewPlacer.GetComponentInChildren<MeshFilter>();
    }
    
    private void Update()
    {
        UpdatePreview();
        var hitClass1 = InputGather.Instance.GetHitClass<TowerPlacementGrid>();

        switch (_currentMode)
        {
            case eCurrentMode.None:
                if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject) // Selecting
                    if (hitClass1 != null && hitClass1.HasTowerOnIt)
                    {
                        SelectedTower = null;
                        TowerPlacementGrid = hitClass1;
                        _currentMode = eCurrentMode.Selection;
                    }

                if (SelectedTower != null)
                {
                    TowerPlacementGrid = null;
                    _currentMode = eCurrentMode.Painting;
                }
                
                break;
            case eCurrentMode.Selection:
                if (SelectedTower != null) _currentMode = eCurrentMode.Painting;
                
                if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject) // Selecting
                {
                    if (hitClass1 != null)
                    {
                        if (hitClass1 == TowerPlacementGrid)
                        {
                            TowerPlacementGrid = null;
                        }
                        else if (hitClass1.HasTowerOnIt)
                        {
                            TowerPlacementGrid = hitClass1;
                        }
                        else
                        {
                            TowerPlacementGrid = null;
                            _currentMode = eCurrentMode.None;
                        }
                    }
                    else
                    {
                        TowerPlacementGrid = null;
                        _currentMode = eCurrentMode.None;
                    }
                }
                break;
            case eCurrentMode.Painting:
                if (SelectedTower == null) _currentMode = eCurrentMode.None;
                
                if (hitClass1 != null && 
                    InputGather.Instance.MouseLeftClick &&
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    if(CanPlacable(out TowerPlacementGrid grid))
                    {
                        BuyTower(SelectedTower);
                    }
                    else if(hitClass1.HasTowerOnIt)
                    {
                        TowerPlacementGrid = hitClass1;
                        _currentMode = eCurrentMode.Selection;
                    }
                }
                break;
        }
          
        if (InputGather.Instance.CancelButton)
        {
            TowerPlacementGrid = null;
            SelectedTower = null;
            _currentMode = eCurrentMode.None;
        }
        
        // Zombie Code ***
        // var hitClass = InputGather.Instance.GetHitClass<TowerPlacementGrid>();
        // if (SelectedTower != null) // Placing
        // {
        //     if (hitClass != null && 
        //         InputGather.Instance.MouseLeftClick &&
        //         !EventSystem.current.IsPointerOverGameObject() &&
        //         CanPlacable(out DefaultNamespace.TowerPlacementGrid grid))
        //     {
        //         BuyTower(SelectedTower);
        //     }
        // }
        // else if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject) // Selecting
        // {
        //     if (hitClass != null)
        //     {
        //         if (hitClass == TowerPlacementGrid)
        //             TowerPlacementGrid = null;
        //         else if (hitClass.HasTowerOnIt)
        //             TowerPlacementGrid = hitClass;
        //         else
        //             TowerPlacementGrid = null;
        //     }
        //     else
        //         TowerPlacementGrid = null;
        // }
      
    }
    
    public void UpdatePreview()
    {
        // Color And Mesh Stuff
        if (SelectedTower == null || PlacementDisabled)
        {
            previewPlacerMeshFilter.mesh = null;
            return;
        }
        else
        {
            if (SelectedTower.TowerScriptable.prefab.GetComponent<MeshFilter>() != null)
            {
                previewPlacerMeshFilter.mesh = SelectedTower.TowerScriptable.prefab.GetComponent<MeshFilter>().sharedMesh;
            }
            else
            {
                previewPlacerMeshFilter.mesh = SelectedTower.TowerScriptable.previewMesh;
            }
        }
        
        if (CanPlacable(out TowerPlacementGrid grid))
            previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = canPlace;
        else
            previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = cannotPlace;
        
        // Placement
        var hitClass = InputGather.Instance.GetHitClass<TowerPlacementGrid>();

        if (hitClass != null)
            previewPlacerMeshFilter.transform.position = hitClass.GetPlacementPosition.position;
        else
            previewPlacerMeshFilter.transform.position = InputGather.Instance.GetMousePosition();
    }
    
    public void BuyTower(PurchaseButton selectedTower)
    {
        if (CanPlacable(out TowerPlacementGrid grid))
        {
            if (MoneyManager.instance.RemoveMoney(selectedTower.TowerScriptable.purchaseCost))
            {
                source?.Play();
                grid.AddTower(selectedTower.TowerScriptable);
            }
        }
    }

    public bool CanPlacable(out TowerPlacementGrid placementGrid)
    {
        placementGrid = InputGather.Instance.GetHitClass<TowerPlacementGrid>();

        if (placementGrid == null) return false;

        return !placementGrid.HasTowerOnIt;
    }

    public enum eCurrentMode
    {
        None,
        Selection,
        Painting,
    }
}
