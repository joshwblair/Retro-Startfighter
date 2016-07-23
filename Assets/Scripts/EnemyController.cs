using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	public GameObject explosion;
	public int scoreValue;
	private GameController gameController;
	private EnemySpawnController spawnController;
	private EnemyFire enemyFire;

	public void SetGameController() {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		spawnController = GameObject.FindWithTag ("EnemyStop").GetComponent<EnemySpawnController> ();
		if (!this.CompareTag ("Bomber")) {
			enemyFire = this.gameObject.GetComponent<EnemyFire> ();
		}
	}
		
	void OnTriggerEnter( Collider c){
		if (c.CompareTag("Projectile") || c.CompareTag("BombProjectile") || c.CompareTag("Player") /*|| c.CompareTag("Hazard")*/) {
			if (explosion != null) {
				Instantiate (explosion, transform.position, transform.rotation);
			}
			gameController.AddScore (scoreValue);
			spawnController.enemyAlive--;
			if (this.tag == "Enemy") {
				enemyFire.endFire ();
			}
			this.gameObject.SetActive(false);
		}
	}
}
