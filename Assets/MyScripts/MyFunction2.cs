
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClass1 : MonoBehaviour
{
public static bool rotating = false;
private float updateTimer = 0.0f;
	
    void Start()
    {
		
    }
	
	void Update()
    {
        if( rotating ) 
		{
			transform.Rotate(0, Time.deltaTime*20, 0, Space.World );
			updateTimer += Time.deltaTime; 
//Debug.Log(updateTimer);
	     if( updateTimer > 0.8 ) {

                //SendMessageUpwards("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
                //Debug.Log(MyHealth.curHealth);
                updateTimer = 0.0f;

	     }
 
		};
		if (Input.GetKeyDown (KeyCode.F1)) 
		{
			rotating = !rotating;

		}
    }

	void OnMouseEnter()
{
    rotating = true;
}

void OnMouseExit()
{
    rotating = false;
}
void OnMouseOver () {

	  Color currentColor = GetComponent<Renderer>().material.color;
    float newRed = currentColor.r + 1f * Time.deltaTime;
    Color newColor = new Color(newRed, currentColor.g, currentColor.b);
    GetComponent<Renderer>().material.color = newColor;
	
	
	
	
	
	
	}
}

// transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
// ```