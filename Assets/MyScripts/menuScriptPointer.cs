using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class menuScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string level;
    public Texture2D texture1;
    public Texture2D texture2;
    public AudioClip menu_beep;
    public bool isQuit;

    private RawImage rawImage;
    void Awake()
	{
        Debug.Log("Awake");
		rawImage = GetComponent<RawImage>();
	}
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  OnPointerEnter(PointerEventData eventData)
{
     // Получаем компонент RawImage
        Debug.Log("Enter");
        if (rawImage != null)
        {
            rawImage.texture = texture2; // Присваиваем текстуру объекту RawImage
        }
}

public void  OnPointerExit(PointerEventData eventData)
{
    Debug.Log("Exit");
    if (rawImage != null)
        {
            rawImage.texture = texture1; // Присваиваем текстуру объекту RawImage
        }
}

public void OnPointerClick(PointerEventData eventData)
{
    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource != null && menu_beep != null)
    {
        audioSource.PlayOneShot(menu_beep);
    }
    else
    {
        if (audioSource == null)
        {
            Debug.Log("MissingAudio1");
        }
        else
        {
            Debug.Log("MissingAudio");
        }
    };
    

	  if( isQuit ){

	    #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif

	     Debug.Log("Game Over!");

	  } else {
        
	    SceneManager.LoadScene( level );

	  }
}
}
