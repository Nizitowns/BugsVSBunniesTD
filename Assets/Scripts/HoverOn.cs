using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;


public class HoverOn : MonoBehaviour, IPointerExitHandler
{

    public GameObject TowerInfo;
 //   private float duration = 5f;
  //  private float speed = 25f;
    //private float elapsedTime;
    //float percent;
    //Vector3 startPos;
    //Vector3 endPos;
    public bool onPointerEnter=false;



    void Start()
    {
        //textColor.a = 1;
        //TowerInfo.SetActive(false);
      //  startPos = TowerInfo.transform.position;
      //  endPos = new Vector3(TowerInfo.transform.position.x, TowerInfo.transform.position.y+180, TowerInfo.transform.position.z);


    }
    //separated from main methods to handle animation
    
    void Update()
    {

        //        elapsedTime += (Time.deltaTime*speed);
        //      float percent=elapsedTime / duration;

        //Controls the transparency of our preview text
        CanvasGroup group =TowerInfo.GetComponent<CanvasGroup>();
        group.interactable = onPointerEnter;//Disable the tower blocking of an invisible hover text

        if (onPointerEnter)
            group.alpha = (1+group.alpha*19)/20f; //Fade in alpha when pointer is over
        else
            group.alpha = 0; //Snap alpha off when pointer is off

        /*if (onPointerEnter)
        {
            TowerInfo.transform.position = Vector3.Lerp(startPos, endPos, percent);
        }
        else if (!onPointerEnter)
        {
            TowerInfo.transform.position = Vector3.Lerp(endPos, startPos, percent);
        }
        */


    }

   

    public void currentActive(GameObject current) {
        //TowerInfo.SetActive(true);

        onPointerEnter = true;
        //elapsedTime = 0;
        SetIconValues Current = current.GetComponent<SetIconValues>();

        GameObject Desc = TowerInfo.transform.Find("Desc").gameObject;
        GameObject Title = TowerInfo.transform.Find("Title").gameObject;

        Desc.GetComponent<TMP_Text>().text = Current.AboutText;
        Desc.GetComponent<TMP_Text>().color = Current.TextColor;
        Title.GetComponent<TMP_Text>().text = Current.ObjectName;
        Title.GetComponent<TMP_Text>().color = Current.TextColor;


    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerEnter = false;
        //elapsedTime = 0;
        //TowerInfo.SetActive(false);


    }

}