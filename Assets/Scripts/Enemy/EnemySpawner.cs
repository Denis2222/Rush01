using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public int 					maxEnemySpawn;
	public float				timePerSpawn;
	public List<GameObject>		spawnables;
	public List<GameObject>		spawneds;

	private float			lastSpawn;
	void Start ()
	{
		
	}

	void spawnRandomEnemy()
	{
		int toSpawn = Random.Range (0, spawnables.Count);
		GameObject enemy = GameObject.Instantiate (spawnables[toSpawn]);
		enemy.transform.position = this.transform.position;
		print("Enemy spawn");
		//enemy.GetComponent<EnemyController> ().isWalking = true; // walking slowly to player

		spawneds.Add(enemy);
		this.lastSpawn = Time.fixedTime;
	}

	void Update ()
	{
		float diff = Time.fixedTime - lastSpawn;
		if (diff > timePerSpawn) {
			if (spawneds.Count < maxEnemySpawn) {
				spawnRandomEnemy ();
			}
		}
	}
}
