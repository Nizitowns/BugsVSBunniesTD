using System;
using DefaultNamespace;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover instance;
    [HideInInspector]
    public GameObject PivotTargetLocation; 
    private void Start()
    {
        CurTargDist = CurDist;
        // targetTimeScale = 1;
        instance = this;
    }
    public float OrbitSpeed = 1;
    public float VerticalSpeed=1;

    public RotationType rotationType;
    public enum RotationType {PivotUpDown,WalkingOrbit,Walking }

    public float MaxPitch = 10;
    public float MinPitch = 30;

    public bool AllowZoom;
    public float ScrollSpeed;
    [Min(1)]
    public float MaxDist;
    [Min(1)]
    public float MinDist;
    [Min(1)]
    public float CurDist=1;
    float CurTargDist;


    public float AltOrbitSpeed;


    // public bool AllowTimeDilation = true;
    public float TimeSpeedUpSpeed = 4;
    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying&&AllowZoom)
        {
            Transform target = transform.GetChild(0).transform;

                Vector3 targ_dir = -target.forward;

                target.transform.localPosition = targ_dir * CurDist;
            CurDist = Mathf.Min(Mathf.Max(MinDist, CurDist), MaxDist);
        }
    }
    bool IsValid(Vector3 point)
    {

        RaycastHit hit;
        bool hasColliderBelow = Physics.Raycast(point, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("CameraWalkable"));


        return hasColliderBelow;
    }
    // float targetTimeScale = 1;
    // [Tooltip("Sets the TimeScale to value, but it if it is already that value it just resets it to 1")]
    // public void ToggleSetTimeSpeed(float value)
    // {
    //     if(targetTimeScale==value)
    //     {
    //         value = 1;
    //     }
    //     targetTimeScale = value;
    // }
    private void Update()
    {
        if (PivotTargetLocation != null)
        {
            transform.position = Vector3.Lerp(transform.position, PivotTargetLocation.transform.position, 0.1f);
        }

        Transform target = transform.GetChild(0).transform;
        // if(AllowTimeDilation)
        // {
        //     if (MenuHook.IsPaused)
        //     {
        //         Time.timeScale = 0;
        //     }
        //     else
        //     {
        //
        //
        //         if (Input.GetAxis("Accelerate") > 0)
        //         {
        //             Time.timeScale = Math.Max(targetTimeScale, TimeSpeedUpSpeed);
        //         }
        //         else
        //             Time.timeScale = targetTimeScale;
        //     }
        //
        // }

        if (rotationType == RotationType.PivotUpDown)
        {

            transform.eulerAngles += new Vector3(0, -Input.GetAxis("Horizontal")*Time.deltaTime * OrbitSpeed, 0);
            float verticalOrbit = -Input.GetAxis("Vertical") * VerticalSpeed/(Mathf.Max(1,Time.timeScale));
            target.localEulerAngles += new Vector3(verticalOrbit, 0, 0);

            target.localEulerAngles = new Vector3(Mathf.Max(Mathf.Min(MaxPitch, target.localEulerAngles.x), MinPitch), target.localEulerAngles.y, target.localEulerAngles.z);
        }
        else if(rotationType==RotationType.WalkingOrbit)
        {
            transform.eulerAngles += new Vector3(0, -Input.GetAxis("Horizontal")*(1 / (Mathf.Max(1, Time.timeScale))) * Time.deltaTime * OrbitSpeed, 0);
            Vector3 targDir = transform.GetChild(0).forward * VerticalSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            targDir.y = 0;
            if(IsValid(transform.position+targDir))
                transform.position += targDir;

            if(PivotTargetLocation!=null&&Vector3.Distance(transform.position, PivotTargetLocation.transform.position)<1)
            {
                PivotTargetLocation = null;
            }
        }
        else if (rotationType==RotationType.Walking)
        {
            target.eulerAngles += new Vector3(0, -Input.GetAxis("Orbit") * Time.unscaledDeltaTime * AltOrbitSpeed, 0);

            Vector3 targDir =target.forward * VerticalSpeed * Time.unscaledDeltaTime * InputGather.Instance.AxisNormalized.x;
            targDir += target.right * OrbitSpeed * Time.unscaledDeltaTime * InputGather.Instance.AxisNormalized.y;
            targDir.y = 0;
            if (IsValid(transform.position + targDir))
                transform.position += targDir;

            if (PivotTargetLocation != null && Vector3.Distance(transform.position, PivotTargetLocation.transform.position) < 1)
            {
                PivotTargetLocation = null;
            }
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
}
