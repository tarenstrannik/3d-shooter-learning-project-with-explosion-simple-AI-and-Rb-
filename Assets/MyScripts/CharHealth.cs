using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Collider))]
public class CharHealth : MonoBehaviour
{
    public float startHealth = 100.0f;
	private float curHealth;
	public TextMeshProUGUI textObject; // Ссылка на компонент TextMeshProUGUI
	void Awake()
	{
        curHealth = startHealth;
        if(textObject!=null)
        textObject.text = ""+(int)curHealth;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name+" "+curHealth);
        //Debug.Log(curHealth);
        if (textObject != null)
            textObject.text = ""+ (int)curHealth;
        if (curHealth <= 0)
        {
            //Debug.Log("Death");
            SendMessageUpwards("Die", SendMessageOptions.DontRequireReceiver);
        }
    }
    void ApplyDamage(float damage)
    {
       // Debug.Log(name+curHealth);
        if (curHealth >= 0.0)
        {
            

            curHealth -= (damage);
            if (curHealth <= 0.0)
            {
                curHealth = 0;
            }
            
            //Debug.Log(curHealth);
        }
    }
    public float getCurrentHealth()
    {
        return curHealth;
    }
}
