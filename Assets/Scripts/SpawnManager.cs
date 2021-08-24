using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9;
    private PlayerController playerControllerScript;

    public GameObject[] powerUpPrefab;
    public GameObject[] enemyPrefab;
    public GameObject enemyBossPrefab;
    public int enemyCount;
    public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find( "Player" ).GetComponent<PlayerController>();
        SpawnEnemyWave(level);
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

                if ( level % 2 == 0 )
				{
                    int powerUpIdx = Random.Range( 0, powerUpPrefab.Length );
                    Instantiate( powerUpPrefab[powerUpIdx], GenerateSpawnPosition(), powerUpPrefab[powerUpIdx].transform.rotation );
				}
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
            if ( level % 4 == 0 )
            {
                SpawnBossWave( level / 2);
                level -= 2;
            }

            int enemyIdx = Random.Range( 0, enemyPrefab.Length );
            Instantiate( enemyPrefab[enemyIdx], GenerateSpawnPosition(), enemyPrefab[enemyIdx].transform.rotation );
        }
    }

    void SpawnBossWave(int minionsToSpawn)
    {
        var boss = Instantiate( enemyBossPrefab, GenerateSpawnPosition(), enemyBossPrefab.transform.rotation );

        boss.GetComponent<Enemy>().enemyToSpawn = minionsToSpawn;
    }

    public void SpawnMinion(int amount)
	{
        for (int i = 0; i < amount; i++ )
		{
            int enemyIdx = Random.Range( 0, enemyPrefab.Length );

			Instantiate( enemyPrefab[enemyIdx], GenerateSpawnPosition(), enemyPrefab[enemyIdx].transform.rotation );
        }
    }
}
