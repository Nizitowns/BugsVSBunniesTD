using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.TowerSystem;
using Helper;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [Min(0)]
    public float CurrentHealth=4;

    public Action EnemyBreached;
    public Action BaseDefeated;

    public GameObject[] Carrots;

    float defaultVolume=0.02f;

    public MenuManager PauseMenuManager;
    public CanvasGroup DeathScreen;

    private AudioSource mAudioSource;

    [SerializeField] private AudioClip _audioClip;
    
    void FadeDeathScreen()
    {
        PauseMenuManager.TweenEnable(DeathScreen);
    }
    private void Start()
    {
        EnemiesBreached =new List<GameObject> ();
       
        if(PauseMenuManager!=null&&DeathScreen!=null)
            BaseDefeated += FadeDeathScreen;
        
        mAudioSource = ComponentCopier.CopyComponent(SoundFXPlayer.Source, transform.gameObject);
    }

    public void DamageBase(int amount)
    {
        CurrentHealth -= amount;

        mAudioSource.PlayOneShot(_audioClip, AudioManager.SFXVolume * 10);

        for (int i=0;i<Carrots.Length;i++)
        {
            if (((i + 1) > CurrentHealth) && Carrots[i] != null && Carrots[i].GetComponent<Rigidbody>().constraints==RigidbodyConstraints.FreezeAll)
            {

                Carrots[i].GetComponent<Rigidbody>().AddForce(Vector3.up*10,ForceMode.Impulse);
                Carrots[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                Destroy(Carrots[i].gameObject, 5);
            }
        }

        EnemyBreached?.Invoke();

        if(CurrentHealth<=0)
        {
            DefeatedBase();
        }
    }
    List<GameObject> EnemiesBreached;
    public void DefeatedBase()
    {

        BaseDefeated?.Invoke();
    }
        private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<DefaultEnemy>()!=null&&!EnemiesBreached.Contains(other.gameObject))
        {
            EnemiesBreached.Add(other.gameObject);
            other.gameObject.GetComponent<DefaultEnemy>().Kill(false);
            DamageBase(other.gameObject.GetComponent<Enemy>().Config.carrotDamage);
        }

    }
}
