using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class grenadeExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    private float throwTime;
    public static float explTime;
    public static float startexplTime = 4.0f;
    public static float startexplTimeMin = 3.2f;
    public static float startexplTimeMax = 4.2f;

    public float explosionRange = 5.0f;
    public float explosionPower = 5000f;
    public float upwardsForceMod = 0.005f;
    public float explosionDamage = 150f;
    public GameObject explosionPrefab;

    private bool isGrenadeDestroyed = false;
    private bool destroyDelayed = false;
    private bool targetRemoved = false;
    private bool isAssignedenemyRagdollDead = false;
    void Awake()
    {

        //Debug.Log("ThrowTime2 "+Time.time);
        if (explTime < 0)
        {
            GrenadeExpl(transform.position);
            //Debug.Log("ShowTime1 "+Time.time);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        explTime -= Time.deltaTime;
        if (explTime < 0)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            // Ћокальные координаты центра массы
            Vector3 localCenterOfMass = rigidbody.centerOfMass;

            // √лобальные координаты центра массы
            Vector3 globalCenterOfMass = transform.TransformPoint(localCenterOfMass);
            GrenadeExpl(globalCenterOfMass);

            //Debug.Log("ShowTime2 "+Time.time);
        }
    }
    IEnumerator destroyGrenade()
    {
        while (true)
        {
            //Debug.Log(isGrenadeDestroyed);
            if (isGrenadeDestroyed)
            {
                Destroy(gameObject);
                Rigidbody rigidbody = GetComponent<Rigidbody>();

                // Ћокальные координаты центра массы
                Vector3 localCenterOfMass = rigidbody.centerOfMass;

                // √лобальные координаты центра массы
                Vector3 globalCenterOfMass = transform.TransformPoint(localCenterOfMass);
                GameObject GrenadeExplosion = Instantiate(explosionPrefab, globalCenterOfMass, transform.rotation);

                yield break;
            }
            yield return null;
        }
    }
    private void GrenadeExpl(Vector3 explosionPosition)
    {
        if (isGrenadeDestroyed)
        {
            return; // ≈сли ракета уже уничтожена, пропустить обработку
        }
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRange);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>() != null && hit.gameObject != gameObject && !(hit is CharacterController) && (hit is BoxCollider || hit is SphereCollider || hit is CapsuleCollider || hit is MeshCollider))
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
                else if (hit.gameObject.GetComponent<robotPartHealth>() != null && hit.gameObject.GetComponent<robotPartHealth>().robotParent.GetComponent<robotRigidbodyHealth>() != null
                    && hit.gameObject.GetComponent<robotPartHealth>().robotParent.GetComponent<robotRigidbodyHealth>().curRBHealth <= 0)
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
            else if (hit.gameObject != gameObject)
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
        if (!destroyDelayed) isGrenadeDestroyed = true;
        StartCoroutine(destroyGrenade());
    }

    IEnumerator ExplosionForce(Collider hit, Vector3 explosionPosition, string targetType)
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
                //isGrenadeDestroyed = true;
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
                    isGrenadeDestroyed = true;
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
                    isGrenadeDestroyed = true;
                    yield break;
                }
            }
            else if (targetType == "enemyRagdollDeadNoUse")
            {
                //isGrenadeDestroyed = true;
                yield break;
            }
            else if (hit != null)
            {
                //Debug.Log("2" + hit.name);
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRange, upwardsForceMod, ForceMode.Impulse);
                isGrenadeDestroyed = true;
                yield break;
            }
            isGrenadeDestroyed = false;
            yield return null;
        }

    }
    IEnumerator WaitForRemoveParent(GameObject hit)
    {
        while (true)
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
