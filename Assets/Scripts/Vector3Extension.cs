using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Vector3Extension
    {
        public static void AnimatedPlacement(this GameObject gameObject)
        {
            Vector3 placedPosition = gameObject.transform.position;
            gameObject.transform.position -= Vector3.down * 2;
            gameObject.transform.DOMove(placedPosition, 1f).SetEase(Ease.OutExpo);
        }

        public static void AnimatedRemove(this GameObject gameObject)
        {
            Vector3 endPos = gameObject.transform.position + Vector3.down * 10;
            gameObject.transform.DOMove(endPos, 1f).SetEase(Ease.OutExpo);
        }
    }
}