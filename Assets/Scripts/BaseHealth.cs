using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [Min(0)]
    public float CurrentHealth=4;

    public Action EnemyBreached;
    public Action BaseDefeated;

    public GameObject[] Carrots;

    public AudioSource PluckingSound;
    float defaultVolume=0.02f;

    public MenuManager PauseMenuManager;
    public CanvasGroup DeathScreen;
    void FadeDeathScreen()
    {
        PauseMenuManager.TweenEnable(DeathScreen);
    }
    private void Start()
    {
        EnemiesBreached =new List<GameObject> ();
        if (PluckingSound!=null)
        {
            defaultVolume = PluckingSound.volume;
        }
        if(PauseMenuManager!=null&&DeathScreen!=null)
            BaseDefeated += FadeDeathScreen;
    }

    public void DamageBase(int amount)
    {
        CurrentHealth -= amount;

        PluckingSound.volume = defaultVolume * AudioManager.SFXVolume;
        PluckingSound.Play();

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

        if(other.gameObject.GetComponent<EnemyCharacteristics>()!=null&&!EnemiesBreached.Contains(other.gameObject))
        {
            EnemiesBreached.Add(other.gameObject);
            other.gameObject.GetComponent<EnemyCharacteristics>().Die(false,true);
            DamageBase(other.gameObject.GetComponent<EnemyCharacteristics>().CarrotDamage);
        }

    }
}
