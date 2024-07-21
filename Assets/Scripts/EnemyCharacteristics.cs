using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacteristics : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        gameObject.tag = "enemies";
        foreach (Transform t in this.transform)
        {
            t.gameObject.tag = "enemies";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
