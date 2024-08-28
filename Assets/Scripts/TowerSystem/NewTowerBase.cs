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
            // TODO Add AudioManager
            // if (audioSource != null) initial_volume = audioSource.volume;
        }

        protected virtual void OnFire()
        {
            var spawnPos = BulletSource.position + new Vector3(0, 1, 0);
            var bullet = Instantiate(Config.bulletPrefab, spawnPos, transform.rotation);
            bullet.GetComponent<bulletBehavior>().enemy = TargetedEnemy;
            
            var randomClip = Config.firingSFX[Random.Range(0, Config.firingSFX.Count)];
            SoundFXPlayer.Instance.PlaySFX(randomClip);
            _particleSystem.Play();
        }

        protected virtual IEnumerator FireLoopCo()
        {
            yield return null;
        }

        protected void RotateToTarget()
        {
            if (TargetedEnemy == null) return;
            
            var direction = TargetedEnemy.GetTransform().position - transform.position;
            direction.y = 0;
            var rotation = Quaternion.LookRotation(direction);
            RotateAxis.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEnemy enemey))
            {
                TargetList.Add(enemey);
                TargetedEnemy = enemey;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out IEnemy enemey))
                TargetList.Remove(enemey);
        }

        protected virtual void SetNewTarget()
        {
            if (TargetList.Count == 0)
            {
                TargetedEnemy = null;
                return;
            }

            while (TargetList.Count > 0)
            {
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
                    TargetedEnemy = null;
                }
                else
                {
                    break;
                }
            }
        }
    }
}