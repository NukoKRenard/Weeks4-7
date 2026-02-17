using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 

public class BoidController : MonoBehaviour
{
    public float boidSpeed = 5.0f;
    public float moveAwayDistance = 1.0f;
    public GameObject prefab;
    public List<Boid> boids;
    public float noise;
    public int distance_falloff_exponent;
    public Slider NoiseSlider;
    public Slider FalloffSlider;
    public Slider LifeTimeSlider;
    public TMP_Text BoidsCounter; 
    public TMP_Text FrameRate;
    void Start()
    {
	    //Initialises a new list boids use to keep track of each other.
	    boids = new List<Boid>();
        
    }

    void Update()
    {
	    //Sets the value of some text meshes to count the boids and framerate.
	    BoidsCounter.text = "Boids: "+boids.Count;
	    //This is important because the boid algorithm runs in O(n^2)
	    FrameRate.text = "FPS: "+(1/Time.deltaTime);
    }

    //Spawns boids based on a count (default is 10)
    public void CreateBoid(int count) 
    {
	    for (int i = 0; i < count; i ++)
	    {
		    //Each boid needs to keep track of every other boid, so a reference to this list is given to each of them.
		    Boid boid = Instantiate(prefab,transform).GetComponent<Boid>();
		    boids.Add(boid);
		    boid.boids = boids;
		    boid.speed = boidSpeed;
		    boid.collisionDistance = moveAwayDistance;
		    boid.lifetime = LifeTimeSlider.value;
		    boid.directional_noise = noise;
		    boid.distance_falloff_exponent = distance_falloff_exponent;
	    }
    }
    
    //The next few functions are only to change certain paramaters of the flock.
    public void changeBoidNoise()
     {
             noise = NoiseSlider.value;
 
             foreach (Boid boid in boids)
             {
                     boid.directional_noise = noise;
             }
     }

    public void changeExponentFalloff()
    {
	    distance_falloff_exponent = (int)FalloffSlider.value;
	    foreach (Boid boid in boids)
	    {
		    boid.distance_falloff_exponent = distance_falloff_exponent;
	    }
    }
}
