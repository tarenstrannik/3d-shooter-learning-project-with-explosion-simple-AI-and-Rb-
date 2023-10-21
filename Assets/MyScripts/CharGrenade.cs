using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharGrenade : MonoBehaviour
{
    public int startGrenade = 0;
	private  int curGrenade;
	public TextMeshProUGUI textObject; // Ссылка на компонент TextMeshProUGUI
	void Awake()
	{
        curGrenade = startGrenade;
        if (textObject != null)
            textObject.text = ""+(int)curGrenade;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (textObject != null)
            textObject.text = ""+ (int)curGrenade;
    }
    void SpendGren(int ammo)
    {
        if (curGrenade >= 0)
        {
            curGrenade = curGrenade - ammo;
        }
    }
    void TakeGren(int ammo)
    {

        curGrenade = curGrenade + ammo;

    }
   public int getCurrentGren()
    {
        return curGrenade;
    }
}
