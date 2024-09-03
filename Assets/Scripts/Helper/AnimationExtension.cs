using System.Collections.Generic;
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
            Tween tween = gameObject.transform.DOMove(placedPosition, 1f).SetEase(Ease.OutExpo).SetLink(gameObject);
            return tween;
        }

        /// <summary>
        ///  Destroys the GameObject On Complete
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Tween AnimatedRemove(this GameObject gameObject)
        {
            Vector3 endPos = gameObject.transform.position + Vector3.up * 1f;
            Tween routine = gameObject.transform.DOMove(endPos, 0.1f).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() => MonoBehaviour.Destroy(gameObject));
            return routine;
        }
    }
}