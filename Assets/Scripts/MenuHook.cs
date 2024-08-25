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
    public static bool IsPaused;
    private void OnDisable()
    {
        if(PauseIfEnabled)
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
    private void OnEnable()
    {
        if(PauseIfEnabled)
        {
            IsPaused = true;
            Time.timeScale = 0f;
        }
    }
    private void OnDestroy()
    {
        if (PauseIfEnabled)
        {
            IsPaused = false;
            Time.timeScale = 1;
        }
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
