using UnityEngine;
using System.Collections;

public class BulletDestroyScript : MonoBehaviour {

	public Rigidbody rb;
	public GameObject playerTarget;
	public GameController gameController;
	public Vector3 currentVector;
	public Vector3 enemyTarget;
	public ReticleScript thisReticle;
	public EnemyFire enemyFire;
	private float lerpSpeed;
	public bool enemyBullet;
	public bool isDestroyed;

	private Color playerBulletColor = new Color (152, 107, 26, 255);
	private Color enemyBulletColor = Color.blue;

	public void OnEnable() {
		isDestroyed = false;
		rb = this.gameObject.GetComponent<Rigidbody> ();
		lerpSpeed = 25f;
		gameObject.GetComponent<AudioSource> ().Play ();
		transform.rotation = Quaternion.identity;
		thisReticle = GameObject.FindWithTag ("Reticle").GetComponent<ReticleScript> ();
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		currentVector = thisReticle.farPoint;
		Invoke ("Destroy", 4f);
		if (!enemyBullet) {	
			transform.LookAt (currentVector);
		}
		if (enemyBullet) {
			enemyTarget = enemyFire.direction;
			transform.LookAt (enemyTarget);
		}
	}

	void Destroy () {
		gameObject.SetActive (false);
	}

	void OnDisable() {
		CancelInvoke ();
	}

	void Update() {
		if (playerTarget != null && !playerTarget.activeInHierarchy) {
			isDestroyed = true;
		}
		if (!enemyBullet && playerTarget != null && playerTarget.activeInHierarchy && !isDestroyed) {
			Vector3 currentVelocity = rb.velocity;
			Vector3 seekingVelocity = (playerTarget.transform.position- this.transform.position).normalized * thisReticle.speed;
			rb.velocity = seekingVelocity;
			//rb.velocity = Vector3.Lerp(currentVelocity, seekingVelocity, lerpSpeed);
		}
	}

	void OnTriggerEnter( Collider c){
		if (!enemyBullet) {
			if (c.gameObject.CompareTag("Enemy") || c.gameObject.CompareTag("Hazard") || c.gameObject.CompareTag("Bomber") || c.gameObject.CompareTag("Exploder")) {
				if (c.gameObject.CompareTag ("Enemy"))
					c.gameObject.GetComponent<EnemyFire> ().endFire ();
				gameObject.SetActive (false);
			}
		} else {
			if(c.gameObject.CompareTag("Player")) {
				gameObject.SetActive (false);
			}
		}

	}

	public void setEnemyBullet(bool bulletType){
		enemyBullet = bulletType;
	}

	public void setPlayerTarget(GameObject g) {
		playerTarget = g;
	}
}
