using UnityEngine;
using UnityEngine.InputSystem;

public class UIDemo : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update()
	{
		if (Keyboard.current.anyKey.wasPressedThisFrame == true) {
			ChangeColor();
		}	
	}

	void ChangeColor()
	{
		spriteRenderer.color = Random.ColorHSV();
	}
}
