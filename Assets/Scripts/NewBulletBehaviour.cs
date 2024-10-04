using System;
using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;
using DefaultNamespace.TowerSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public interface IPoolable
{
    public int ID { get; }
    public GameObject Prefab { get; }
    public void Dispose();
}

namespace DefaultNamespace
{
    public class NewBulletBehaviour : MonoBehaviour
    {
        private List<Debuff> _debuffs;
        private IEnemyUnit _target;
        private BulletConfig _bulletConfig;
        
        //Spawns + Entangles with enemies when killed (Bubbles foreach enemy etc.)
        public GameObject EntangleWhenKillEnemy;

        private Vector3 _lastDirection;

        private float durationTimer = 0;
        
        public void Initialize(List<Debuff> debuffs, IEnemyUnit target, BulletConfig bulletConfig)
        {
            _debuffs = debuffs;
            _target = target;
            _bulletConfig = bulletConfig;

            _lastDirection = _target.mTransform.position - transform.position;
        }
       
        private void Update()
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > 10) // Total Life Duration
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

        private void OnTriggerEnter(Collider other)
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
                ApplyDamage(other.transform);
            }
            
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
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}