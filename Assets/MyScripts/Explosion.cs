using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioClip;
    void Start()
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        if (audioSource != null)
    {
        audioSource.Play();
    }


    ParticleSystem BurstPS = this.GetComponent<ParticleSystem>();
    ParticleSystem SmokePS = transform.Find("Smoke").GetComponent<ParticleSystem>();
    ParticleSystem BurstLightPS = transform.Find("LightBurst").GetComponent<ParticleSystem>();
    BurstPS.Play();
    SmokePS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem BurstPS = this.GetComponent<ParticleSystem>();
        ParticleSystem SmokePS = transform.Find("Smoke").GetComponent<ParticleSystem>();
        if (!BurstPS.isPlaying&&!SmokePS.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}
