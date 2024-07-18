using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCenter : MonoBehaviour
{
    public static BaseCenter Instance;
    private void Start()
    {
        Instance = this;
    }
}
