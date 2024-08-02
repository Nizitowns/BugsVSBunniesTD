using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkEffect : MonoBehaviour
{
    public float Duration=1;
    private Tween shrinkTween;
    private void Start()
    {
        shrinkTween?.Kill();
        shrinkTween = transform.DOScale(Vector3.zero, Duration);
        shrinkTween.onComplete += destroyMe;
        shrinkTween.Play();
    }
    void destroyMe()
    {
        Destroy(gameObject);
    }

}
