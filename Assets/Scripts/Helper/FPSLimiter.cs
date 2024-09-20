using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Script to limit framerate
*   Create empty object in level and add script to limit framerate
*/
public class FPSLimiter : MonoBehaviour
{
    // -1 set by default means no limiter
    public int targetFPS = -1;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
