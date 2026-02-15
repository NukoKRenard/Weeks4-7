using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{
    public float boidSpeed = 5.0f;
    public float moveAwayDistance = 1.0f;
    public GameObject prefab;
    public List<Boid> boids;

    void Start()
    {
	    boids = new List<Boid>();
        
    }

    void Update()
    {
	    Debug.Log(Mathf.Atan2(transform.up.x,transform.up.y));
    }

    public void CreateBoid() 
    {
	    Boid boid = Instantiate(prefab,transform).GetComponent<Boid>();

	    boids.Add(boid);
	    boid.boids = boids;
	    boid.speed = boidSpeed;
	    boid.collisionDistance = moveAwayDistance;
    }

}
