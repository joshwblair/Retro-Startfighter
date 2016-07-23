using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

	public GameController gameController;
	public GameObject playerExplosion;
	public GameObject gunBarrel;
	public Boundary boundary;
	private float xAxis;
	private float yAxis;
	[SerializeField] private int playerSpeed = 0;
	private Rigidbody rb;
	[SerializeField] private float tilt = 0;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		xAxis = Input.GetAxis ("Horizontal");
		yAxis = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (xAxis, yAxis, 0f);
		rb.velocity =  movement * playerSpeed;

		rb.position = new Vector3 
		(
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
			Mathf.Clamp (rb.position.y, boundary.yMin, boundary.yMax), 
			0f
			
		);

		rb.rotation = Quaternion.Euler (rb.velocity.y * -tilt, rb.velocity.x * tilt, (rb.velocity.y + rb.velocity.x) * -tilt);

	}

	void OnTriggerEnter(Collider c) {
		if (c.CompareTag ("Hazard") || c.CompareTag("Bomber") || c.CompareTag("Exploder") 
			|| (c.CompareTag ("Projectile") && c.gameObject.GetComponent<BulletDestroyScript> ().enemyBullet)
			|| c.CompareTag("BombProjectile")) {
			if (playerExplosion != null) {
				Instantiate (playerExplosion, transform.position, transform.rotation);
			}
			gameController.GameOver ();
			gameObject.SetActive (false);
		}
	}
}