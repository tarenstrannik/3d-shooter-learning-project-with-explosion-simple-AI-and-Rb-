using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class robotRagdoll : MonoBehaviour
{
    public Transform parentRobot;
    public Rigidbody robotBody;
    public float deltaTnoFalling = 0.1f;
    private float deltaTime;
    public float minSpeed = 0.5f;

    public Transform dead_robot;
    public AudioClip dieSound;
    public GameObject currentWeapon;
    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("1"+ GetInstanceID()+robotBody);
        deltaTime = 0.0f;
  
    }

    // Update is called once per frame
    void Update()
    {
        
        if (robotBody.velocity.magnitude < minSpeed)
        {
            //Debug.Log(robotBody.velocity.magnitude);
            deltaTime += Time.deltaTime;
        }
        else
        {
            deltaTime = 0.0f;
        }

            if (deltaTime >= deltaTnoFalling)
        {

            DisAssembleBody();

        }
        
    }
    void rigidbodyDie()
    {
        Destroy(parentRobot.gameObject);
        if (dieSound != null)
        {
            AudioSource.PlayClipAtPoint(dieSound, transform.position);
        }

        Destroy(gameObject);
        Transform killed = Instantiate(dead_robot, transform.position, transform.rotation);
        AssignTransformChildParent(transform, killed);
        //Debug.Log("Replace");

    }
    void DisAssembleBody()
    {
        Transform newPos = robotBody.GetComponent<Transform>();
        //Debug.Log(newPos.position);
        parentRobot.SetPositionAndRotation(new Vector3(newPos.position.x, newPos.position.y + 1f, newPos.position.z), new Quaternion(0, newPos.rotation.y, 0, newPos.rotation.w));
        //Debug.Log("p"+parentRobot.position);
        foreach (Transform child in parentRobot)
        {
            Transform curSrc = transform.Find(child.name);
            if (curSrc) AssignTransformChildParent(curSrc, child);
        }

        parentRobot.gameObject.SetActive(true);
        parentRobot.SendMessage("ApplyDamage",(parentRobot.GetComponent<CharHealth>().getCurrentHealth() - GetComponent<robotRigidbodyHealth>().curRBHealth), SendMessageOptions.DontRequireReceiver);
       
       Destroy(gameObject);
    }
    void AssignTransformChildParent(Transform src, Transform dst)
    {
       // Debug.Log(1+" "+src.name + " " + src.position + " " + dst.position);
        dst.position = src.position;
        dst.rotation = src.rotation;
        //Debug.Log(2 + " " + src.name + " " + src.position + " " + dst.position);
        foreach (Transform child in dst)
        {
            Transform curSrc = src.Find(child.name);
            if (curSrc) AssignTransformChildParent(curSrc, child);
        }
    }
    

    
}
