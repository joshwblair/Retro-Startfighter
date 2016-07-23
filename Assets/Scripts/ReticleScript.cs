using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Boundary2 {
	public float xMin, xMax, yMin, yMax;
	public Vector3 origin, extent;
	public Rect reticleRect;
}
public class ReticleScript : MonoBehaviour {
	
	[SerializeField] private GameObject sphere;
	[SerializeField] private float radius;
	[SerializeField] private GameObject reticleBounds;
	[SerializeField] private GameObject player;
	private Vector2 mousePosition;
	public Vector3 reticleBoundsOffset;
	public Boundary2 boundary;
	public Vector3 limitCenter, tempPosition;
	private Bounds bounds;
	public float MouseX, MouseY;
	public bool enemyTarget;

	public Vector3 point;
	public GameObject gunBarrel;
	public GameObject projectile;
	public float speed;
	public Vector3 target, farPoint, direction;

	public float fireRate;
	private float nextFire;

	public SpriteRenderer crosshair;

	public GameObject currentPlayerTarget;

	void Start () {
		//Cursor.visible = false;
		//Time.timeScale = 0.25f;
		crosshair = gameObject.GetComponent<SpriteRenderer> ();
		Vector3 mPos = Input.mousePosition;
		mPos.z = this.transform.position.z - Camera.main.transform.position.z;
		this.transform.position = Camera.main.ScreenToWorldPoint (mPos);
		radius = 2f;
		nextFire = 0f;
	}

	void Update () {

		MouseX = 0.25f * Input.GetAxisRaw ("Mouse X");
		MouseY = 0.25f * Input.GetAxisRaw ("Mouse Y");
		//Positioning reticle bounds and reticle
		reticleBounds.transform.position = player.transform.position + reticleBoundsOffset;
		this.transform.position += new Vector3 (MouseX, MouseY, 0f);

		//Ensuring reticle stays within bounds
		tempPosition = this.transform.position;
		limitCenter = this.transform.parent.gameObject.transform.position;
		tempPosition -= limitCenter;
		if (tempPosition.magnitude > radius)
			tempPosition = tempPosition.normalized * radius;
		this.transform.position = tempPosition + limitCenter;

		//targetting
		Vector3 origin = gunBarrel.transform.position;
		direction = this.transform.position - origin;
		farPoint = (this.transform.position - Camera.main.transform.position) * 500; //Grabs a point far in the distance in the direction of ray from camera to reticle
		Debug.DrawLine (gunBarrel.transform.position, farPoint, Color.magenta);

		//Ray cast to center of reticle from the camera
		Ray mouseTarget = new Ray (Camera.main.transform.position, ((this.transform.position) - Camera.main.transform.position) * 100);

		//Ray cast from front of ship to the reticle
		Ray bolt = new Ray (origin, direction);

		point = bolt.origin + (bolt.direction * 500); //Grabs a point far in the distance in the direction of the bolt.
		RaycastHit hit;
		if (Physics.SphereCast (Camera.main.transform.position, .5f,(this.transform.position) - Camera.main.transform.position, out hit, 200f)) {
			if (hit.collider.CompareTag ("Enemy") || hit.collider.CompareTag("Bomber") || hit.collider.CompareTag("Exploder")) {
				gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
				target = hit.collider.gameObject.transform.position - gunBarrel.transform.position;
				if (Input.GetButton ("Fire1")) {
					currentPlayerTarget = hit.collider.gameObject;
				}
				enemyTarget = true;
			}
		} else {
			currentPlayerTarget = null;
			target = farPoint - gunBarrel.transform.position;
			gameObject.GetComponent<SpriteRenderer> ().color = Color.gray;
			enemyTarget = false;
		}
		Debug.DrawLine (gunBarrel.transform.position, point, Color.red);
		Debug.DrawLine(gunBarrel.transform.position, this.transform.position, Color.green);

		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Invoke ("Fire", 0.0001f);
		}

	}

	void Fire() {
		GameObject obj = BulletObjectPoolerScript.current.GetPooledObject();
		if (obj == null)
			return;
		obj.GetComponent<BulletDestroyScript> ().setEnemyBullet (false);
		obj.GetComponent<BulletDestroyScript> ().setPlayerTarget (currentPlayerTarget);
		obj.transform.position = gunBarrel.transform.position;
		obj.SetActive (true);
		obj.GetComponent<Rigidbody> ().velocity = target.normalized * speed;
		obj.GetComponent<BulletDestroyScript> ().OnEnable ();
	}

}
