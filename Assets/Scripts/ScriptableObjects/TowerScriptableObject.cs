using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.TowerSystem
{
    [CreateAssetMenu(fileName = "New Tower", menuName = "New Data / Tower Data")]
    public class TowerScriptableObject : ScriptableObject
    {
        [Header("Purchase Settings")] 
        public int purchaseCost;
        public string selectedTitle;
        public Sprite purchaseIcon;
        public GameObject prefab;
        public Mesh previewMesh;
        public Color purchaseIconAdditionalTint = Color.white;

        [Header("Tower Settings")]
        public float attackRadius;
        public float fireRate;
        public float burstDelay;
        public GameObject bulletPrefab;
        public TargetType myTargetingType;
        
        [Header("Upgradable To")]
        public TowerScriptableObject[] upgradeOptions;
        
        [Header("Particle Settings")]
        public ParticleSystem particulOnShoot;

        [Header("Audio Settings")] 
        public float audioCoolDown;
        public AudioType audioPlayType = AudioType.Multisource;
        public List<AudioClip> firingSfx;
    }

    public enum TargetType
    {
        RandomSelect,
        FocusOnFirst,
        FocusOnMiddle,
        FocusOnLast,
    }

    public enum AudioType
    {
        SingleSource,
        Multisource
    }
}