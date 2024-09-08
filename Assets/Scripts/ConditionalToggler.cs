using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionalToggler : MonoBehaviour
{
    public enum ConditionType { AlwaysTrue,WhenSceneIdMatches};
    public ConditionType ToggleCondition;

    public List<int> AllowedValues= new List<int>();

    private void Start()
    {
        if(ToggleCondition==ConditionType.WhenSceneIdMatches)
        {
            if(!AllowedValues.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
