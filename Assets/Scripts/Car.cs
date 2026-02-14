using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour
{
    public float speed  = 3.0F;
    public List<GameObject> cars;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
	    transform.rotation = Quaternion.AngleAxis(90,new Vector3(0,0,1));
    }

    // Update is called once per frame
    void Update()
    {
	   transform.position = transform.position + new Vector3(-Time.deltaTime*speed,0,0);
	   
	   Vector2 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));

	   if (transform.position.x < -screenDimensions.x-1)
	   {
		   cars.Remove(gameObject);
		   Destroy(gameObject,1);
	   }
    }
}
