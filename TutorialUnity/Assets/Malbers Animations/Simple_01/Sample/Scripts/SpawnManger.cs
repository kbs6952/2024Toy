using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManger : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject powerupObject;
    [SerializeField] private Transform enemySpawnPosition;

    public int enemyCount = 0;
    public int waveNumber = 1;

    public float limitWidth = 5;
    public float limitHeight = 11;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy(waveNumber);

        // 한 개의 적만, 특정 위치에서 생성되는 코드 작성
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<SampleEnemy>().Length; // 하이어라키에서 SampleEnemy 스크립트를 갖고 있는 오브젝트를 찾아서 그 갯수를 반환하는 코드

        if (enemyCount == 0)                                  // SampleEnemy 스크립트를 갖고 있는 오브젝트 0 "모든 Enemy가 죽었을 때" 적을 생성한다.
        {
            waveNumber++;
            SpawnEnemy(waveNumber);
        }
    }

    private void SpawnEnemy(int spawnNumber)                  // 적을 전부 처치할 때 마다 Wave의 수가 1씩 증가하고, 증가한 Wave 수 만큼 다음 웨이브(Enemy)를 생성한다.
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            GameObject enemyObj = Instantiate(enemy, RandomSpawnPoistion(), Quaternion.identity);
        }  
    }

    private Vector3 RandomSpawnPoistion()
    {
        float randomX = UnityEngine.Random.Range(-limitWidth, limitWidth);
        float randomZ = UnityEngine.Random.Range(-limitHeight, limitHeight);
        
        Vector3 randomPos = new Vector3(randomX, 0, randomZ);

        return randomPos;
    }
}
