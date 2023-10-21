using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class Door : MonoBehaviour
{
    public static bool isOpen = false;
    public static bool OpenClose = false;
    public List<GameObject> neededTorches;
    private float updateTimer = 0.0f;

    public GameObject door;
    
    public AudioClip door_sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* RaycastHit hit;
         if(Physics.Raycast( transform.position, transform.forward, out hit, 5))
         {
             if( (hit.collider.gameObject.tag == "Door") && (!isOpen) )
             {

                 isOpen = true;

             }

         }*/
        int torch_num = 0;
        

        

        foreach (GameObject torchObject in neededTorches)
        {
            GameObject Fire = GameObject.Find(torchObject.gameObject.name + "/Fire");
            if (Fire != null)
            {
                ParticleSystem particleSystem = Fire.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    if (particleSystem.isPlaying) torch_num++;
                }
            }
        }

            if (OpenClose && torch_num < neededTorches.Count)
        {
            if (updateTimer < 0.5)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null && door_sound != null)
                {
                    audioSource.PlayOneShot(door_sound);
                }
                door.transform.position += new Vector3(Time.deltaTime / 2, 0, 0);


                updateTimer += Time.deltaTime;

            }
            if (updateTimer >= 0.5 && updateTimer < 1.5)
            {


                updateTimer += Time.deltaTime;

            }
            if (updateTimer >= 1.5 && updateTimer < 2)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null && door_sound != null)
                {
                    audioSource.PlayOneShot(door_sound);
                }
                door.transform.position -= new Vector3(Time.deltaTime / 2, 0, 0);

                updateTimer += Time.deltaTime;

            }
            if (updateTimer >= 2)
            {
                OpenClose = false;
                updateTimer = 0.0f;

            }

        }
        if (torch_num>= 4)
        {

            if (updateTimer < 1.5)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null && door_sound != null)
                {
                    audioSource.PlayOneShot(door_sound);
                }
                door.transform.position += new Vector3(Time.deltaTime, 0, 0);


                updateTimer += Time.deltaTime;

            }

        }
    }
}
