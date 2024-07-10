using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스폰 관리 
public class SpawnManager2 : MonoBehaviour
{


    // 스폰 포인트 배열 
    [SerializeField]
    Transform[] spawnPoints;


    // 몬스터 프리팹 
    [SerializeField]
    GameObject enemy;

    // 생성 간격을 주기 위한 변수 : Time.deltaTime으로 curTime을 매 프레임 증가시키고 curTime이 spawnTime보다 크면 생성코드를 실행되게 만들어준다. 
    [SerializeField]
    private float spawnTime = 3f;
    private float curTime;


    // Enemy 제한할 수 
    public int enemyCount;
    [SerializeField]
    private int maxCount;

    // 중복 생성 제한을 위한 변수 
    public bool[] isSpawn;

    public static SpawnManager2 instance;
    private void Awake()
    {
        isSpawn = new bool[spawnPoints.Length];
        for (int i = 0; i < isSpawn.Length; i++)
        {
            isSpawn[i] = false;
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            // 랜덤으로 스폰 포인트 결정 
            int x = Random.Range(0, spawnPoints.Length);
            if (!isSpawn[x])
            {
                SpawnEnemy(x);
            }
        }
        // Update는 프레임마다 호출되며, Time.deltaTime은 이전프레임의 완료까지 시간을 반환 
        curTime += Time.deltaTime;
    }

    public void SpawnEnemy(int ranNum)
    {
        curTime = 0;    // 몬스터 생성후 초기화 
        enemyCount++;   // 적개체 추가
        // 몬스터 프리팹 생성 
        Instantiate(enemy, spawnPoints[ranNum]);
        isSpawn[ranNum] = true;
    }
}
