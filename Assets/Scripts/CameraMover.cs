using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover instance;
    [HideInInspector]
    public GameObject PivotTargetLocation; 
    private void Start()
    {
        instance = this;
    }
    public float OrbitSpeed = 1;
    public float VerticalSpeed=1;

    public float MaxPitch = 10;

    public float MinPitch = 30;
    private void FixedUpdate()
    {
        if(PivotTargetLocation!=null)
        {
            transform.position = Vector3.Lerp(transform.position, PivotTargetLocation.transform.position, 0.1f);
        }

        float verticalOrbit = -Input.GetAxis("Vertical") * VerticalSpeed;
        transform.eulerAngles += new Vector3(0, -Input.GetAxis("Horizontal") * OrbitSpeed, 0);
        Transform target = transform.GetChild(0).transform;
        target.localEulerAngles += new Vector3(verticalOrbit, 0, 0);

        target.localEulerAngles = new Vector3(Mathf.Max(Mathf.Min(MaxPitch,target.localEulerAngles.x), MinPitch), target.localEulerAngles.y, target.localEulerAngles.z);
    }
}
