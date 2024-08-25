using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class TowerBehavior : MonoBehaviour
{

    SphereCollider myCollider;
    public float AttackRadius;
    //per second
    public float fireRate;
    public float burstDelay;
    public GameObject BulletPrefab;
    public GameObject BulletSource;
    // Start is called before the first frame update
    public List<GameObject> targetList = new List<GameObject>(); //We can switch GameObject to instances of the Enemy class

    //How the tower targets enemies
    public TargetType MyTargetingType;

    //The gameobject that should rotate to look at enemy target; (If null assumed to be this gameobject)
    public GameObject RotationAxis;
    //The mesh to show when trying to place.
    public Mesh PreviewMesh;
    //The particle system to play when shooting
    public ParticleSystem playOnShoot;
    //The audio source from which to fire from
    public AudioSource audioSource;

    public List<AudioClip> firingSFX;
    public enum TargetType {RandomSelect,FocusOnTarget };
    public AudioType audioPlayType = AudioType.Multisource;
    public enum AudioType {SingleSource,Multisource };
    void Start()
    {
        if (RotationAxis == null)
            RotationAxis = gameObject;
        myCollider =GetComponent<SphereCollider>();
        myCollider.radius=AttackRadius;

        if(audioSource!=null)
        {
            initial_volume = audioSource.volume;
        }

        StartCoroutine(FireLoop());
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other+" "+other.GetInstanceID()+" entered");
        targetList.Add(other.gameObject);
        //printList(list);

    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other + " " + other.GetInstanceID()+ " exited");
        if(other.gameObject== lastTargeted)
        {
            enemyID = -1;
        }
        targetList.Remove(other.gameObject);
        //printList(list);

    }


    //for testing
    void printList(List<int> num)
    {
        string numbs = "{ ";
        for (int i = 0; i < num.Count; i++)
        {
            numbs = numbs+ (num[i].ToString()+", ");
        }
        numbs += " }";
        Debug.Log(numbs);

    }
    int enemyID=0;
    GameObject lastTargeted;

    float allowAudioSourceAfter;
    public float AudioCoolDown=0.15f;
    float initial_volume = 0.02f;
    public int BurstLength = -1;
    private IEnumerator FireLoop()
    {
        int burstCounter = BurstLength;
        
        while (true)
        {
            if (targetList.Count > 0)
            {
                if (MyTargetingType == TargetType.FocusOnTarget)
                {
                    enemyID = targetList.IndexOf(lastTargeted);
                }
                if (MyTargetingType == TargetType.RandomSelect||enemyID<=-1)
                {
                    //pick a random enemy to attack
                    enemyID = Random.Range(0, targetList.Count - 1);
                }
                //   Debug.Log(targetList[enemyID]);
                //Make sure the enemy is not already dead (is null) and is enabled.
                if (targetList[enemyID] != null && targetList[enemyID].activeInHierarchy && targetList[enemyID].GetComponent<EnemyCharacteristics>() != null)
                {

                    bool abortShot = false;
                    if (BurstLength >= 0)
                    {
                        burstCounter++;

                        if (burstCounter > BurstLength)
                        {
                            burstCounter = 0;
                            yield return new WaitForSeconds(burstDelay);
                            abortShot = true;
                        }
                    }

                    if (!abortShot)
                    { 
                        GameObject unit = targetList[enemyID];
                    lastTargeted = unit;
                    // Debug.Log("PEW! hit enemy " + unit + " at location " + unit.transform.position);
                    BulletPrefab.GetComponent<bulletBehavior>().enemy = unit;
                    //prefab.GetComponent<bulletBehavior>().setTargetPosition(unit);
                    //the weird y position is so the bullet shoots from the top instaed of the middle. adjust as needed


                    //LookAt Code From Internet
                    if (unit != null)
                    {
                        // Get the direction to the enemy
                        Vector3 direction = unit.transform.position - RotationAxis.transform.position;
                        direction.y = 0; // Keep the direction strictly on the Y-axis

                        // Create the rotation towards the enemy
                        Quaternion rotation = Quaternion.LookRotation(direction);

                        // Apply the rotation to the tower
                        RotationAxis.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0);
                    }


                    Vector3 spawnPos = transform.position + new Vector3(0, 1, 0);
                    if (BulletSource != null)
                    {
                        spawnPos = BulletSource.transform.position;
                    }
                    Instantiate(BulletPrefab, spawnPos, transform.rotation);
                    if (playOnShoot != null)
                    {
                        playOnShoot.Play();
                    }
                    if (audioSource != null && firingSFX.Count > 0 && !audioSource.isPlaying)
                    {
                        AudioClip randomClip = firingSFX[Random.Range(0, firingSFX.Count)];
                        audioSource.volume = initial_volume * AudioManager.SFXVolume;

                        if (audioPlayType == AudioType.Multisource)
                        {
                            if (Time.timeSinceLevelLoadAsDouble >= allowAudioSourceAfter)
                            {
                                GameObject newSource = Instantiate(audioSource.gameObject, audioSource.transform.position, audioSource.transform.rotation, audioSource.transform.parent);

                                newSource.GetComponent<AudioSource>().clip = randomClip;

                                newSource.GetComponent<AudioSource>().Play();

                                Destroy(newSource, randomClip.length + 1);

                                allowAudioSourceAfter = Time.timeSinceLevelLoad + AudioCoolDown;

                            }

                        }
                        else
                        {
                            audioSource.GetComponent<AudioSource>().clip = randomClip;

                            audioSource.GetComponent<AudioSource>().Play();

                        }


                    }
                    yield return new WaitForSeconds(fireRate);

                }
                
                }
                else
                {//Lets remove the null instance and try again next loop interation.
                    targetList.RemoveAt(enemyID);
                }
            }
            else
            {
                burstCounter = BurstLength;
            }

            yield return new WaitForEndOfFrame();
        }

        
    }
}
