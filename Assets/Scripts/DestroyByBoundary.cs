using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
	
	void OnTriggerExit(Collider other)
	{
		other.gameObject.SetActive (false);
	}


}
