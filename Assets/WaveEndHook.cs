using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndHook : MonoBehaviour
{
    public EnemySpawner ListenTarget;

    [Tooltip("Set if you want this gameobjects active status to be set active and for it to fade in.")]
    public GameObject FadeInOnComplete;
    [Tooltip("Set if you want this gameobjects active status to toggle when the wave completes.")]
    public GameObject ToggleOnComplete;
    void Start()
    {
        if(ListenTarget != null)
        {
            if (FadeInOnComplete != null)
                ListenTarget.WavesCompleted += FadeIn;

            if (ToggleOnComplete != null)
                ListenTarget.WavesCompleted += ToggleActive;
        }
    }

    public void FadeIn()
    { 
        StartCoroutine(DelayFadeIn());
    }
    public IEnumerator DelayFadeIn()
    {
        Vector3 pos = FadeInOnComplete.transform.position;
        float negativeOffset = -7;
        FadeInOnComplete.transform.position = pos + new Vector3(0, negativeOffset, 0);
        FadeInOnComplete.SetActive(true);
        for (float dy= negativeOffset; dy<=0;dy+=0.1f)
        {
            FadeInOnComplete.transform.position = pos + new Vector3(0, dy, 0);
            yield return new WaitForFixedUpdate();
        }
        FadeInOnComplete.transform.position = pos;
    }

    public void ToggleActive()
    {
        ToggleOnComplete.SetActive(!ToggleOnComplete.gameObject.activeSelf);
    }
}
