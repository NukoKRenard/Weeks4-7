using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public float speed;
    public float collisionDistance;
    public List<Boid> boids;
    public Boid myScript;

    void Start()
    {
        myScript = GetComponent<Boid>();	    
    }
    void Update()
    {
	bool will_collide = false;

       Vector2 collisionsCenter = new Vector2(0,0);
       int collisionsCount = 0;
       foreach (Boid boid in boids)
       {
	       if (boid != myScript)
	       {
	           Vector2 direction = boid.transform.position-transform.position;
		   float distance = direction.magnitude;
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
       if (
				  transform.position.x > 10 ||
				  transform.position.y > 10 ||
				  transform.position.x < 0 ||
				  transform.position.y < 0)
		  {
			  //HeadTowards(-transform.position);
		  }
		  else if (will_collide)
		  {
			  Debug.Log("Will collide with: "+collisionsCenter);
			  HeadAway(collisionsCenter);
		  }

      // transform.position += transform.up*Time.deltaTime*speed;

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
}
