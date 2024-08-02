using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotater : MonoBehaviour
{
    public float Speed;
    float dy;
    void Update()
    {
        dy += Speed * Time.fixedDeltaTime*Time.timeScale;
        transform.localEulerAngles= new Vector3(dy,0,0);
    }
}
