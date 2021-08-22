using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9;
    private PlayerController playerControllerScript;

    public GameObject powerUpPrefab;
    public GameObject enemyPrefab;
    public int enemyCount;
    public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find( "Player" ).GetComponent<PlayerController>();
        SpawnEnemyWave(level);
        Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if ( !playerControllerScript.gameOver )
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;
            if ( enemyCount == 0 )
            {
                SpawnEnemyWave( ++level );

                if ( level % 3 == 0 )
                    Instantiate( powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation );
            }
        }
        else
        {
            Destroy( gameObject );
            Debug.Log( level );
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        return new Vector3(spawnPosX, 0, spawnPosZ);
    }

    void SpawnEnemyWave( int level )
    {
        for (int i = 0; i < level; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
