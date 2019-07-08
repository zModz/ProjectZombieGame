using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

	public Transform Player;
	public Transform target;
	private NavMeshAgent nav;
	private Animation anim;
	//public HitmarkerScript marker;
	public int damage;
	public Transform hand;

	bool isDead;
	public int currentHealth = 100;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		Player = GameObject.FindGameObjectWithTag ("Player").transform;
		nav = GetComponent <NavMeshAgent> ();
		//this.marker = Player.GetComponent<WeaponScript> ().marker;
	}
	
	// Update is called once per frame
	void Update () {
		nav.SetDestination (Player.position);

		float dist = Vector3.Distance(Player.transform.position, target.transform.position);
		if (dist < damage){
			anim.Play ("attack");
		}
	}

	public void TakeDamage (int amount, Vector3 hitPoint)
	{
		// If the enemy is dead...
		if (isDead)
			return;

		// Play the hurt sound effect.
		//enemyAudio.Play ();

		// Reduce the current health by the amount of damage sustained.
		currentHealth -= amount;
		//marker.getHitmarker ();

		// Set the position of the particle system to where the hit was sustained.
		//hitParticles.transform.position = hitPoint;

		// And play the particles.
		//hitParticles.Play();

		// If the current health is less than or equal to zero...
		if (currentHealth <= 0) {
			death ();
		}
	}

	void death() 
	{
		anim.Play ("back_fall");
		nav.Stop ();
		//Destroy (gameObject, 3);
	}
}
