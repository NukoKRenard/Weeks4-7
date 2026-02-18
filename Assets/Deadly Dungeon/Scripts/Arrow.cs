using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    public EventDrivenLara targetScript;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetScript = target.GetComponent<EventDrivenLara>();
    }

    // Update is called once per frame
    void Update()
    {
	    transform.position += transform.up*Time.deltaTime;

    }
}
