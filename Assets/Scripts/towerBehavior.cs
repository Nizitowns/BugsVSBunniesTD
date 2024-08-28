using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.TowerSystem;
using UnityEngine;

public class TowerBehavior : NewTowerBase
{
    public int BurstLength = -1;

    protected override IEnumerator FireLoopCo()
    {
        var burstCounter = BurstLength;

        while (true)
        {
            while (TargetedEnemy == null)
            {
                yield return new WaitForSeconds(0.25f);
                Debug.Log("Waiting For Enemy");
                SetNewTarget();
            }
            
            RotateToTarget();
            OnFire();
            
            yield return new WaitForSeconds(Config.fireRate);

            //
            // var abortShot = false;
            // if (BurstLength >= 0)
            // {
            //     burstCounter++;
            //
            //     if (burstCounter > BurstLength)
            //     {
            //         burstCounter = 0;
            //         yield return new WaitForSeconds(Config.burstDelay);
            //         abortShot = true;
            //     }
            // }
            //
            // if (!abortShot)
            // {
            //     RotateToTarget();
            //     OnFire();
            //     
            //     yield return new WaitForSeconds(Config.fireRate);
            // }
            // else
            // {
            //     burstCounter = BurstLength;
            // }

            yield return new WaitForEndOfFrame();
        }
    }

    private void printList(List<int> num)
    {
        var numbs = "{ ";
        for (var i = 0; i < num.Count; i++) numbs = numbs + num[i] + ", ";
        numbs += " }";
        Debug.Log(numbs);
    }
}