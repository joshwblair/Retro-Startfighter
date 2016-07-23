using UnityEngine;
using System.Collections;

public class EnemySuicideBomber : MonoBehaviour {

	public GameController gameController;
	public GameObject enemyExplosion;
	public GameObject enemyController;
	public Vector3 startPosition;
	public GameObject player;
	public Rigidbody bomberRigidbody;

	public float speed;
	public bool stop;
	public bool returnToStart;
	public float returnTime;

	//Coroutine Values
	public float reversalSpeed;
	public float startRunWait;


	public void Start () {
		reversalSpeed = 5f;
		startRunWait = 2f;
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		player = GameObject.FindWithTag ("Player");
		bomberRigidbody = this.GetComponent<Rigidbody> ();
		bomberRigidbody.velocity = new Vector3 (0f, 0f, -30f);
		startPosition = this.transform.position;
		enemyController = GameObject.FindWithTag ("EnemyStop");
		stop = true;
		returnToStart = false;
		returnTime = 0f;
	}

	void Update() {
		if (bomberRigidbody.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (-bomberRigidbody.velocity);
		}

		if (returnToStart) {
			returnTime -= Time.deltaTime;
			if (returnTime > 0) {
				bomberRigidbody.velocity = Vector3.Lerp (bomberRigidbody.velocity, (startPosition - transform.position).normalized * speed, 0.02f);
			}
			if (returnTime < 0 && transform.position != startPosition) {
				bomberRigidbody.velocity = new Vector3 (0f, 0f, 0f);
				transform.position = Vector3.Lerp (transform.position, startPosition, 0.05f);
				if (returnTime < -2) {
					transform.position = startPosition;
				}
			}
			speed -= Time.deltaTime*10;
			if (transform.position == startPosition) {
				bomberRigidbody.velocity = new Vector3 (0f, 0f, -30f);
				stop = true;
				returnToStart = false;
			}
		}

		if (gameController.gameOver) {
			StopCoroutine (BombingRun ());
		}
	}

	void OnTriggerEnter(Collider c) {
		if(c.CompareTag("EnemyStop") && stop) {
			bomberRigidbody.velocity = new Vector3 (0f, 0f, 0f);
			if (!gameController.gameOver) {
				StartCoroutine (BombingRun ());
			}
		}
	}

	IEnumerator BombingRun(){
		stop = false;
		speed = 75f;
		yield return new WaitForSeconds (startRunWait);
		bomberRigidbody.velocity = new Vector3 (0f, 0f, reversalSpeed);
		yield return new WaitForSeconds (startRunWait + Random.Range(-1f, 1f));
		bomberRigidbody.velocity = new Vector3 (0f, 0f, 0f);
		yield return new WaitForSeconds (startRunWait);
		bomberRigidbody.velocity = (player.transform.position - this.transform.position).normalized * speed;
		yield return new WaitForSeconds (startRunWait);
		float x = Random.Range (30, 60);
		if (Random.Range (0, 1) == 0) {
			x *= -1;
		} 
		bomberRigidbody.velocity = new Vector3 (x, Random.Range(-20f, 20f), Random.Range(1f, 10f));
		returnToStart = true;
		returnTime = 5f;
	}

}
