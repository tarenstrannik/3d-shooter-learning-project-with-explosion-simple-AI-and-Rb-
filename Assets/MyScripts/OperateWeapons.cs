using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class operateWeapons : MonoBehaviour
{
    public Rigidbody grenadePrefab;
    private float throwForce=200f;
    public float throwForceStart = 200f;
    public float throwForceMax = 1000f;
    private bool prepareGrenade = false;
    public GameObject currentWeapon;
       private GameObject grenadeCountdown_object;
    // Start is called before the first frame update
    void Start()
    {
        grenadeCountdown_object  = GameObject.Find("grenade_countdown");
         grenadeCountdown_object.GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1")&& GetComponent<CharAmmo>().getCurrentAmmo() > 0)
        {
            currentWeapon.GetComponent<Weapon>().SendMessage("Fire");

        }
            if (GetComponent<CharGrenade>().getCurrentGren() > 0)
        {
            
	      
         if( Input.GetButtonDown("Grenade1")){
            prepareGrenade=true;
            grenadeExplosion.explTime=Random.Range(grenadeExplosion.startexplTimeMin, grenadeExplosion.startexplTimeMax);//grenadeExplosion.startexplTime;
             //Debug.Log("StartTime "+Time.time);
            grenadeCountdown_object.GetComponent<TextMeshProUGUI>().text = ""+(int)(grenadeExplosion.explTime); 
            grenadeCountdown_object.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                throwForce = throwForceStart;
         }
         if(prepareGrenade)
         {
            
            grenadeExplosion.explTime -=Time.deltaTime;
            if(throwForce< throwForceMax) throwForce +=Time.deltaTime*500f;
            grenadeCountdown_object.GetComponent<TextMeshProUGUI>().text = ""+(int)(grenadeExplosion.explTime);
            if((grenadeExplosion.explTime)<2)
            {
                grenadeCountdown_object.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            if((grenadeExplosion.explTime)<1)
             {
                grenadeCountdown_object.GetComponent<TextMeshProUGUI>().text =""+grenadeExplosion.explTime.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
               
             }
             if((grenadeExplosion.explTime)<0)
             {
                GrenadeThrow(0.0f);
               
             }
            //Debug.Log(grenadeExplosion.explTime);
         }
        if( Input.GetButtonUp("Grenade1")){
            if(prepareGrenade)
                {
                GrenadeThrow(throwForce);
            }
        }
        }
    }
    private void GrenadeThrow(float throwForce)
    {
                    //Debug.Log("ThrowTime1 "+Time.time);
        // float parentHeight = transform.localScale.y; // Получаем высоту родителя
                    float parentPuzo = transform.localScale.x/2f; // Получаем высоту родителя
                // float offsetY = parentHeight / 2f; // Вычисляем половину высоты родителя
                    Transform playerCameraRootTransform = transform.Find("PlayerCameraRoot");

                    Vector3 sdvig = new Vector3(parentPuzo,0,parentPuzo);
                    sdvig = playerCameraRootTransform.rotation * sdvig;
                    
                    Vector3 newPosition = playerCameraRootTransform.position+sdvig;
                    Rigidbody grenade = Instantiate(grenadePrefab, newPosition, playerCameraRootTransform.rotation);
                
                    grenade.name = "newgrenade";
                    Vector3 force = new Vector3(0,0,1);
                    force = playerCameraRootTransform.rotation * force;
                    grenade.GetComponent<Rigidbody>().AddForce( force*throwForce) ;
                    //Collider capsuleCollider = GetComponentInChildren<Collider>();
                    //Physics.IgnoreCollision( capsuleCollider, grenade.GetComponent<Collider>(), true );
                    //Physics.IgnoreCollision( transform.root.GetComponent<Collider>(), grenade.GetComponent<Collider>(), true );
                    throwForce=200f;
                    
                    prepareGrenade=false;
        GetComponent<CharGrenade>().SendMessage("SpendGren", 1);
        grenadeCountdown_object.GetComponent<TextMeshProUGUI>().text ="";
    }
}
