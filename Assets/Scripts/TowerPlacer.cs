using System.Collections.Generic;
using System.Linq;
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
    
    private List<TowerPlacementGrid> _towerPlacementGrids;
    private TowerPlacementGrid _currentTowerPlacementGrid;
    [SerializeField] private Material canPlace;
    [SerializeField] private Material cannotPlace;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
        PlacementDisabled = false;
        previewPlacer = Instantiate(PreviewPlacerPrefab).gameObject;
        previewPlacerMeshFilter = previewPlacer.GetComponentInChildren<MeshFilter>();
        UpdateTowerPlacementList();
    }

    private void OnEnable()
    {
        WaveEndHook.OnWaveEnded += UpdateTowerPlacementList;
    }

    private void OnDisable()
    {
        WaveEndHook.OnWaveEnded -= UpdateTowerPlacementList;
    }

    private void Update()
    {
        var hitClass = InputGather.Instance.GetHitClass<TowerPlacementGrid>();

        UpdatePreview();

        switch (_currentMode)
        {
            case eCurrentMode.None:
                if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject) // Selecting
                    if (hitClass != null && hitClass.HasTowerOnIt)
                    {
                        SelectedTower = null;
                        TowerPlacementGrid = hitClass;
                        _currentMode = eCurrentMode.Selection;
                    }

                if (SelectedTower != null)
                {
                    TowerPlacementGrid = null;
                    previewPlacerMeshFilter.transform.position = InputGather.Instance.GetMousePosition();
                    _currentMode = eCurrentMode.Painting;
                }
                
                break;
            case eCurrentMode.Selection:
                if (SelectedTower != null) _currentMode = eCurrentMode.Painting;
                
                if(InputGather.Instance.MouseLeftClick && !InputGather.isMouseOverGameObject) // Selecting
                {
                    hitClass = InputGather.Instance.GetHitClass<TowerPlacementGrid>();
                    if (hitClass != null)
                    {
                        if (hitClass == TowerPlacementGrid)
                        {
                            TowerPlacementGrid = null;
                        }
                        else if (hitClass.HasTowerOnIt)
                        {
                            TowerPlacementGrid = hitClass;
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
                
                _currentTowerPlacementGrid = FindClosest(InputGather.Instance.GetMousePosition());

                if (_currentTowerPlacementGrid != null && 
                    InputGather.Instance.MouseLeftClick &&
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    if(_currentTowerPlacementGrid != null)
                    {
                        BuyTower(SelectedTower);
                    }
                    else if (_currentTowerPlacementGrid == TowerPlacementGrid)
                    {
                        TowerPlacementGrid = null;
                    }
                    else if(_currentTowerPlacementGrid.HasTowerOnIt)
                    {
                        TowerPlacementGrid = _currentTowerPlacementGrid;
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
        
        if (CanPlacable())
            previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = canPlace;
        else
            previewPlacerMeshFilter.GetComponent<MeshRenderer>().material = cannotPlace;
        
        // Placement
        _currentTowerPlacementGrid = FindClosest(InputGather.Instance.GetMousePosition());
        if (_currentTowerPlacementGrid != null)
        {
            previewPlacerMeshFilter.transform.position = Vector3.Lerp(previewPlacerMeshFilter.transform.position, _currentTowerPlacementGrid.GetPlacementPosition.position, Time.unscaledDeltaTime * 10);
        }
    }
    
    public void BuyTower(PurchaseButton selectedTower)
    {
        if (CanPlacable())
        {
            if (MoneyManager.instance.RemoveMoney(selectedTower.TowerScriptable.purchaseCost))
            {
                source?.Play();
                _currentTowerPlacementGrid.AddTower(selectedTower.TowerScriptable);
            }
        }
    }

    public bool CanPlacable()
    {
        return _currentTowerPlacementGrid != null && !_currentTowerPlacementGrid.HasTowerOnIt;
    }
    
    private void UpdateTowerPlacementList()
    {
        _towerPlacementGrids = FindObjectsOfType<TowerPlacementGrid>().ToList();
        
        if (_towerPlacementGrids.Count > 0)
            _currentTowerPlacementGrid = _towerPlacementGrids[0];
        else
            Debug.LogError("There is No Placement Grids Found");
    }
    
    private TowerPlacementGrid FindClosest(Vector3 mousePosition)
    {
        TowerPlacementGrid closestTransform = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var grid in _towerPlacementGrids)
        {
            float distanceSqr = (grid.GetPlacementPosition.position - mousePosition).sqrMagnitude; // Squared distance is faster

            if (distanceSqr < closestDistanceSqr)
            {
                closestTransform = grid;
                closestDistanceSqr = distanceSqr;
            }
        }

        return closestTransform;
    }

    public enum eCurrentMode
    {
        None,
        Selection,
        Painting,
    }
}
