using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class targetShoot : MonoBehaviour
{

    public AudioClip targetHit;
    public AudioClip targetRest;

    private bool hit = false;
    public float resetTime = 3.0f;
    private float timer = 0.0f;
    public GameObject targetParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            

          timer += Time.deltaTime;
            

      }
        

      if (timer > resetTime)
        {


            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && targetHit != null)
            {
                audioSource.PlayOneShot(targetRest);
            }

            targetParent.GetComponent<Animation>().Play("up");
hit = false;

         timer = 0.0f;

      }
    }
    void OnCollisionEnter(Collision shootObject )
    {
        

       if (hit == false && shootObject.gameObject.name == "rocket")
        {

            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && targetHit != null)
            {
                audioSource.PlayOneShot(targetHit);
            }

            targetParent.GetComponent<Animation>().Play("down");

           hit = true;
       

       }
        
    }
}
