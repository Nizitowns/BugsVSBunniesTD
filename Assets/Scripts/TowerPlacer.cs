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
        var hitClass = InputGather.Instance.GetHitClass<TowerPlacementGrid>();
        if (SelectedTower != null)
        {
            if (hitClass != null && 
                InputGather.Instance.MouseLeftClick &&
                !EventSystem.current.IsPointerOverGameObject() &&
                CanPlacable(out DefaultNamespace.TowerPlacementGrid grid))
            {
                BuyTower(SelectedTower);
            }
        }
        else if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject)
        {
            if (hitClass != null)
            {
                if (hitClass == TowerPlacementGrid)
                    TowerPlacementGrid = null;
                else if (hitClass.HasTowerOnIt)
                    TowerPlacementGrid = hitClass;
                else
                    TowerPlacementGrid = null;
            }
            else
                TowerPlacementGrid = null;
        }
        
        if (InputGather.Instance.CancelButton)
        {
            TowerPlacementGrid = null;
            SelectedTower = null;
        }
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
}
