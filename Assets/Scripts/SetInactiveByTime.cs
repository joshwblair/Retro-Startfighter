using UnityEngine;
using System.Collections;

public class SetInactiveByTime : MonoBehaviour {

	// Use this for initialization
	public void OnEnable () {
		StartCoroutine (SetInactive ());
	}
	
	IEnumerator SetInactive() {
		yield return new WaitForSeconds (5f);
		gameObject.SetActive (false);
	}
}
