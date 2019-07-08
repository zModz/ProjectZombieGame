using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public GameObject enemy;
	public float spawnTime;
	public Transform[] spawnPoints;
	public int roundCount;
    public int InitialzombieCount;
    public int zombieCount;
	GameObject clone;

	public Text roundText;

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    // Use this for initialization
    void Start ()
    {
        spawnTime = 3f;
        InitialzombieCount = 4;
        zombieCount = InitialzombieCount;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        roundText.text = roundCount.ToString ();
        spawnTime -= Time.deltaTime;

        if (zombieCount == 0)
        {
            var temp = InitialzombieCount + 4;
            Debug.Log("temp = " + temp);
            InitialzombieCount = temp;
            zombieCount = temp;
            NewRound();
        }
	}

    IEnumerator Spawn()
    {
        //int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        List<Transform> freeSpawnPoints = new List<Transform>(spawnPoints);
        for (int i = 0; i < zombieCount; i++)
        {
            while(zombieCount < InitialzombieCount)
            {
                if (freeSpawnPoints.Count >= 0)
                {
                    int index = Random.Range(0, freeSpawnPoints.Count);
                    Transform pos = freeSpawnPoints[index];
                    freeSpawnPoints.RemoveAt(index); // remove the spawnpoint from our temporary list
                    clone = (Instantiate(enemy, pos.position, pos.rotation));
                    //clone = (Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation)) as GameObject;
                    //return; // Not enough spawn points
                }
                yield return null;
            }
        }
    }

	void NewRound() {
		roundCount++;
        StartCoroutine(Spawn());
	}
}
