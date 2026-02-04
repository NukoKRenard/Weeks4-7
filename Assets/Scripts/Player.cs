using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if (Keyboard.current.leftArrowKey.isPressed) {
		    transform.position = transform.position + new Vector3(-1,0,0)*Time.deltaTime;
	    }
	    if (Keyboard.current.rightArrowKey.isPressed) {
		    transform.position = transform.position + new Vector3(1,0,0)*Time.deltaTime;
	    }
	    if (Keyboard.current.upArrowKey.isPressed) {
		    transform.position = transform.position + new Vector3(0,1,0)*Time.deltaTime;
	    }
	    if (Keyboard.current.downArrowKey.isPressed) {
		    transform.position = transform.position + new Vector3(0,-1,0)*Time.deltaTime;
	    }
    }
}
