using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class menuScriptMouse : MonoBehaviour
{
//     public string level;
//     public Texture2D texture1;
//     public Texture2D texture2;
//     public AudioClip menu_beep;
//     public static bool isQuit;

    //private RawImage rawImage;
    void Awake()
	{
        Debug.Log("B1");
		//rawImage = GetComponent<RawImage>();
	}
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("b2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnMouseEnter()
{
     // Получаем компонент RawImage
        Debug.Log("BEnter");
        // if (rawImage != null)
        // {
        //     rawImage.texture = texture2; // Присваиваем текстуру объекту RawImage
        // }
}

 void OnMouseExit()
{
    Debug.Log("BExit");
    // if (rawImage != null)
    //     {
    //         rawImage.texture = texture1; // Присваиваем текстуру объекту RawImage
    //     }
}
}
