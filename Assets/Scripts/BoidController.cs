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
	    boids = new List<Boid>();
        
    }

    void Update()
    {
	    BoidsCounter.text = "Boids: "+boids.Count;
	    FrameRate.text = "FPS: "+(1/Time.deltaTime);
    }

    public void CreateBoid(int count) 
    {
	    for (int i = 0; i < count; i ++)
	    {
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
