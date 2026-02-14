using UnityEngine;
using System.Collections.Generic;

public class FroggerPlayer : MonoBehaviour
{
    public List<GameObject> cars;
    private SpriteRenderer myRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject car in cars) {
		SpriteRenderer spriteRenderer = car.GetComponent<SpriteRenderer>();

	}
    }
}
