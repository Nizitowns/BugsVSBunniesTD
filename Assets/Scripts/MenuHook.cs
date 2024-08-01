using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHook : MonoBehaviour
{
    public KeyCode ActivationKey=KeyCode.Escape;

    public CanvasGroup ActivateOnPress;
    

    public MenuManager MenuManager;
    public FadeType myFadeType;
    public enum FadeType {TweenEnable,TweenEnableImmediate };

    public bool PauseIfEnabled;
    private void OnDisable()
    {
        if(PauseIfEnabled)
        {
            Time.timeScale = 1;
        }
    }
    private void OnEnable()
    {
     if(PauseIfEnabled)
        {

            Time.timeScale = 0;
        }
    }
    private void OnDestroy()
    {
        if(PauseIfEnabled)
            Time.timeScale = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown(ActivationKey))
        {
            if (myFadeType == FadeType.TweenEnable)
            {
                MenuManager?.TweenEnable(ActivateOnPress);
            }
            else
            {
                MenuManager?.EnableImmediate(ActivateOnPress);

            }
        }
    }
}
