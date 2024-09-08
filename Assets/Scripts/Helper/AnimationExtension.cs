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
        /// <summary>
        /// Sets the Tween Use deafult Ease and FrameIndepent Update
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="isInAnimation"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static Tween DefaultUISettings(this Tween tween, bool isInAnimation = true, Ease? ease = null)
        {
            tween.SetUpdate(UpdateType.Normal, true);
            tween.SetEase(ease ?? (isInAnimation ? Ease.InSine : Ease.OutSine));
            return tween;
        }
    }

    public static class Vector3Extension
    {
        public static Vector3 RandomDirection(this Vector3 vector)
        {
            var x = Random.Range(-1, 1);
            var y = Random.Range(-1, 1);
            var z = Random.Range(-1, 1);

            return new Vector3(x, y, z).normalized;
        }
    }
}