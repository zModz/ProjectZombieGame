using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

	public Transform Player;
	public Transform Enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onCollisionEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			PlayerScript playerHealth = Player.GetComponent <PlayerScript> ();

			EnemyScript enemyDamage = Enemy.GetComponent <EnemyScript> ();

			if(playerHealth != null)
			{
				// ... the enemy should take damage.
				playerHealth.TakeDamageZombie (enemyDamage.damage);
			}
		}
	}
}
