using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Min(1)]
    public int LevelID=1;
    Button button;
    float scale;
    float scale_mod;
    private void Start()
    {
        scale=transform.localScale.x;
        scale_mod = 0;
        button = GetComponent<Button>();
        if(LevelID==1 )
        {
            PlayerPrefs.SetInt("LevelCleared_0", 1);
        }
    }
    bool mouseOver;
    public void OnPointerEnter(PointerEventData data)
    {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData data)
    {
        mouseOver = false;
    }
    void Update()
    {
        button.interactable = PlayerPrefs.GetInt("LevelCleared_" + (LevelID-1), 0)==1;
        
        if(mouseOver&&button.interactable)
        {
            scale_mod =(Mathf.Sin(2.5f*Time.timeSinceLevelLoad)+1)/66f;
        }
        else
        {
            scale_mod = 0;
        }
        transform.localScale = new Vector3(scale+ scale_mod, scale+ scale_mod, scale+ scale_mod);
    }
}
