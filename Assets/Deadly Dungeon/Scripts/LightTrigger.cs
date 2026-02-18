using UnityEngine;
using UnityEngine.Events;

public class LightTrigger : MonoBehaviour
{
    public GameObject player;	
    public SpriteRenderer myRenderer;
    public UnityEvent<GameObject> OnTrapTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myRenderer.bounds.Contains(player.transform.position))
	{
	    OnTrapTrigger.Invoke(player);
	}
    }
}
