using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	private Vector3 velocity;
	private PlayerController pCont;
	[SerializeField] private float smoothTimeY = 0;
	[SerializeField] private float smoothTimeX = 0;
	public Vector3 minCamPos;
	public Vector3 maxCamPos;
	public Vector3 direction;


	public GameObject player;

	void Start () {
		pCont = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		minCamPos = new Vector3 (pCont.boundary.xMin, pCont.boundary.yMin, this.transform.position.z);
		maxCamPos = new Vector3 (pCont.boundary.xMax, pCont.boundary.yMax, this.transform.position.z);
	}
	// Update is called once per frame
	void FixedUpdate () {
		//print (Screen.width);
		//print (Screen.height);
		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY) + .05f;

		transform.position = new Vector3 (posX, posY, transform.position.z);
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, minCamPos.x+5, maxCamPos.x-5),
			Mathf.Clamp (transform.position.y, minCamPos.y+2f, maxCamPos.y-2f),
			Mathf.Clamp (transform.position.z, minCamPos.z, maxCamPos.z));
	}
}
