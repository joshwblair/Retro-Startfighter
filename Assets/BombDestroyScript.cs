using UnityEngine;
using System.Collections;

public class BombDestroyScript : MonoBehaviour {
	void OnTriggerEnter (Collider c) {
		if (c.CompareTag ("Player")) {
			gameObject.SetActive (false);
		}
	}
}
