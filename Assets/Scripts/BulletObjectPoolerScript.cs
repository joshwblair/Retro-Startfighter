using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletObjectPoolerScript : MonoBehaviour {
	
	public static BulletObjectPoolerScript current;
	public GameObject pooledEnemyBomb;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;

	List<GameObject> pooledObjects;
	List<GameObject> pooledBombs;

	void Awake() {
		current = this;
	}

	void Start () {
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}

		pooledBombs = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pooledEnemyBomb);
			obj.SetActive (false);
			pooledBombs.Add (obj);
		}
	}

	public GameObject GetPooledObject() {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObjects.Add (obj);
			return obj;
		} 

		return null;
	}

	public GameObject GetPooledBomb() {
		for (int i = 0; i < pooledBombs.Count; i++) {
			if (!pooledBombs [i].activeInHierarchy) {
				return pooledBombs [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledEnemyBomb);
			pooledBombs.Add (obj);
			return obj;
		}

		return null;
	}

}
