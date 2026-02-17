using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Boid : MonoBehaviour
{
    public float speed;
    public float collisionDistance;
    public List<Boid> boids;
    public Boid myScript;
    public float directional_noise;
    public int distance_falloff_exponent;
    public float lifetime;
    void Start()
    {
        myScript = GetComponent<Boid>();
        if (lifetime == -1)
	{
		lifetime *= 100;
	}	
    }
    void Update()
    {
	if (lifetime < 0 && lifetime > -100)
	{
		boids.Remove(myScript);
		Destroy(gameObject);
	}
	lifetime -= Time.deltaTime;
	bool will_collide = false;

       Vector2 collisionsCenter = new Vector2(0,0);
       int collisionsCount = 0;

       Vector2 flockPosition = new Vector2(0,0);
       Vector2 flockHeading = new Vector2(0,0);

       Vector2 screen_size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
       foreach (Boid boid in boids)
       {

	       if (boid != myScript)
	       {
		       Vector2 direction = boid.transform.position-transform.position;
		       float distance = direction.magnitude;
	      
		       //Adds to the average flock position, weighted inversely by the boid's distance.
		       //The weight also has to take into account the portal effect on each side of the screen

		       flockPosition += (Vector2)(direction/boids.Count)*DistanceFalloff(boid,screen_size); 
		       flockHeading += (Vector2)(boid.transform.up/boids.Count)*DistanceFalloff(boid,screen_size);
			       
					
	       

		   if (distance < collisionDistance)
		   {
			   collisionsCount++;
			   collisionsCenter += direction;
			   will_collide = true;
		   }
	       }
       }
       collisionsCenter/=collisionsCount;
       collisionsCenter = collisionsCenter.normalized;
       if (transform.position.y < -screen_size.y && transform.up.y < 0)
       {
	       transform.position = new Vector2(transform.position.x,screen_size.y);
       }
       else if (transform.position.y > screen_size.y && transform.up.y > 0)
       {
	       transform.position = new Vector2(transform.position.x,-screen_size.y);
       }
       if (transform.position.x < -screen_size.x && transform.up.x < 0)
       {
	       transform.position = new Vector2(screen_size.x,transform.position.y);	       
       }
       else if (transform.position.x > screen_size.x && transform.up.x > 0)
       {
	       transform.position = new Vector2(-screen_size.x,transform.position.y);
       }


	       
       
       if (will_collide)
       {
	       HeadAway(collisionsCenter);
       }
       else 
       {
	       HeadTowards(flockPosition);
	       HeadTowards(flockHeading);
       }

       transform.localEulerAngles += new Vector3(0,0,Random.Range(-directional_noise,directional_noise));
       transform.position += transform.up*Time.deltaTime*speed;

    }

    //Turn the boid to head away from a specific point.
    void HeadAway(Vector2 position) {
	    position = -position; 
	    float headingangle = getTurnDirection(position,transform.up);

	    transform.localEulerAngles += new Vector3(0,0,-headingangle*1000)*Time.deltaTime;
    }

    //Turn the boid to head towards a specific point.
    void HeadTowards(Vector2 position) { 
	    float headingangle = getTurnDirection(position,transform.up);

	    transform.localEulerAngles += new Vector3(0,0,-headingangle*1000)*Time.deltaTime;
    }

    float getTurnDirection(Vector2 target, Vector2 angle)
    {
	    angle = angle.normalized;
	    float rawTurn = Vector2.Angle(target,angle)/360;

	    //If turning Clockwise makes the angle smaller turn left.
	    if (
			    Vector2.Dot(RotateVector(target,rawTurn/2),angle) <
			    Vector2.Dot(target,angle)
	       )
	    {
		    return rawTurn;
	    }	    	    
	    //Else turn counter clockwise
	    return -rawTurn;
    }
    
    //I'm assuming we are not allowed to use matrix math, so I wrote a function to do rotate things without matricies.
    Vector2 RotateVector(Vector2 start, float angle)
    {
	    float x = Mathf.Cos(angle)*start.x+Mathf.Sin(angle)*start.y;
	    float y = Mathf.Cos(angle)*start.y-Mathf.Sin(angle)*start.x;

	    return new Vector2(x,y);
    }

    Vector2 DistanceFalloff(Boid boid, Vector2 screen_size)
    {
	    //This part uses a sin function to repeat the distance, so that the weights repeat at the edges.
	    return new Vector2(
			    //Depending on the exponent, Cosine has the ability to return a negative value, this is because if the boid we are calculating for distance is on the bottom of the screen, I want the boid distance equasion to invert it's direction.
			    //This way boids understand the wrapping effect of the game space, and (for example one boid is on the bottom of the screen and one is on the top, instead of taking the long way and going to the middle, they will go to the ends of the screen so they can wrap around to each other.
			    //Unfortunately this seems to make the Boids really want to stick to the edges, I do not know why this bug happens, but it can be fixed by setting the distance exponent to an even number (making all negative numbers positive)
			    /*
			     *I have a theory as to why this happens. The main goal of the boids is to try and flock together, and because the map wraps around they are not only attracted to nearby boids, but also if a boid is on the edge they are attracted to a boid on the opposing edge.
			     * Originally the attraction was negative so that the boid would take the quickest route (off of the side of the frame so it wraps around) to the group of boids.
			     * The weight of a boids position and heading can be imagined as a cube made of two cosine functions, with the period of cone cosine function being twice the size of the screen (in either width or height) So if a boid is at the edge you can imagine that all of the area nearby the boid in a square has a high weight, and will have a large impact on the boid, however the boids on the other side would also have a weight, so the square repeats.
			     * I believe that the reason boids are attracted to the edges when given an odd exponent is because it is at the edges of the screen when a boid has the hights total weight across the screen, and because of the variety of directions the total direction and position is more stable and centered.
			     * */
			    //Technically the boids are not supposed to do this, but it looks pretty so I'm keeping it in.
			    Mathf.Pow((Mathf.Cos(((boid.transform.position.x-transform.position.x)/(2*screen_size.x)*Mathf.PI))),distance_falloff_exponent),
	                    Mathf.Pow((Mathf.Cos(((boid.transform.position.y-transform.position.y)/(2*screen_size.y)*Mathf.PI))),distance_falloff_exponent)
			    );
    }
}
