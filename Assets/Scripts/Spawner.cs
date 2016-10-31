using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject[] pickupPrefab;
    public float spawnDelay;

    private float nextSpawnTime;
    private Transform tf;
    private GameObject spawnedPickup;

	void Start () {
        tf = gameObject.GetComponent<Transform>();
        nextSpawnTime = Time.time + spawnDelay;
	}
	void Update() {
        if (spawnedPickup == null) {
            if (Time.time > nextSpawnTime) {
                spawnedPickup = Instantiate(RandomPickup(), tf.position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }else {
            nextSpawnTime = Time.time + spawnDelay;
        }
	}
    public GameObject RandomPickup() { return pickupPrefab[Random.Range(0, pickupPrefab.Length)]; }
}