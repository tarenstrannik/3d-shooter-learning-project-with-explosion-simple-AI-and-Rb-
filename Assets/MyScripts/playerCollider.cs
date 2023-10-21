using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(AudioSource))]

public class playerCollider : MonoBehaviour
{
     public AudioClip crate_take;
     public static int grenade;

     private GameObject UI_Canvas;
     private bool haveMatches = false;

     private GameObject Fire;

     public GameObject MatchGUI;

    // Start is called before the first frame update

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter( Collider collisionInfo  ){
//Debug.Log(collisionInfo.gameObject.name);
	   if( collisionInfo.gameObject.tag == "Crate"){
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && crate_take != null)
    {
        audioSource.PlayOneShot(crate_take);
    }
	      

	      Destroy( collisionInfo.gameObject );
            GetComponent<CharAmmo>().SendMessage("TakeAmmo", 20);


	   }
       if( collisionInfo.gameObject.tag == "GrenadeBox"){
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && crate_take != null)
    {
        audioSource.PlayOneShot(crate_take);
    }
	      

	      Destroy( collisionInfo.gameObject );
            GetComponent<CharGrenade>().SendMessage("TakeGren", 10);
 

	   }

       if( collisionInfo.gameObject.tag == "Matchbox"){
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && crate_take != null)
    {
        audioSource.PlayOneShot(crate_take);
    }
	      

	      Destroy( collisionInfo.gameObject );
          haveMatches = true;
          UI_Canvas=  GameObject.Find("UI_Canvas");
          GameObject matchGUIObj = Instantiate(MatchGUI, UI_Canvas.transform);

	   }

	
    if( collisionInfo.gameObject.tag == "Torch"){
        if(haveMatches)
        {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && crate_take != null)
    {
        audioSource.PlayOneShot(crate_take);
    }
                collisionInfo.SendMessageUpwards("torchLit", SendMessageOptions.DontRequireReceiver);


	   }
    }

}
}
