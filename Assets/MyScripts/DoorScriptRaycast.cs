using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class DoorScriptRaycast : MonoBehaviour
{
    public static bool isOpen = false;
    public static bool OpenClose = false;
    public static int torch_num = 0;
    private float updateTimer = 0.0f;

    public GameObject door;
     private RaycastHit hit;

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
        if(OpenClose && torch_num<4)
        {
                if( updateTimer < 0.5 ) {
             AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && door_sound != null)
            {
                audioSource.PlayOneShot(door_sound);
            }
	       door.transform.position += new Vector3(Time.deltaTime/2, 0, 0);


	       updateTimer += Time.deltaTime; 

	        } 
            if( updateTimer>=0.5 && updateTimer < 1.5 ) {
            

	       updateTimer += Time.deltaTime; 

	     } 
         if( updateTimer >=1.5 &&  updateTimer<2) {
          AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && door_sound != null)
            {
                audioSource.PlayOneShot(door_sound);
            }
	       door.transform.position -= new Vector3(Time.deltaTime/2, 0, 0);

	       updateTimer += Time.deltaTime;  

	     } 
         if( updateTimer >=2 ) {
           OpenClose=false; 
	       updateTimer = 0.0f; 

	     } 
         
        }
        if( isOpen ){ 

	     if( updateTimer < 1.5 ) {
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
