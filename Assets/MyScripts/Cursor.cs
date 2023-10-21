using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnApplicationFocus(bool hasFocus)
    {

        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
         
        }
    }
}
