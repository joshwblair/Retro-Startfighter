using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
	public float tumble;
	public Vector3 thisVelocity;

	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble; 
	}

	void FixedUpdate(){
		if (!gameObject.CompareTag("SkyboxCamera")){
			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3(0.0f, 0.0f, -75.0f);
		}
		thisVelocity = gameObject.GetComponent<Rigidbody> ().angularVelocity;
	}
}