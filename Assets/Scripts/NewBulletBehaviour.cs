using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.OnDeathEffects;
using DefaultNamespace.TowerSystem;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public interface IPoolable
{
    public PoolManager.PoolID ConnectedPool { get; }
    public void Reset();
    public void Dispose();
}

namespace DefaultNamespace
{
    public class NewBulletBehaviour : MonoBehaviour, IPoolable
    {
        private List<Debuff> _debuffs;
        private IEnemyUnit _target;
        private BulletConfig _bulletConfig;
        
        //Spawns + Entangles with enemies when killed (Bubbles foreach enemy etc.)
        public GameObject EntangleWhenKillEnemy;

        private Vector3 _lastDirection;

        private float durationTimer = 0;

        public void Initialize(List<Debuff> debuffs, IEnemyUnit target, BulletConfig bulletConfig, Vector3 startPosition)
        {
            _debuffs = debuffs;
            _target = target;
            _bulletConfig = bulletConfig;
            transform.position = startPosition;

            if(_target != null)
                _lastDirection = _target.mTransform.position - transform.position;
            
            if (bulletConfig.useRaycast)
            {
                // Deactivate Particles and visual
                var particles = transform.GetComponentsInChildren<ParticleSystem>();
                foreach (var particle in particles)
                    particle.Stop();

                var meshRenderers = transform.GetComponents<MeshRenderer>();
                foreach (var mesh in meshRenderers)
                    mesh.enabled = false;
            }
        }
        
        private void Update()
        {
            if (_bulletConfig.useRaycast) return;
            
            durationTimer += Time.deltaTime;
            if (transform.position.y < -0.5f || durationTimer > 5) // Total Life Duration
                Dispose();

            if (!CheckTargetAvaliablity())
            {
                transform.position += _lastDirection.normalized * _bulletConfig.speed * Time.deltaTime;
                return;
            }
            
            _lastDirection = _target.mTransform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _target.mTransform.position,
                _bulletConfig.speed * Time.deltaTime);
        }

        public void CustomTriggerCheck(IEnemyUnit target, Vector3 startPosition)
        {
            var lastDir = target.mTransform.position - startPosition;

            Ray ray = new Ray(startPosition, lastDir);
            var hitCastAll = Physics.RaycastAll(ray, 1000);
            for (int i = 0; i < hitCastAll.Length; i++)
            {
                if (hitCastAll[i].transform.TryGetComponent(out IEnemyUnit unit) && unit == target)
                {
                    CheckCollision(unit.mTransform);
                    return;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckCollision(other.transform);
        }

        private void CheckCollision(Transform transform)
        {
            if (_bulletConfig.activateAOE)
            {
                Vector3 pos = new Vector3(transform.position.x, 2.5f, transform.position.z);
                Ray ray = new Ray(pos, Vector3.down);
                var hitall = Physics.SphereCastAll(ray, _bulletConfig.hitAreaRadius);
                foreach (var hit in hitall)
                {
                    ApplyDamage(hit.transform);
                }
                
                // Debuging
                if (Debugger.ShowAreaEffectVisualizer)
                {
                    var ball = Instantiate(DebugVisual.Instance.BulletAreaVisualizer);
                    ball.transform.position = pos;
                    ball.transform.localScale = Vector3.one * _bulletConfig.hitAreaRadius * 2;
                
                    Destroy(ball, 0.15f);
                }
            }
            else
            {
                ApplyDamage(transform);
            }
            
            if(!_bulletConfig.useRaycast)
                Dispose();
        }
        

        private void ApplyDamage(Transform target)
        {
            if (target.TryGetComponent(out IDebuffable debuffable))
            {
                foreach (var debuff in _debuffs)
                {
                    debuffable.ApplyDebuff(debuff);
                }
            }
            
            if (target.TryGetComponent(out IEnemyUnit enemyUnit))
            {
                if (enemyUnit.TakeDamage(_bulletConfig.damage, false))
                {
                    ApplySpecialDeath(enemyUnit);
                    enemyUnit.Kill(true);
                }
            }
        }

        private bool CheckTargetAvaliablity()
        {
            if (_target == null) return false;
            if (_target.IsDead) return false;
            if (_target.mTransform == null) return false;
            if (!_target.mTransform.gameObject.activeInHierarchy) return false;

            return true;
        }

        private void ApplySpecialDeath(IEnemyUnit enemyUnit)
        {
            var copiedMesh = new Mesh();
            var skinnedMesh = new SkinnedMeshRenderer();

            GameObject newMeshObject = null;
            MeshFilter meshFilter = null;
            MeshRenderer meshRenderer = null;
            
            switch (_bulletConfig.deathEffect)
            {
                case eDeathEffect.None:
                    // Nothing Happens - Just Die
                    break;
                case eDeathEffect.Shrink:
                    skinnedMesh = enemyUnit.mTransform.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                    skinnedMesh.BakeMesh(copiedMesh);
            
                    newMeshObject = new GameObject("CopiedMesh");
                    meshFilter = newMeshObject.AddComponent<MeshFilter>();
                    meshRenderer = newMeshObject.AddComponent<MeshRenderer>();
                    meshFilter.mesh = copiedMesh;
                    meshRenderer.material = skinnedMesh.material;

                    var killTime = 0.5f;
                    newMeshObject.transform.position = enemyUnit.mTransform.position;
                    newMeshObject.transform.DOScale(Vector3.zero, killTime).SetEase(Ease.InBack).SetLink(newMeshObject);
                    
                    Destroy(newMeshObject, killTime);
                    break;
                case eDeathEffect.BubbleUp:
                    skinnedMesh = enemyUnit.mTransform.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                    skinnedMesh.BakeMesh(copiedMesh);
            
                    newMeshObject = new GameObject("CopiedMesh");
                    meshFilter = newMeshObject.AddComponent<MeshFilter>();
                    meshRenderer = newMeshObject.AddComponent<MeshRenderer>();
                    meshFilter.mesh = copiedMesh;
                    meshRenderer.material = skinnedMesh.material;
            
                    var spawn = Instantiate(EntangleWhenKillEnemy, enemyUnit.mTransform.position, quaternion.identity);
            
                    spawn.transform.localScale *= enemyUnit.mTransform.localScale.x;
                    newMeshObject.transform.SetParent(spawn.transform);
                    newMeshObject.transform.localPosition = new Vector3(0,0, -enemyUnit.offset); // TODO Add Offset Based On Enemy's Pivot Point
            
                    spawn.GetComponent<Rigidbody>().AddForce(new Vector3(_lastDirection.x, 0, _lastDirection.z).normalized * 10, ForceMode.Impulse);
                    spawn.GetComponent<Rigidbody>().AddTorque(new Vector3().RandomDirection() * 10, ForceMode.Impulse);

                    Destroy(spawn, 5);
                    break;
                
                case eDeathEffect.FlyAway:
                    skinnedMesh = enemyUnit.mTransform.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                    skinnedMesh.BakeMesh(copiedMesh);
            
                    newMeshObject = new GameObject("CopiedMesh");
                    meshFilter = newMeshObject.AddComponent<MeshFilter>();
                    meshRenderer = newMeshObject.AddComponent<MeshRenderer>();
                    meshFilter.mesh = copiedMesh;
                    meshRenderer.material = skinnedMesh.material;

                    newMeshObject.transform.position = enemyUnit.mTransform.position;
                    
                    var rigidbody = newMeshObject.AddComponent<Rigidbody>();
                    // rigidbody.useGravity = false;
                    rigidbody.AddForce(new Vector3(_lastDirection.x, 0, _lastDirection.z).normalized * 50, ForceMode.Impulse);
                    rigidbody.AddTorque(new Vector3().RandomDirection() * 10, ForceMode.Impulse);
                    
                    Destroy(newMeshObject, 5);
                    break;
                
                case eDeathEffect.Electrocute:
                    // TODO Get Electrocuted
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CallApplyDamage(Transform target) => ApplyDamage(target);

        public PoolManager.PoolID ConnectedPool { get; set; }

        public void Reset()
        {
            durationTimer = 0;
            _lastDirection = Vector3.zero;
        }

        public void Dispose()
        {
            ConnectedPool.ReturnToPool(this);
        }
    }
}