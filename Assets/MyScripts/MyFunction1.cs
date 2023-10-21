using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClass : MonoBehaviour
{
	public static int life = 100;
    // public static void MyStaticFunction()
    // {
        // //int life = 100;
        // Debug.Log("Статическая функция. Значение переменной life: " + life);
    // }
	public static int damage(int hit)
    {
        return life - hit;
        
    }
    void Start()
    {
		if(life==100)
		{
			Debug.Log("Life left: " + damage(10));
		}
         // Вызов статической функции внутри того же класса
    }
	
	void Update()
    {
        transform.position += new Vector3(0, 0.01f, 0);

		if (transform.position.y > 2)
		{
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
    }

}
