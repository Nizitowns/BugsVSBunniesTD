using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public static class AnimationExtension
    {
        public static Tween AnimatedPlacement(this GameObject gameObject)
        {
            Vector3 placedPosition = gameObject.transform.position;
            gameObject.transform.position -= Vector3.down * 2;
            Tween routine = gameObject.transform.DOMove(placedPosition, 1f).SetEase(Ease.OutExpo);
            return routine;
        }

        public static Tween AnimatedRemove(this GameObject gameObject)
        {
            Vector3 endPos = gameObject.transform.position + Vector3.up * 1f;
            Tween routine = gameObject.transform.DOMove(endPos, 0.09f).SetEase(Ease.Linear);
            return routine;
        }
    }
}