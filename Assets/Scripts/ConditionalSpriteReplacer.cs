using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalSpriteReplacer : MonoBehaviour
{
    public ConditionType conditionType;
    public enum ConditionType {Impossible,IfPaused,IfTimeAccelerated};

    public Sprite IfTrueSprite;

    Image image;
    Sprite originalSprite;
    void Start()
    {
        image = GetComponent<Image>();
        originalSprite = image.sprite;
    }

    void Update()
    {
        bool conditionMet = false;
        if(conditionType==ConditionType.IfPaused)
        {
            conditionMet = Time.timeScale == 0;
        }
        else if (conditionType == ConditionType.IfTimeAccelerated)
        {
            conditionMet = Time.timeScale > 1;
        }



        if (conditionMet)
        {
            image.sprite = IfTrueSprite;
        }
        else
        {
            image.sprite= originalSprite;
        }
    }
}
