using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public CanvasGroup CurrentActive;
    int targetSceneID=-1;

    public float fadeDuration = 0.25f;



    void Start()
    {
        DisableAll();
    }
    private void DisableAll()
    {
        foreach (Transform c in transform)
        {
            CanvasGroup canvasGroup = c.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                if (CurrentActive != canvasGroup)
                {
                    FullDisable(canvasGroup);
                }
            }
        }
    }
    private void FullDisable(CanvasGroup group)
    {
        group.alpha = 0;
        group.gameObject.SetActive(false);
    }

    Tween fadeTween;
    public void TweenEnable(CanvasGroup group)
    {
        fadeTween?.Kill();
        fadeTween = CurrentActive.DOFade(0,fadeDuration);
        fadeTween.onComplete += FadeIn;
        fadeTween.Play();


        CurrentActive = group;
    }
    public void FadeOutAndLoad(int sceneID)
    {
        fadeTween?.Kill();
        fadeTween = CurrentActive.DOFade(0, fadeDuration);
        fadeTween.onComplete += LoadScene;
        fadeTween.Play();
        targetSceneID = sceneID;
    }

    public void FadeOutAndQuit()
    {
        fadeTween?.Kill();
        fadeTween = CurrentActive.DOFade(0, fadeDuration);
        fadeTween.onComplete += Application.Quit;
        fadeTween.Play();
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(targetSceneID);
    }

    private void FadeIn()
    {
        DisableAll();
        CurrentActive.gameObject.SetActive(true);
        fadeTween?.Kill();
        fadeTween = CurrentActive.DOFade(1, fadeDuration);
        fadeTween.Play();
    }
    private void OnDestroy()
    {
        fadeTween?.Kill();
    }
}
