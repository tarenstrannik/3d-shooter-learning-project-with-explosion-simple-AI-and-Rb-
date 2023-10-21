using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    //public bool isOpen = false;
    //private float updateTimer = 0.0f;

    //public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* if( isOpen ){ 

	     if( updateTimer < 1 ) {

	       door.transform.position += new Vector3(Time.deltaTime, 0, 0);


	       updateTimer += Time.deltaTime; 

	     } 

	  }*/
    }
    void OnControllerColliderHit ( ControllerColliderHit hit )
    {
       
	  if( (hit.gameObject.tag == "Door") && (!DoorScriptRaycast.isOpen)&& (!DoorScriptRaycast.OpenClose) ){

	       Door.OpenClose = true;

	   }

	}
}
