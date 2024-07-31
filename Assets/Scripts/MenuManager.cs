using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
public class MenuManager : MonoBehaviour
{
    public CanvasGroup CurrentActive;
    int targetSceneID=-1;

    public float fadeDuration = 0.25f;

    CanvasGroup lastActive;
    public Volume volume;


    void Start()
    {
        if(volume != null)
            targetWeight = volume.weight;
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

        lastActive = CurrentActive;
        CurrentActive = group;
    }
    public void FadeVinnete(float amount)
    {

        targetWeight = amount;
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
    float targetWeight;
    private void FadeIn()
    {
        DisableAll();
        lastActive.gameObject.SetActive(false);
        CurrentActive.gameObject.SetActive(true);
        fadeTween?.Kill();
        fadeTween = CurrentActive.DOFade(1, fadeDuration);
        fadeTween.Play();
    }
    private void OnDestroy()
    {
        fadeTween?.Kill();
    }

    private void Update()
    {
        if(volume!=null)
        {
            volume.weight= (volume.weight*20+ targetWeight)/21f;
        }
    }
}
