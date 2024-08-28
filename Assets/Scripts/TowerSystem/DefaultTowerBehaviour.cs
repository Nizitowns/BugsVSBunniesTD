using System.Collections.Generic;
using DefaultNamespace.TowerSystem;
using UnityEngine;

public class DefaultTowerBehaviour : NewTowerBase
{
    private void printList(List<int> num)
    {
        var numbs = "{ ";
        for (var i = 0; i < num.Count; i++) numbs = numbs + num[i] + ", ";
        numbs += " }";
        Debug.Log(numbs);
    }
}