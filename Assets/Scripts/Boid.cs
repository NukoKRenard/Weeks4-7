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
	//If the slider is set to negative one the boids will live forever, this just ensures that DeltaTime would never accidentally make a boid immortal (In cases of large lag)
        myScript = GetComponent<Boid>();
        if (lifetime == -1)
	{
		lifetime *= 100;
	}	
    }
    void Update()
    {
	//Lifetime timer code
	//If the lifetime has expired, delete the boid.
	if (lifetime < 0 && lifetime > -100)
	{
		boids.Remove(myScript);
		Destroy(gameObject);
	}
	lifetime -= Time.deltaTime;
	
	
	bool will_collide = false;
	Vector2 collisionsCenter = new Vector2(0,0);
	int collisionsCount = 0;
	
	//The average heading on the flock, weighted by each boids distance.
	Vector2 flockPosition = new Vector2(0,0);

	//The average heading of the flock, weighted by each boid's distance.
	Vector2 flockHeading = new Vector2(0,0);

	//Screen size
	Vector2 screen_size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
	foreach (Boid boid in boids)
       {
	       //If we compare against ourselves our equasions divide by zero, and we don't want that.
	       if (boid != myScript)
	       {
		       //The direction to the boid
		       Vector2 direction = boid.transform.position-transform.position;
		       //The distance to the boid.
		       float distance = direction.magnitude;
	      
		       //Adds to the average flock position, weighted inversely by the boid's distance.
		       //The weight also has to take into account the portal effect on each side of the screen
		       //Adds the boids position to the average position of the flock, weighted inversely by it's distance.
		       flockPosition += (Vector2)(direction/boids.Count)*DistanceFalloff(boid,screen_size); 
		       //Adds teh boids direction to the average direction of the flock, weighted inversely by it's distance.
		       flockHeading += (Vector2)(boid.transform.up/boids.Count)*DistanceFalloff(boid,screen_size);
			       
		   //If the boid is about to collide with another boid it adds that boid position to an average in order to veer away from.
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
       //The next few branching statements detect if a boid is outside of the bounds of the map, and is heading outside of the map.
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


	       
       //If the boid is about to colide with another, then that takes priority, and the boid will veer away.
       if (will_collide)
       {
	       HeadAway(collisionsCenter);
       }
       //Otherwise it moves with the rest of the flock.
       else 
       {
	       HeadTowards(flockPosition);
	       HeadTowards(flockHeading);
       }

       //Injects some noise into the boids direction.
       transform.localEulerAngles += new Vector3(0,0,Random.Range(-directional_noise,directional_noise));

       //Moves the boid forwards by a speed value.
       transform.position += transform.up*Time.deltaTime*speed;

       //Mouse interaction
       Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
       //If the mouse left mouse button is clicked, the boids should move towards the mouse.
       if (Mouse.current.leftButton.isPressed)
       {
	      HeadTowards(mousePos); 
       }
       //If the right mouse button is clicked, the boids should move away from the mouse.
       else if (Mouse.current.leftButton.isPressed)
       {
	      HeadAway(mousePos); 
       }

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

    //Gets how to turn in order to head towards a specific point.
    //(This was the hardest part, I would have used matrix or quaternion  math, however I was unsure if I was allowed, so I just used sine and cosine)
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
