using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.TowerSystem
{
    public interface ITower
    {
        
    }
    
    public abstract class NewTowerBase : MonoBehaviour, ITower
    {
        public TowerScriptableObject Config { get; set; }

        protected List<IEnemy> TargetList = new List<IEnemy>();
        protected IEnemy TargetedEnemy;
        protected IEnemy LastTargeted;

        private SphereCollider _myCollider;
        [SerializeField] protected GameObject RotateAxis;
        [SerializeField] protected Transform BulletSource;

        private ParticleSystem _particleSystem;

        protected virtual void Start()
        {
            _myCollider = GetComponent<SphereCollider>();
            _myCollider.radius = Config.attackRadius;

            _particleSystem = Instantiate(Config.particulOnShoot, BulletSource);

            StartCoroutine(FireLoopCo());
        }

        protected virtual void OnFire()
        {
            if (TargetedEnemy == null) return;
            
            var spawnPos = BulletSource.position + new Vector3(0, 1, 0);
            var bullet = Instantiate(Config.bulletPrefab, spawnPos, transform.rotation);
            bullet.GetComponent<bulletBehavior>().enemy = TargetedEnemy.mTransform.gameObject;
            
            var randomClip = Config.firingSfx[Random.Range(0, Config.firingSfx.Count)];
            SoundFXPlayer.Instance.PlaySFX(randomClip);
            _particleSystem.Play();
        }

        protected virtual void StopFire()
        {
            _particleSystem.Stop();
        }

        protected virtual IEnumerator FireLoopCo()
        {
            while (true)
            {
                SetNewTarget();

                if (TargetedEnemy != null)
                {
                    RotateToTarget();
                    OnFire();
                }
            
                yield return new WaitForSeconds(Config.fireRate);

                yield return new WaitForEndOfFrame();
            }
        }

        protected void RotateToTarget()
        {
            if (TargetedEnemy == null) return;
            
            var direction = TargetedEnemy.mTransform.position - transform.position;
            direction.y = 0;
            var rotation = Quaternion.LookRotation(direction);
            RotateAxis.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0);
        }

        protected virtual void SetNewTarget()
        {
            if (TargetList.Count == 0)
            {
                TargetedEnemy = null;
                return;
            }

            switch (Config.myTargetingType)
            {
                case TargetType.RandomSelect:
                    TargetedEnemy = TargetList[Random.Range(0, TargetList.Count)];
                    break;
                case TargetType.FocusOnTarget:
                    TargetedEnemy = TargetList[0];
                    break;
            }

            if (TargetedEnemy.isDead)
            {
                TargetList.Remove(TargetedEnemy);
                SetNewTarget();
            }
               
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEnemy enemey))
            {
                TargetList.Add(enemey);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out IEnemy enemey))
                TargetList.Remove(enemey);
        }
    }
}