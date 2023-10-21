using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotPartHealth : MonoBehaviour
{
    public Transform robotParent;
    private float partMass;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        partMass = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ApplyDamage(float damage)
    {
       // Debug.Log(name+" "+damage+" "+ partMass);
        if (robotParent.GetComponent<robotRagdoll>().currentWeapon != gameObject)
        {
         //   Debug.Log(name + " " + damage + " " + partMass);
            robotParent.GetComponent<robotRigidbodyHealth>().ApplyDamageRB(damage, partMass);
        }
    }
}
