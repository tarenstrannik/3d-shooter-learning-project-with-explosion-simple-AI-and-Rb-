using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotAnimations : MonoBehaviour
{

    public float minimumWalkSpeed = 0.5f;
    public float minimumRunSpeed = 3f;
    void Start()
    {
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
        GetComponent<Animation>()["shoot"].wrapMode = WrapMode.Once;
        GetComponent<Animation>().Stop();
    }
    private void OnEnable()
    {
        GetComponent<Animation>().Stop();
    }
    void SetSpeed(float speed )
    {
        if (speed >= minimumRunSpeed)
            GetComponent<Animation>().CrossFade("run");
        else if (speed >= minimumWalkSpeed && speed < minimumRunSpeed)
            GetComponent<Animation>().CrossFade("walk");
        else
            GetComponent<Animation>().CrossFade("idle");
    }

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
