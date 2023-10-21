using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rocketPrefab;
    public float initialSpeed = 20.0f;

    public float reloadTime = 0.5f;

    private float lastShot = 0.0f;
    public Transform RocketSpawn;
    public List<Collider>  noCollide;
    public Transform plCameraRoot = null;
    public float minWeaponMoveDist = 3.0f;
    public GameObject WeaponOwner;
    private Quaternion startWeaponRotation; 
    void Awake()
    {
        startWeaponRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (plCameraRoot != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(plCameraRoot.position, plCameraRoot.forward, out hit, float.MaxValue))
            {

                if (Vector3.Distance(transform.position, hit.point) >= minWeaponMoveDist)
                {
                    //Debug.Log(Vector3.Distance(transform.position, hit.point)+" "+ minWeaponMoveDist);
                    Vector3 direction = hit.point - transform.position;

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5 * Time.deltaTime);
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, startWeaponRotation * plCameraRoot.rotation, 5 * Time.deltaTime);
            }
        }
    }

    public void Fire()
    {
        if (Time.time > reloadTime + lastShot) 
        {
           
            Rigidbody rocket = Instantiate(rocketPrefab, RocketSpawn.position, RocketSpawn.rotation);
            WeaponOwner.GetComponent<CharAmmo>().SendMessage("SpendAmmo",1);

            rocket.velocity = rocket.GetComponent<Transform>().TransformDirection(new Vector3(0, 0, initialSpeed));
            rocket.name = "rocket";
            foreach(Collider coll in noCollide)
            Physics.IgnoreCollision(rocket.GetComponent<Collider>(), coll);
            //Collider capsuleCollider = GetComponentInChildren<Collider>();
            //Physics.IgnoreCollision( capsuleCollider, grenade.GetComponent<Collider>(), true );
            //Physics.IgnoreCollision( transform.root.GetComponent<Collider>(), grenade.GetComponent<Collider>(), true );


            lastShot = Time.time;


        }
    }
}
