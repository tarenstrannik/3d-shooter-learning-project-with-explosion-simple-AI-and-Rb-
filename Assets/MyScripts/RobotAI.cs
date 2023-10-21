using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(AudioSource))]

public class RobotAI : MonoBehaviour
{
    public float runSpeed = 3.0f;
    public float searchTimeout = 3.0f;
    public float walkSpeed = 1.0f;
    public float speed = 1.0f;
    public float rotationSpeed = 5.0f;
    public float GetWPDistance = 2.0f;
    public Transform Eyes;
   public Transform target ;
    public GameObject currentWeapon;
    public float seeRange = 10.0f;
    public float closeRange = 5.0f;

    public float shootRange = 15.0f;
    public float shootAngle = 4.0f;
    public float shootDelay = 0.7f;

    //public float robotHealth = 300.0f;
    public AudioClip dieSound;


    public bool persFlight = false;
    //public float deltaTnoFalling = 0.1f;
   // public float minSpeed = 0.1f;

    public Transform dead_robot;
    public Transform ragdoll_robot;
    public bool isReplaced = false;

    private List<TransformData> childTransforms;
    public float sborkaDuration=1.0f;
    private bool assemblyFinished = true;
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        childTransforms = new List<TransformData>();
        //StartCoroutine("Patrol");
    }
    void OnEnable()
    {
        if(!persFlight)
        { 
            StartCoroutine("Patrol"); 
        }
        else 
        {
            speed = walkSpeed;
            persFlight = false;
            // StartCoroutine(AssembleBody());
            AssembleBody();
            StartCoroutine("Patrol");
        }
        
        
    }
    IEnumerator Patrol()
    {
        robotWaypoint curWayPoint = robotWaypoint.FindNearestPoint(transform.position);
        while (true)
        {
            if (assemblyFinished)
            {
                Vector3 waypointPosition = curWayPoint.transform.position;
                MoveTo(waypointPosition);
                // Достигли контрольной точки - ищем следующую
                if (Vector3.Distance(waypointPosition, transform.position) < GetWPDistance)
                    curWayPoint = GetNextWaypoint(curWayPoint);
                if (SeePlayer())
                {
                    yield return StartCoroutine(GoToPlayer());
                    curWayPoint = robotWaypoint.FindNearestPoint(transform.position);
                }
                if (persFlight) charFlight();
            }
                yield return null;
            
        }
    }

    bool SeePlayer() {
        if (target.parent.GetComponent<CharHealth>().getCurrentHealth() > 0)
        {
            Vector3 closestPointEyesToTarget = Eyes.transform.position;
            Vector3 closestPointTargetToEyes = target.GetComponent<Collider>().ClosestPoint(Eyes.transform.position);
            //Debug.Log(closestPointEyesToTarget + " " + closestPointTargetToEyes+" " + Vector3.Distance(closestPointEyesToTarget, closestPointTargetToEyes));
            if (Vector3.Distance(closestPointEyesToTarget, closestPointTargetToEyes) <= seeRange)
            {
                // Debug.Log("close"+closestPointEyesToTarget + " " + closestPointTargetToEyes + " " + Vector3.Distance(closestPointEyesToTarget, closestPointTargetToEyes));

                Vector3 directionToTarget = closestPointTargetToEyes - closestPointEyesToTarget;
                // Vector3 hordirectionToTarget = directionToTarget;
                //Vector3 verdirectionToTarget = directionToTarget;
                //hordirectionToTarget.y = 0;
                //verdirectionToTarget.x = 0;
                //verdirectionToTarget.z = 0;
                // Debug.Log(hordirectionToTarget + " " + verdirectionToTarget);
                //float angleToTarget = Vector3.Angle(Eyes.forward, hordirectionToTarget);
                //float vertangleToTarget = Vector3.Angle(Eyes.forward, verdirectionToTarget);
                //Debug.Log(angleToTarget + " " + vertangleToTarget);
                // Проверка горизонтального угла
                //if (angleToTarget <= 90f)
                //{
                // Проверка вертикального угла

                // if (vertangleToTarget <= 60f && vertangleToTarget >= -60f)
                //{
                RaycastHit hit;
                if (Physics.Raycast(closestPointEyesToTarget, directionToTarget, out hit, seeRange))
                {
                    // Debug.Log(hit.transform.name + closestPointEyesToTarget + " " + closestPointTargetToEyes + " " + Vector3.Distance(closestPointEyesToTarget, hit.point));
                    if (hit.transform == target)
                    {
                        return true;
                    }
                }
                // }
                //}

                /*RaycastHit hit;
           if (Physics.Linecast (transform.position, target.position, out hit)){
               if(hit.transform == target ) return true;
           }*/
            }
        }
	return false;
}
    IEnumerator GoToPlayer()
    {
        Vector3 lastPlayerPos = target.position;
        while (true)
        {
            if (SeePlayer())
            {
                
                // Игрок удалился - преследование прекратить
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < seeRange * 2)
                {
                    lastPlayerPos = target.position;
                    if (distance > closeRange)
                    {
                        speed = runSpeed;
                        SendMessage("SetSpeed", speed);
                        MoveTo(lastPlayerPos);
                    }
                        
                    else
                    {
                        StopMove();
                        RotateTo(lastPlayerPos);
                        yield return StartCoroutine(Shoot());
                    }

                }
                else yield break;

            }
            else
            {
                speed = walkSpeed;
                SendMessage("SetSpeed", speed);
                yield return StartCoroutine(SearchPlayer(lastPlayerPos));
                if (!SeePlayer())
                    yield break;
            }
            if (persFlight) charFlight();
            yield return null;
        }
    }
    IEnumerator  Shoot()
    {
        float startShootTime = shootDelay;
        
        
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0;
        float distance = Vector3.Distance(transform.position, target.position);
        float angle = Vector3.Angle(targetDirection, transform.forward);
        // Начинаем стрельбу
        if (distance < shootRange && angle < shootAngle)
        {
            //Debug.Log(angle);
            GetComponent<Animation>().CrossFade("shoot");
            // Задержка на проигрывание анимации стрельбы
            while (true)
            {
                if (persFlight) charFlight();
                startShootTime -= Time.deltaTime;
                if (startShootTime<=0)
                {
                    currentWeapon.GetComponent<Weapon>().SendMessage("Fire");
                    yield break;
                }
                    yield return null;
            }
            
            //взрыв разворачивает робота за время задержки и тот успевает выстрелить в не туда. это хорошо или нет?
            //distance = Vector3.Distance(transform.position, target.position);
            //targetDirection = target.position - transform.position;
            //targetDirection.y = 0;
            //angle = Vector3.Angle(targetDirection, transform.forward);
            //if (distance < shootRange && angle < shootAngle)
            //{
            
            
            //};
        }
        
    }

    void RotateTo(Vector3 position)
    {
        speed = 0.0f;
        SendMessage("SetSpeed", speed);
        Vector3 direction = position - transform.position;
        direction.y = 0;
        if (direction.magnitude >= 0.1)
        {
            
            // Поворачиваемся к игроку
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
           // transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
    void charFlight()
    {
        if(gameObject.activeSelf && GetComponent<CharHealth>().getCurrentHealth() > 0)
        {
           
            Transform rd_robot = Instantiate(ragdoll_robot, transform.position, transform.rotation);
            AssignTransformChild(transform, rd_robot);
            rd_robot.gameObject.GetComponent<robotRagdoll>().parentRobot = transform;
            rd_robot.gameObject.GetComponent<robotRigidbodyHealth>().curRBHealth = GetComponent<CharHealth>().getCurrentHealth();
            if (assemblyFinished)
            {
               
                // Рекурсивно перебираем дочерние объекты и сохраняем их трансформы
                CollectChildTransforms(transform, childTransforms);
            }
            
            gameObject.SetActive(false);
            
        }
        
    }

    IEnumerator SearchPlayer( Vector3 position)
    {
        // Идем к последней видимой позиции игрока, но через 1 сек. возвращаемся к патрулированию
        float timeout = searchTimeout;
        while (timeout > 0.0)
        {
            MoveTo(position);

            // Видим игрока
            if (SeePlayer())
            {
               
                yield break;
            }
            if (persFlight) charFlight();//yield return StartCoroutine(Flight());
            timeout -= Time.deltaTime;
            yield return null;
        }
        
    }
    void StopMove()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    void MoveTo(Vector3 position )
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        Vector3 direction = position - transform.position;
        direction.y = 0;
        // поворачиваемся к новой точке

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        // Двигаем робота, то есть его компонент CharacterController 
        //direction = forward * speed;
        //GetComponent<CharacterController>().SimpleMove(direction);
       
        rb.velocity = forward * speed;

        SendMessage("SetSpeed", speed);
    }


    robotWaypoint GetNextWaypoint(robotWaypoint currentWaypoint)
    {
        // Ищем точку, где роботу не придется слишком сильно поворачивать в сторону, противоположную направлению
        // его предыдущего перемещения
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // Чем ближе расположены вектора, тем больше значение возвращаемое функцией Dot
        robotWaypoint best = currentWaypoint;
        float bestDot = -10.0f;
        foreach (robotWaypoint cur  in currentWaypoint.connected)
        {
            Vector3 direction = Vector3.Normalize(cur.transform.position - transform.position);
            var dot = Vector3.Dot(direction, forward);
            if (dot > bestDot && cur != currentWaypoint)
            {
                bestDot = dot;
                best = cur;
            }
        }
        return best;
    }

    void Die()
    {
        
                if (dieSound != null)
                {
                    AudioSource.PlayClipAtPoint(dieSound, transform.position);
                }
                
                Destroy(gameObject);
        Transform killed = Instantiate(dead_robot, transform.position, transform.rotation);
        AssignTransformChild(transform, killed);
        //Debug.Log("Replace");
        
    }
    void  AssignTransformChild(Transform src , Transform dst )
    {
        dst.position = src.position;
        dst.rotation = src.rotation;
        foreach (Transform child   in dst)
        {
            Transform curSrc = src.Find(child.name);
            if (curSrc) AssignTransformChild(curSrc, child);
        }
    }
    // Start is called before the first frame update
    void AssembleBody()
    {
        //Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //transform.position = newPos;
        //parentRobot.rotation = robotBody.rotation;
        foreach (Transform child in transform)
        {
            //Debug.Log(child.name);
            Vector3 curSrcPos = childTransforms.Find(transformData => transformData.name == child.name).position;
            Quaternion curSrcRot = childTransforms.Find(transformData => transformData.name == child.name).rotation;
            if (curSrcPos!=null&& curSrcRot!=null)
            {
                //Debug.Log(child.name+ child.position+"f" + curSrcPos);
                AssignTransformChildPlavno(curSrcPos, curSrcRot, child);
            }

        }
       // yield return null;
        /*foreach (Transform child in parentRobot)
        {
            Transform curSrc = transform.Find(child.name);
            if (curSrc) AssignTransformChildParent(curSrc, child);
        }*/

        //yield return new WaitForSeconds(sborkaDuration);

    }
    void AssignTransformChildPlavno(Vector3 srcPos, Quaternion srcRot, Transform dst)
    {
        //Debug.Log(dst.name);

        StartCoroutine(LerpPositionCoroutine(dst.localPosition, srcPos, dst));
        // dst.position = src.position;
        // dst.rotation = src.rotation;
        StartCoroutine(SlerpRotationCoroutine(dst.localRotation, srcRot, dst));
        foreach (Transform child in dst)
        {
            Vector3 curSrcPos = childTransforms.Find(transformData => transformData.name == child.name).position;
            Quaternion curSrcRot = childTransforms.Find(transformData => transformData.name == child.name).rotation;
            if (curSrcPos != null && curSrcRot != null)
                
                // AssignTransformChildPlavno(curSrcPos, curSrcRot, child);
                AssignTransformChildPlavno(curSrcPos, curSrcRot, child);
        }
    }

    private IEnumerator LerpPositionCoroutine(Vector3 startPosition, Vector3 destinationPosition, Transform dst)
    {
       
        float elapsedTime = 0f;

        while (elapsedTime < sborkaDuration)
        {
            if (persFlight) charFlight();
            assemblyFinished = false;
            elapsedTime += Time.deltaTime;

            // Интерполируем позицию объекта
            float t = Mathf.Clamp01(elapsedTime / sborkaDuration);
            dst.localPosition = Vector3.Lerp(startPosition, destinationPosition, t);
            //Debug.Log(dst.name + " " + dst.position);
            yield return null;
        }

        // Устанавливаем конечную позицию точно
        dst.localPosition = destinationPosition;
        assemblyFinished = true;
    }

    private IEnumerator SlerpRotationCoroutine(Quaternion startRotation, Quaternion destinationRotation, Transform dst)
    {
        
        float elapsedTime = 0f;

        while (elapsedTime < sborkaDuration)
        {
            if (persFlight) charFlight();
            assemblyFinished = false;
            elapsedTime += Time.deltaTime;

            // Интерполируем поворот объекта
            float t = Mathf.Clamp01(elapsedTime / sborkaDuration);
            dst.localRotation = Quaternion.Slerp(startRotation, destinationRotation, t);

            yield return null;
        }

        // Устанавливаем конечный поворот точно
        dst.localRotation = destinationRotation;
        assemblyFinished = true;
    }


    private void CollectChildTransforms(Transform parentTransform, List<TransformData> childTransforms)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            Vector3 localPosition = parentTransform.InverseTransformPoint(childTransform.position);
            Quaternion localRotation = Quaternion.Inverse(parentTransform.rotation) * childTransform.rotation;
            TransformData transformData = new TransformData(childTransform.name, localPosition, localRotation);
            childTransforms.Add(transformData);
            //TransformData transformData = new TransformData(childTransform.name, childTransform.position, childTransform.rotation);
           
            //Debug.Log(childTransform.name + childTransform.transform.position + "s");

            // Рекурсивно вызываем CollectChildTransforms для каждого дочернего объекта
            CollectChildTransforms(childTransform, childTransforms);
        }
    }

    // Класс для хранения данных о трансформе
    private class TransformData
    {
        public string name { get; }
        public Vector3 position { get; }
        public Quaternion rotation { get; }

        public TransformData(string Name, Vector3 pos,Quaternion rot)
        {
            name = Name;
            position = pos;
            rotation = rot;
        }
    }
}
