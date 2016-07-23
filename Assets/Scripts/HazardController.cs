using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HazardController : MonoBehaviour {

	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public List<GameObject> hazards;
	public GameObject hazard;
	private GameObject currentHazard;
	private bool willGrow = true;

	void Start () {
		hazards = new List<GameObject> ();
		for (int i = 0; i < hazardCount; i++) {
			GameObject obj = (GameObject)Instantiate (hazard);
			obj.SetActive (false);
			hazards.Add (obj);
		}
		StartCoroutine( SpawnWaves ());
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		while (true) {
			currentHazard = GetPooledObject ();
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range (-spawnValues.y, spawnValues.y), spawnValues.z);
			currentHazard.SetActive (true);
			currentHazard.transform.position = spawnPosition;
			yield return new WaitForSeconds (spawnWait);
		}
	}

	GameObject GetPooledObject() {
		for (int i = 0; i < hazards.Count; i++) {
			if (!hazards [i].activeInHierarchy) {
				return hazards [i];
			} 
		}
		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (hazard);
			hazards.Add (obj);
			return obj;
		}
		return null;
	}
}
