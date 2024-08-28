using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.TowerSystem;
using UnityEngine;

public class DefaultTowerBehaviour : NewTowerBase
{
    protected override IEnumerator FireLoopCo()
    {
        while (true)
        {
            SetNewTarget();

            if (TargetedEnemy != null)
            {
                RotateToTarget();
                OnFire();
            }
            
            yield return new WaitForSeconds(Config.fireRate);

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