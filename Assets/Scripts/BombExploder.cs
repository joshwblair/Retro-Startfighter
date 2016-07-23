using UnityEngine;
using System.Collections;

public class BombExploder : MonoBehaviour {
	private EnemySpawnController enemyController;
	private GameController gameController;
	private Transform playerTransform;
	private float speed = 3f;
	private bool stopped;
	[SerializeField] private GameObject[] bullets = new GameObject[8];
	[SerializeField] private Vector3[] bulletDirs = new [] {
		Vector3.up, new Vector3 (0f, -1f, 0f), new Vector3 (-1f,0f,0f), new Vector3(1f,0f,0f),
		new Vector3(-1f,1f,0f), new Vector3(1f,1f,0f), new Vector3(1f,-1f,0f), new Vector3(-1f,-1f,0f)
	};

	public void OnEnable () {
		stopped = false;
		enemyController = GameObject.FindWithTag ("EnemyStop").GetComponent<EnemySpawnController> ();
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		if (!gameController.gameOver) {
			playerTransform = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		}
		GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, -30f);
	}

	void Start() {
	}

	void Update () {
		if (!gameController.gameOver) {
			playerTransform = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		}
		if (stopped) {
			if (!gameController.gameOver) {
				transform.position = Vector3.Lerp (this.transform.position, playerTransform.position, 0.01f);
			}

		}
	}

	IEnumerator BlowUp() {
		
		yield return new WaitForSeconds (1f);
		for (int i = 0; i < bullets.Length; i++) {
			GameObject obj = BulletObjectPoolerScript.current.GetPooledBomb();
			if (obj == null)
				return false;
			obj.transform.position = this.transform.position;
			bullets [i] = obj;
			bullets [i].SetActive (true);
			Vector3 dir = bulletDirs[i];
			bullets [i].GetComponent<Rigidbody>().velocity = dir.normalized * speed;
			bullets [i].GetComponent<SetInactiveByTime> ().OnEnable ();
		}
		yield return new WaitForSeconds (0.01f);
		enemyController.enemyAlive--;
		gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider c) {
		if (c.CompareTag ("PlayerPlane") && this.gameObject.activeInHierarchy) {
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			stopped = true;
			StartCoroutine (BlowUp ());
		}
	}
}
