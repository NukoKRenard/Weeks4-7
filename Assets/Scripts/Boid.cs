using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public float speed;
    public List<Boid> boids;
    public Boid myScript;

    void Start()
    {
        myScript = GetComponent<Boid>();	    
    }

    void Update()
    {
       foreach (Boid boid in boids)
       {
	       if (boid != myScript)
	       {
	           Vector2 direction = boid.transform.position-transform.position;
		   float distance = direction.magnitude; 
	       }
       }
       transform.position += transform.up*Time.deltaTime*speed; 
    }
}
