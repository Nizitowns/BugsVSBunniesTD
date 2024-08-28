using DG.Tweening;
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
    private void OnDestroy()
    {
        shrinkTween?.Kill();
    }
    void destroyMe()
    {
        Destroy(gameObject);
    }

}
