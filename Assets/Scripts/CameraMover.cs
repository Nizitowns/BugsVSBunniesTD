using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraMover : MonoBehaviour
{
    public static CameraMover instance;
    [HideInInspector]
    public GameObject PivotTargetLocation; 
    private void Start()
    {
        CurTargDist = CurDist;
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
    private void FixedUpdate()
    {
        if (PivotTargetLocation != null)
        {
            transform.position = Vector3.Lerp(transform.position, PivotTargetLocation.transform.position, 0.1f);
        }

        Transform target = transform.GetChild(0).transform;


        if (rotationType == RotationType.PivotUpDown)
        {

            transform.eulerAngles += new Vector3(0, -Input.GetAxis("Horizontal") * OrbitSpeed, 0);
            float verticalOrbit = -Input.GetAxis("Vertical") * VerticalSpeed;
            target.localEulerAngles += new Vector3(verticalOrbit, 0, 0);

            target.localEulerAngles = new Vector3(Mathf.Max(Mathf.Min(MaxPitch, target.localEulerAngles.x), MinPitch), target.localEulerAngles.y, target.localEulerAngles.z);
        }
        else if(rotationType==RotationType.WalkingOrbit)
        {
            transform.eulerAngles += new Vector3(0, -Input.GetAxis("Horizontal") * OrbitSpeed, 0);
            Vector3 targDir = transform.GetChild(0).forward * VerticalSpeed * Input.GetAxis("Vertical");
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

            target.eulerAngles += new Vector3(0, -Input.GetAxis("Orbit") * AltOrbitSpeed, 0);

            Vector3 targDir =target.forward * VerticalSpeed * Input.GetAxis("Vertical");
            targDir += target.right * OrbitSpeed * Input.GetAxis("Horizontal");
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

            CurTargDist += -Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;
            CurTargDist = Mathf.Min(Mathf.Max(MinDist, CurTargDist), MaxDist);

            CurDist = (CurDist * 4 + CurTargDist) / 5f;
            target.transform.localPosition = targ_dir * CurDist;
        }

    }
}
