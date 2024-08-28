using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.TowerSystem
{
    [CreateAssetMenu(menuName = "New Tower", fileName = "New Tower")]
    public class TowerScriptableObject : ScriptableObject
    {
        [Header("Purchase Settings")] 
        [Tooltip("What this tower appears as when selected.")]
        public GameObject prefab;
        public string SelectedTitle;
        public Sprite purchaseIcon;
        public Mesh previewMesh;
        public int purchaseCost;
        public Color purchaseIconAdditionalTint = Color.white;
        public GameObject[] upgradePaths;

        [Header("Tower Settings")]
        public float attackRadius;
        public float fireRate;
        public float burstDelay;
        public GameObject bulletPrefab;
        public TargetType myTargetingType;
        
        [Header("Particle Settings")]
        public ParticleSystem particulOnShoot;

        [Header("Audio Settings")] 
        public List<AudioClip> firingSFX;
        public AudioType audioPlayType = AudioType.Multisource;
        public float audioCoolDown;
    }

    public enum TargetType
    {
        RandomSelect,
        FocusOnTarget
    }

    public enum AudioType
    {
        SingleSource,
        Multisource
    }
}