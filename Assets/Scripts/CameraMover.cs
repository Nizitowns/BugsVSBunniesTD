using DefaultNamespace;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover instance;
    [HideInInspector] public GameObject PivotTargetLocation;

    public bool AllowZoom;
    
    public float OrbitSpeed = 1;
    public float VerticalSpeed = 1;
    public float ScrollSpeed;
    public float AltOrbitSpeed;
    [Min(1)] public float MaxDist;
    [Min(1)] public float MinDist;
    [Min(1)] public float CurDist = 1;
    
    private float CurTargDist;
    
    private void Start()
    {
        CurTargDist = CurDist;
        instance = this;
    }

    private void Update()
    {
        if (PivotTargetLocation != null)
        {
            transform.position = Vector3.Lerp(transform.position, PivotTargetLocation.transform.position, 0.1f);
        }

        Transform target = transform.GetChild(0).transform;

        target.eulerAngles += new Vector3(0, -InputGather.Instance.Orbit * Time.unscaledDeltaTime * AltOrbitSpeed, 0);
        Vector3 targDir = target.forward * VerticalSpeed * Time.unscaledDeltaTime * InputGather.Instance.AxisNormalized.x;
        targDir += target.right * OrbitSpeed * Time.unscaledDeltaTime * InputGather.Instance.AxisNormalized.y;
        targDir.y = 0;
        if (IsValid(transform.position + targDir))
            transform.position += targDir;

        if (PivotTargetLocation != null &&
            Vector3.Distance(transform.position, PivotTargetLocation.transform.position) < 1)
        {
            PivotTargetLocation = null;
        }

        if (AllowZoom)
        {
            Vector3 targ_dir = -target.forward;

            CurTargDist += -InputGather.Instance.ScroolWheel * Time.unscaledDeltaTime * ScrollSpeed;
            CurTargDist = Mathf.Min(Mathf.Max(MinDist, CurTargDist), MaxDist);

            CurDist = (CurDist * 4 + CurTargDist) / 5f;
            target.transform.localPosition = targ_dir * CurDist;
        }
    }
    
    private bool IsValid(Vector3 point)
    {
        RaycastHit hit;
        bool hasColliderBelow = Physics.Raycast(point, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("CameraWalkable"));

        return hasColliderBelow;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && AllowZoom)
        {
            Transform target = transform.GetChild(0).transform;

            Vector3 targ_dir = -target.forward;

            target.transform.localPosition = targ_dir * CurDist;
            CurDist = Mathf.Min(Mathf.Max(MinDist, CurDist), MaxDist);
        }
    }
}