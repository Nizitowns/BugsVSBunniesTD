using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.TowerSystem
{
    [SelectionBase]
    public abstract class NewTowerBase : MonoBehaviour
    {
        public TowerScriptableObject Config { get; private set; }

        protected SphereCollider _myCollider;
        [SerializeField] protected GameObject RotateAxis;
        [SerializeField] protected Transform BulletSource;
        protected AudioSource mAudioSource;
        
        protected List<Enemy> TargetList = new List<Enemy>();
        protected Enemy TargetedEnemy;
        protected Enemy LastTargeted;

        protected Coroutine FireRoutine;
        protected bool CanShoot = true;
        private float shootTimer = 0;
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
        }

        private void Update()
        {
            OnUpdate();
        }
        
        protected virtual void OnUpdate() { }

        private void FixedUpdate()
        {
            UpdateFire();
            UpdateBurstTimer();
        }
        
        protected virtual void UpdateFire()
        {
            shootTimer += Time.deltaTime;

            if (shootTimer < Config.fireRate) return;
            shootTimer = 0;
            
            if (isDisabled) return;
                
            SetNewTarget();

            if (TargetedEnemy != null)
            {
                RotateToTarget();
                OnFire();
            }
        }

        protected virtual void OnFire()
        {
            if (!CanShoot) return;
            
            var spawnPos = BulletSource.position + new Vector3(0, 1, 0);
            var bullet = Instantiate(Config.BulletConfig.prefab, spawnPos, transform.rotation);
            bullet.GetComponent<NewBulletBehaviour>().Initialize(Config.debuffs, TargetedEnemy, Config.BulletConfig);
            
            _particleSystem.Play();
            PlaySoundFX();
        }

        private bool reset = true;
        
        private void UpdateBurstTimer() // TODO Separate this into cooldown and firetime
        {
            if (Config.burstDelay == 0)
            {
                CanShoot = true;
                return;
            }
            
            if (TargetList.Count == 0 && reset)
            {
                if (BurstTimer + Config.burstDelay < Time.time)
                {
                    BurstTimer = Time.time;
                    CanShoot = true;
                    reset = false;
                }
            }
            else if (BurstTimer + Config.burstDelay < Time.time)
            {
                BurstTimer = Time.time;
                CanShoot = !CanShoot;
                reset = true;
            }
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

        protected void RotateToTarget()
        {
            var direction = TargetedEnemy.mTransform.position - transform.position;
            direction.y = 0;
            var rotation = Quaternion.LookRotation(direction);
            RotateAxis.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0);
        }

        private void SetNewTarget()
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
                case TargetType.FocusOnMiddle:
                    TargetedEnemy = TargetList[(int)(TargetList.Count / 2)];
                    break;
                case TargetType.FocusOnLast:
                    TargetedEnemy = TargetList[^1];
                    break;
            }

            if (TargetedEnemy.IsDead)
            {
                TargetList.Remove(TargetedEnemy);
                SetNewTarget();
            }
        }

        private void OnDestroy() => StopCoroutine(FireRoutine);

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemey))
                TargetList.Add(enemey);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out Enemy enemey))
                TargetList.Remove(enemey);
        }

        
        private void OnDrawGizmosSelected()
        {
            if (Debugger.ShowTowerRange)
            {
                var radius = GetComponent<SphereCollider>().radius;
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}