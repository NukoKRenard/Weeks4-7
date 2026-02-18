using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject thingToFire;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire(GameObject target)
    {
	  GameObject projectile = Instantiate(thingToFire);
	  projectile.GetComponent<Arrow>().target = target; 
    }
}
