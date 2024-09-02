using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotater : MonoBehaviour
{
    public float Speed;
    float dy;

    public ParticleSystem OnlyRotateIfPlaying;
    public enum RotateType {DYOnly,RandomizeTotal };
    public RotateType myType=RotateType.DYOnly;
    void Update()
    {
        if (myType == RotateType.DYOnly)
        {
            if (OnlyRotateIfPlaying == null || OnlyRotateIfPlaying.isPlaying)
            {
                dy += Speed * Time.fixedDeltaTime * Time.timeScale;
                transform.localEulerAngles = new Vector3(dy, 0, 0);
            }
        }
        else if(myType== RotateType.RandomizeTotal) {
            GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1.0f)*Speed, Random.Range(-1, 1.0f) * Speed, Random.Range(-1, 1.0f) * Speed));

            transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x,40,120), transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}
