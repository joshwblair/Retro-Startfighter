using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour {

	public GameController gameController;
	public GameObject enemy;
	public GameObject EnemySuicideBomber;
	public GameObject EnemyStandard;
	public GameObject EnemyBomb;
	public Vector3 spawnValues;
	public int waveNumber;
	public int enemyCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int enemyAlive;
	public int enemySpawned;
	public int totalWaves;

	public List<GameObject> enemies;
	private bool willGrow = true;
	private GameObject currentEnemy;
	public bool waveHasStarted;

	void Start () {
		enemyAlive = 0;
		enemySpawned = 0;
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		enemies = new List<GameObject> ();
		for (int i = 0; i < enemyCount; i++) {
			float enemyChoice = Random.Range (0, 10);
			if (enemyChoice < 4) {
				enemy = EnemyStandard;
			} else if (enemyChoice >= 4 && enemyChoice < 8) {
				enemy = EnemySuicideBomber;
			} else {
				enemy = EnemyBomb;
			}
			GameObject obj = (GameObject)Instantiate (enemy);
			obj.GetComponent<EnemyController> ().SetGameController ();
			obj.SetActive (false);
			enemies.Add (obj);
		}
		waveHasStarted = false;
		StartCoroutine( SpawnWaves ());
	}

	void Update () {
		if (CheckEndWave () && waveHasStarted) {
			gameController.UpdateWaveText ("Wave " + this.waveNumber + " Complete");
			gameController.waveNumber++;
			waveHasStarted = false;
			StartCoroutine (SpawnWaves ());
		}
	}

	IEnumerator SpawnWaves(){
		enemySpawned = 0;
		enemyCount += (int)(waveNumber * enemyCount/10f);
		totalWaves = gameController.totalWaves;
		this.waveNumber = gameController.waveNumber;
		yield return new WaitForSeconds (startWait);
		if (waveNumber > totalWaves) {
			waveHasStarted = false;
			gameController.SetIsWin();
			yield return new WaitForSeconds (startWait/2f);
		} else {
			gameController.UpdateWaveText ("Wave " + waveNumber + " Incoming");
			while (enemySpawned < enemyCount) {
				currentEnemy = GetPooledEnemy ();
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.y, spawnValues.y), spawnValues.z);
				RaycastHit hit;
				if (Physics.SphereCast (spawnPosition, 1f, -transform.forward, out hit, 200f) == false) {
					currentEnemy.SetActive (true);
					if (currentEnemy.CompareTag("Enemy")) {
						currentEnemy.GetComponent<EnemyFire> ().startFire ();
					}
					currentEnemy.transform.position = spawnPosition;
					if (currentEnemy.CompareTag("Enemy")) {
						currentEnemy.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, -10f);
					} 
					if (currentEnemy.CompareTag("Bomber")) {
						currentEnemy.GetComponent<EnemySuicideBomber> ().Start ();
					}
					if (currentEnemy.CompareTag ("Exploder")) {
						//print ("Exploder");
					}
					yield return new WaitForSeconds (spawnWait);
					enemySpawned++;
					enemyAlive += 1;
				}
				waveHasStarted = true;

				/*for (int i = 0; i < enemyCount; i++){
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.y, spawnValues.y), spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				GameObject enem = Instantiate (enemy, spawnPosition, spawnRotation)  as GameObject;
				enem.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, -10);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);*/
			}
		}

	}

	public bool CheckEndWave() {
		if (enemyAlive == 0) {
			return true;
		}
		return false;
	}

	void OnTriggerEnter( Collider c){
		if (c.gameObject.tag == "Enemy") {
			c.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		}
	}

	GameObject GetPooledEnemy () {
		for (int i = 0; i < enemies.Count; i++) {
			if (!enemies [i].activeInHierarchy) {
				return enemies [i];
				}
		}
		if (willGrow) {
			float enemyChoice = Random.Range (0, 10);
			if (enemyChoice < 4) {
				enemy = EnemyStandard;
			} else if (enemyChoice >= 4 && enemyChoice < 8) {
				enemy = EnemySuicideBomber;
			} else {
				enemy = EnemyBomb;
			}
			GameObject obj = (GameObject)Instantiate (enemy);
			obj.GetComponent<EnemyController> ().SetGameController ();
			obj.SetActive (false);
			enemies.Add (obj);
			return obj;
		}
		return null;
	}
}
