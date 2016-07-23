using UnityEngine;
using System.Collections;

public class HazardDestory : MonoBehaviour {

	public GameObject asteroidExplosion;

	void OnTriggerEnter(Collider c) {
		if(c.CompareTag("Player") || c.CompareTag("Enemy") || c.CompareTag("Projectile")) {
			if (asteroidExplosion != null) {
				Instantiate (asteroidExplosion, transform.position, transform.rotation);
			}
			gameObject.SetActive (false);
		}
	}
}
