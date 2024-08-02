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
    [Tooltip("Set if you want this gameobjects parent to be set to the root when the wave completes.")]
    public GameObject FreeOnComplete;

    public GameObject SetPivotTarget;

    [Min(0)]
    public int RewardMoneyOnComplete;

    
    void Start()
    {
        if(ListenTarget != null)
        {
            if (FadeInOnComplete != null)
                ListenTarget.WavesCompleted += FadeIn;

            if (ToggleOnComplete != null)
                ListenTarget.WavesCompleted += ToggleActive;
            if (SetPivotTarget != null)
                ListenTarget.WavesCompleted += SetPivot;
            if (FreeOnComplete != null)
                ListenTarget.WavesCompleted += FreeParent;
            if (RewardMoneyOnComplete > 0)
            {
                ListenTarget.WavesCompleted += RewardMoney;
            }
        }
    }
    public void RewardMoney()
    {
        MoneyManager.instance.AddMoney(RewardMoneyOnComplete);
    }
    public void FreeParent()
    {
        FreeOnComplete.transform.parent = null;
    }
    public void FadeIn()
    { 
        StartCoroutine(DelayFadeIn());
    }
    public void SetPivot()
    {
        CameraMover.instance.PivotTargetLocation = SetPivotTarget;
    }
    public IEnumerator DelayFadeIn()
    {
        TowerPlacer.PlacementDisabled = true;
        Vector3 pos = FadeInOnComplete.transform.position;
        float negativeOffset = -7;
        FadeInOnComplete.transform.position = pos + new Vector3(0, negativeOffset, 0);
        FadeInOnComplete.SetActive(true);
        for (float dy= negativeOffset; dy<=0;dy+=0.1f)
        {
            FadeInOnComplete.transform.position = pos + new Vector3(0, dy, 0);
            yield return new WaitForFixedUpdate();
        }
        TowerPlacer.PlacementDisabled = false;
        FadeInOnComplete.transform.position = pos;
    }

    public void ToggleActive()
    {
        ToggleOnComplete.SetActive(!ToggleOnComplete.gameObject.activeSelf);
    }
}
