using UnityEngine;
using System.Collections;

public class planetRotation : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

		transform.Rotate (new Vector3 (0f,2,0f) * Time.deltaTime);
	
	}
}
