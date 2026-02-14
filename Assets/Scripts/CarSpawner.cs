using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
	private float timer = 0.0F;
	private float spawnTime = 0.0F;
	public float minSpawnTime;
	public float maxSpawnTime;

	public List<GameObject> cars;

	public float bufferSize;
	public GameObject obsticleToSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	if (timer > spawnTime) {
		spawnTime = Random.Range(minSpawnTime,maxSpawnTime);
		
		Vector2 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
		transform.position = new Vector3(
				screenDimensions.x+1,
				(int)Random.Range(-screenDimensions.y+bufferSize,screenDimensions.y),
				0
		);

		GameObject car = Instantiate(obsticleToSpawn,transform.position,Quaternion.identity);

		cars.Add(car);
		Car carscript = car.GetComponent<Car>();
		carscript.cars = cars;

		timer = 0;
	}
        timer += Time.deltaTime;
    }
}
