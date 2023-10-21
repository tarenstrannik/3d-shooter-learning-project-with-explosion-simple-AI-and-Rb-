using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharAmmo : MonoBehaviour
{
    public int startAmmo = 0;
	private  int curAmmo;
	public TextMeshProUGUI textObject; // Ссылка на компонент TextMeshProUGUI
	void Awake()
	{
        curAmmo = startAmmo;
        if (textObject != null)
            textObject.text = ""+(int)curAmmo;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (textObject != null)
            textObject.text = ""+ (int)curAmmo;
    }
    void SpendAmmo(int ammo)
    {
        if (curAmmo >= 0)
        {
            curAmmo = curAmmo-ammo;
        }
    }
    void TakeAmmo(int ammo)
    {

            curAmmo = curAmmo + ammo;

    }
    public int getCurrentAmmo()
    {
        return curAmmo;
    }
}
