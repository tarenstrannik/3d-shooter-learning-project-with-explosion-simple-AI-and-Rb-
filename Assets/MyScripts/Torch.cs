using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void torchLit()
    {
            GameObject Fire = GameObject.Find(this.gameObject.name + "/Fire");
            if (Fire != null)
            {
                ParticleSystem particleSystem = Fire.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    if (!particleSystem.isPlaying)
                    {
                        particleSystem.Play();

                    }
                }
            }


    }
}
