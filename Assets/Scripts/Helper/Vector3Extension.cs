using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public static class AnimationExtension
    {
        public static void AnimatedPlacement(this GameObject gameObject)
        {
            Vector3 placedPosition = gameObject.transform.position;
            gameObject.transform.position -= Vector3.down * 2;
            gameObject.transform.DOMove(placedPosition, 1f).SetEase(Ease.OutExpo);
        }

        public static void AnimatedRemove(this GameObject gameObject)
        {
            Vector3 endPos = gameObject.transform.position + Vector3.up * 1f;
            gameObject.transform.DOMove(endPos, 0.09f).SetEase(Ease.Linear);
        }
    }
}