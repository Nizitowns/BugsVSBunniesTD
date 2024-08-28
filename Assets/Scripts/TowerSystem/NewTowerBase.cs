using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace.TowerSystem
{
    public abstract class NewTowerBase : MonoBehaviour, ITower
    {
        public TowerScriptableObject Config { get; private set; }

        protected SphereCollider _myCollider;
        [SerializeField] protected GameObject RotateAxis;
        [SerializeField] protected Transform BulletSource;
        protected AudioSource mAudioSource;
        
        protected List<IEnemy> TargetList = new List<IEnemy>();
        protected IEnemy TargetedEnemy;
        protected IEnemy LastTargeted;

        protected Coroutine FireRoutine;
        protected bool IsShooting = true;
        protected float BurstTimer = 0;
        protected float AudioTimer = 0;

        protected bool isDisabled = false;

        public bool IsDisabled
        {
            get => isDisabled;
            set => isDisabled = value;
        }

        private ParticleSystem _particleSystem;

        public virtual void Initiliaze(TowerScriptableObject Config)
        {
            this.Config = Config;
            
            _myCollider = GetComponent<SphereCollider>();
            _myCollider.radius = Config.attackRadius;
            mAudioSource = ComponentCopier.CopyComponent(SoundFXPlayer.Instance.Source, BulletSource.gameObject);
            
            BurstTimer = Time.time;
            _particleSystem = Instantiate(Config.particulOnShoot, BulletSource);
            FireRoutine = StartCoroutine(FireLoopCo());
        }

        protected virtual void OnFire()
        {
            if (!CanShoot()) return;
            
            var spawnPos = BulletSource.position + new Vector3(0, 1, 0);
            var bullet = Instantiate(Config.bulletPrefab, spawnPos, transform.rotation);
            bullet.GetComponent<bulletBehavior>().enemy = TargetedEnemy.mTransform.gameObject;
            
            _particleSystem.Play();
            PlaySoundFX();
        }

        protected virtual bool CanShoot(bool updateWhileWaitingTarget = false)
        {
            if (Config.burstDelay == 0)
            {
                IsShooting = true;
                return true;
            }

            if (updateWhileWaitingTarget)
            {
                if (BurstTimer + Config.burstDelay < Time.time)
                {
                    BurstTimer = Time.time;
                    IsShooting = true;
                }

                return IsShooting;
            }
            
            if (BurstTimer + Config.burstDelay < Time.time)
            {
                BurstTimer = Time.time;
                IsShooting = !IsShooting;
            }

            return IsShooting;
        }


        private void PlaySoundFX()
        {
            if (AudioTimer + Config.audioCoolDown < Time.time)
            {
                var randomClip = Config.firingSfx[Random.Range(0, Config.firingSfx.Count)];
                // SoundFXPlayer.PlaySFX(mAudioSource, randomClip);
                mAudioSource.PlayOneShot(randomClip, AudioManager.SFXVolume);

                AudioTimer = Time.time;
            }
        }
        
        protected virtual IEnumerator FireLoopCo()
        {
            while (Application.isPlaying)
            {
                yield return new WaitUntil(() => !isDisabled);
                
                SetNewTarget();

                if (TargetedEnemy != null)
                {
                    RotateToTarget();
                    OnFire();
                }
                else
                {
                    CanShoot(true);
                }
            
                yield return new WaitForSeconds(Config.fireRate);
            }
        }

        protected void RotateToTarget()
        {
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
                case TargetType.FocusOnFirst:
                    TargetedEnemy = TargetList[0];
                    break;
                case TargetType.FocusOnLast:
                    TargetedEnemy = TargetList[^1];
                    break;
            }

            if (TargetedEnemy.isDead)
            {
                TargetList.Remove(TargetedEnemy);
                SetNewTarget();
            }
               
        }

        private void OnDestroy()
        {
            StopCoroutine(FireRoutine);
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