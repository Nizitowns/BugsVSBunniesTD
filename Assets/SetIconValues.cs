using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetIconValues : MonoBehaviour, IPointerEnterHandler
{
    public string ObjectName;
    public string AboutText;
    public Color TextColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //enter and exit are controlled here because otherwise
    //this was always returning false from all 4 iconns having this functionality
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponentInParent<HoverOn>().currentActive(this.gameObject);
        //this.GetComponentInParent<HoverOn>().onPointerEnter = true;
    }





}
