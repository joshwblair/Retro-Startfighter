using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour {
	public GameController gameController;
	//[SerializeField] private GameObject sphere;
	[SerializeField] private float radius;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject self;
	//public bool playerTarget;

	//public Vector3 point;
	public GameObject front;
	public GameObject projectile;
	public float speed;
	//public Vector3 target, farpoint;
	public Vector3 direction;
	public float fireRate;
	public float nextFire;

	// Use this for initialization
	void Start () {
		//direction = Vector3.forward; //Scriptable Object tutorial
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		self = this.gameObject;
		radius = 2f;
		nextFire = 0f;
	}

	public void startFire() {
		InvokeRepeating ("fire", 3f, Random.Range(5f, 10f));
	}

	public void endFire() {
		CancelInvoke ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.gameOver) {
			endFire ();
		}
	}

	void fire() {
		direction = player.transform.position - self.transform.position;
		GameObject bullet = BulletObjectPoolerScript.current.GetPooledObject ();
		if (bullet == null)
			return;
		bullet.GetComponent<BulletDestroyScript> ().setEnemyBullet (true);
		bullet.GetComponent<BulletDestroyScript> ().enemyFire = this;
		bullet.transform.position = front.transform.position;
		bullet.transform.rotation = transform.rotation;
		bullet.SetActive (true);
		bullet.GetComponent<Rigidbody> ().velocity = direction.normalized * speed;
		bullet.GetComponent<BulletDestroyScript> ().OnEnable ();
	}
}
