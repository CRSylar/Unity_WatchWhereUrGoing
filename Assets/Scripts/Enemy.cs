using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    public bool isBoos = false;
    public float spawnDelay;
    public int enemyToSpawn;

    private float nextSpawn;
    private SpawnManager spawnManager;
    private Rigidbody enemyRb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if ( isBoos )
            spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (player)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;

            enemyRb.AddForce( lookDirection * speed);

            if (transform.position.y < -10)
                Destroy(gameObject);

            if (isBoos)
		    {
                if (Time.time > nextSpawn)
			    {
                    nextSpawn = Time.time + spawnDelay;
                    spawnManager.SpawnMinion( enemyToSpawn );
			    }
		    }
        }
    }
}
