using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverOn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string aboutText;
    public GameObject textDisplay;
    void Start()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(name+"\t" + aboutText);
        //GameObject textDisplayClone = Instantiate(textDisplay, new Vector3(0, 5, 0), Quaternion.identity);
        //textDisplayClone.GetComponentInChildren<TMP_Text>().text = aboutText;


        //<TMP_Text>().text=aboutText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit"+ name);
    }
    
}