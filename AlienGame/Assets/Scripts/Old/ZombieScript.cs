using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour {

	public Transform Player;
    public GameObject Player_GO;
    MeshCollider col;
	NavMeshAgent nav;
	Animator anim;
    GameManager GaMa;
	public int damage;
	public int currentHealth = 100;
	public bool isDead;
    float dist;


    // Use this for initialization
    void Awake () {
		currentHealth = 100;
		anim = GetComponent <Animator> ();
		Player = GameObject.FindGameObjectWithTag ("Player").transform;
        Player_GO = GameObject.FindGameObjectWithTag("Player");
        GaMa = FindObjectOfType<GameManager>();
		nav = GetComponent <NavMeshAgent> ();
		col = GetComponent <MeshCollider> ();
	}

	// Update is called once per frame
	void Update () {
		this.nav.SetDestination (Player.position);

        dist = Vector3.Distance(Player.position, transform.position);

        if (dist <= 2)
        {
            this.anim.SetBool("Run-Attack", true);
            StartCoroutine("DamagePlayer");
        }
	}

	public void TakeDamage (int amount)
	{
		// If the enemy is dead...
		if (this.isDead)
			return;

		// Play the hurt sound effect.
		//enemyAudio.Play ();

		// Reduce the current health by the amount of damage sustained.
		this.currentHealth -= amount;
		//marker.getHitmarker ();

		// If the current health is less than or equal to zero...
		if (this.currentHealth == 0) {
			this.isDead = true;
			this.nav.isStopped = true;
			this.col.isTrigger = true;
            this.GaMa.zombieCount--;
			this.anim.SetBool ("Run-Death", true);
			Destroy (this.gameObject, 4);
		}
	}

    IEnumerator DamagePlayer()
    {
        PlayerScript _player = Player_GO.GetComponent<PlayerScript>();
        yield return new WaitForSeconds(2f);
        _player.TakeDamageZombie(damage);
        this.anim.SetBool("Run-Attack", false);
        StopAllCoroutines();
    }
}
