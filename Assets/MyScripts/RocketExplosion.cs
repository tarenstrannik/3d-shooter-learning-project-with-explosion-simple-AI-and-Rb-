using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosion;

	public float timeOut = 3.0f;
    public float explosionRange = 5.0f;
    public float explosionPower = 2000f;
    public float explosionDamage = 100f;
    public float upwardsForceMod = 0.5f;
    private bool rocketCollided = false;
    private bool isRocketDestroyed = false;
    private bool destroyDelayed = false;
    private bool targetRemoved = false;
    private bool isAssignedenemyRagdollDead = false;
    //private bool collidersRecieved = false;
    void Start()

    {
        Invoke("destroyRocketTime", timeOut);
    }

    void OnCollisionEnter(Collision collision )
    {
        if (rocketCollided)
        {
            return; // ≈сли ракета уже уничтожена, пропустить обработку
        }
        rocketCollided=true;
        //Debug.Log("Collide");
        ContactPoint contact  = collision.contacts[0];

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);

        GameObject RocketExplosion = Instantiate(explosion, contact.point, rotation);
        RocketExpl(contact.point+ contact.normal * 1.0f);

    }
    void destroyRocketTime()
    {
        GameObject RocketExplosion = Instantiate(explosion, transform.position, transform.rotation);
        RocketExpl(transform.position);
    }

    IEnumerator destroyRocket()
    {
        while (true)
        {
            //Debug.Log(isRocketDestroyed);
            if (isRocketDestroyed)
            {
                ParticleSystem Smoke = transform.Find("Smoke")?.GetComponent<ParticleSystem>();
                ParticleSystem Fire = transform.Find("Fire")?.GetComponent<ParticleSystem>();

                if (Fire) Fire.Stop();
                if (Smoke) Smoke.Stop();

                transform.DetachChildren();

                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
    private void RocketExpl(Vector3 explosionPosition)
    {

       // int i = 0;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRange);
        /*foreach (Collider hit1 in colliders)
        {
            Debug.Log(i+hit1.name);
            i++;
        }*/

        //collidersRecieved = true;
        foreach (Collider hit in colliders)
        {
            //Debug.Log(hit.name+" "+ hit.GetInstanceID());
           
            if (hit.GetComponent<Rigidbody>()!= null && hit.gameObject != gameObject && !(hit is CharacterController) &&  (hit is BoxCollider || hit is SphereCollider || hit is CapsuleCollider || hit is MeshCollider))
            {
                //Debug.Log("1collide " + hit.gameObject.name + " " + hit.gameObject.GetInstanceID() + " " + gameObject.name + " " + gameObject.GetInstanceID());
                Vector3 closestPoint = hit.ClosestPoint(explosionPosition);
                float distance = Vector3.Distance(closestPoint, explosionPosition);
                float hitPoints = 1.0f - Mathf.Clamp01(distance / explosionRange);
                hitPoints *= explosionDamage;



                hit.SendMessage("ApplyDamage", hitPoints, SendMessageOptions.DontRequireReceiver);//
                //if (hit.gameObject.GetComponent<RobotAI>() != null ) hit.gameObject.GetComponent<RobotAI>().persFlight = true;
                //hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);
                destroyDelayed = true;
                string targetType = "";
                if (hit.gameObject.GetComponent<RobotAI>() != null && hit.gameObject.GetComponent<CharHealth>() != null)
                {
                    targetType = "enemy";
                }
                else if(hit.gameObject.GetComponent<robotPartHealth>()!= null && hit.gameObject.GetComponent<robotPartHealth>().robotParent.GetComponent<robotRigidbodyHealth>() != null
                    && hit.gameObject.GetComponent<robotPartHealth>().robotParent.GetComponent<robotRigidbodyHealth>().curRBHealth<=0)
                {
                    if (!isAssignedenemyRagdollDead)
                    {
                        targetType = "enemyRagdollDead";
                        isAssignedenemyRagdollDead = true;
                    }
                    else
                    {
                        targetType = "enemyRagdollDeadNoUse";
                    }
                }
                //Debug.Log(targetType);
                
                StartCoroutine(ExplosionForce(hit, explosionPosition, targetType));
               // Debug.Log(destroyDelayed);


            }
            else if(hit.gameObject != gameObject)
            {
                //Debug.Log("2collide " + hit.gameObject.name + " " + hit.gameObject.GetInstanceID() + " " + gameObject.name + " " + gameObject.GetInstanceID());
                //Debug.Log(hit.name);
                Vector3 closestPoint = hit.ClosestPointOnBounds(explosionPosition);
                float distance = Vector3.Distance(closestPoint, explosionPosition);
                float hitPoints = 1.0f - Mathf.Clamp01(distance / explosionRange);
                hitPoints *= explosionDamage;
                //Debug.Log(hit.name+"damag" +hitPoints);
                hit.SendMessage("ApplyDamage", hitPoints, SendMessageOptions.DontRequireReceiver);
                hit.SendMessageUpwards("torchLit", SendMessageOptions.DontRequireReceiver);
                //Debug.Log(destroyDelayed);


            }
        }
        //Debug.Log("f "+destroyDelayed);
        if (!destroyDelayed) isRocketDestroyed = true;
        StartCoroutine(destroyRocket());

        
    }
    IEnumerator ExplosionForce(Collider hit,Vector3 explosionPosition,string targetType)
    {
        //Debug.Log("EX1");
        while (true)
        {
            //Debug.Log(targetType);
            if (targetType == "enemy")
            {
                if (hit != null) hit.gameObject.GetComponent<RobotAI>().persFlight = true;
                //if (hit.gameObject.GetComponent<CharHealth>() != null && hit.gameObject.GetComponent<CharHealth>().getCurrentHealth() > 0)

                //hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);
                //isRocketDestroyed = true;
                //yield break;
                //}
                //else if (hit.gameObject.GetComponent<CharHealth>() != null )
                //{
                StartCoroutine(hit != null ? WaitForRemoveParent(hit.gameObject) : WaitForRemoveParent(null));
                if (targetRemoved)
                {
                    //Debug.Log(1+hit.name);
                    Collider[] newallColliders = Physics.OverlapSphere(explosionPosition, explosionRange);
                    List<Collider> newcolliders = new List<Collider>();

                    foreach (Collider newcollider in newallColliders)
                    {
                        if (newcollider.GetComponent<Collider>() != null && newcollider.gameObject != this.gameObject && !(newcollider is CharacterController) && (newcollider is BoxCollider || newcollider is SphereCollider || newcollider is CapsuleCollider || newcollider is MeshCollider))
                        {
                            newcolliders.Add(newcollider);
                        }
                    }
                    foreach (Collider newhit in newcolliders)
                    {
                        //Debug.Log(hit.name+" "+ hit.GetInstanceID());
                        if (newhit.GetComponent<Rigidbody>() != null && newhit != this.GetComponent<Collider>())
                        {
                            //Debug.Log(newhit.name);
                            newhit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);

                        }
                    }
                    isRocketDestroyed = true;
                    yield break;
                }

            }
            else if (targetType == "enemyRagdollDead")
            {
               // Debug.Log(hit.name + " " + hit.GetInstanceID()+" "+ targetRemoved);
                StartCoroutine(hit != null ? WaitForRemoveParent(hit.gameObject) : WaitForRemoveParent(null));
                //Debug.Log("T" + targetRemoved);
                if (targetRemoved)
                {
                   // Debug.Log("T"+targetType);
                    Collider[] newallColliders = Physics.OverlapSphere(explosionPosition, explosionRange);
                    List<Collider> newcolliders = new List<Collider>();

                    foreach (Collider newcollider in newallColliders)
                    {
                        if (newcollider.GetComponent<Collider>() != null && newcollider.gameObject != this.gameObject && !(newcollider is CharacterController) && (newcollider is BoxCollider || newcollider is SphereCollider || newcollider is CapsuleCollider || newcollider is MeshCollider))
                        {
                            newcolliders.Add(newcollider);
                        }
                    }
                    foreach (Collider newhit in newcolliders)
                    {
                        //Debug.Log(hit.name+" "+ hit.GetInstanceID());
                        if (newhit.GetComponent<Rigidbody>() != null && newhit != this.GetComponent<Collider>())
                        {
                           // Debug.Log(newhit.name);
                            newhit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);

                        }
                    }
                    isRocketDestroyed = true;
                    yield break;
                }
            }
            else if(targetType == "enemyRagdollDeadNoUse")
            {
                //isRocketDestroyed = true;
                yield break;
            }
            else if (hit != null)
            {
                //Debug.Log("2" + hit.name);
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);
                isRocketDestroyed = true;
                yield break;
            }
            isRocketDestroyed = false;
            yield return null;        
        }


    }
    IEnumerator WaitForRemoveParent(GameObject hit)
    {
        while(true)
        {
            //Debug.Log("testremoved!"+hit);
            if (hit == null || !hit.gameObject.activeSelf)
            {
                //Debug.Log("removed!"+ targetRemoved);
                targetRemoved = true;
                //Debug.Log("removed!" + targetRemoved);
                yield break;
            }
            yield return null;
        }
    }


}
