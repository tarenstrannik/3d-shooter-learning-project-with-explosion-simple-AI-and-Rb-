using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotRigidbodyHealth : MonoBehaviour
{

 
    public float totalMass = 0f;
    public float curRBHealth = 0f;

    void Start()
    {
        CalculateMassAndAddHealth(transform);
    }

    void CalculateMassAndAddHealth(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Collider col = obj.GetComponent<Collider>();

        if (rb != null && col != null)
        {
            totalMass += rb.mass;
            obj.gameObject.AddComponent<robotPartHealth>().robotParent = transform;
        }

        // Рекурсивный обход всех дочерних объектов
        foreach (Transform child in obj)
        {
            CalculateMassAndAddHealth(child);
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name+" "+curRBHealth);
        if (curRBHealth <= 0)
        {
            SendMessageUpwards("rigidbodyDie", SendMessageOptions.DontRequireReceiver);


        }
    }

   public void ApplyDamageRB(float damage, float mass)
    {
        curRBHealth -= damage * mass / totalMass;
        

    }
}
