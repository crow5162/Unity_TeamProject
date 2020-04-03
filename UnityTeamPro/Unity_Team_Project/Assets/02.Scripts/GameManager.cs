using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //몬스터 출현 위치 
    public Transform[] points;
    //몬스터 프리팹 저장할 변수 
    public GameObject enemy;
    //생성 시간 
    public float createTime = 3.0f;
    //최대 생성 갯수 
    public int maxEnemy = 8;
    //게임 종료 판단 
    public bool isGameOver = false;
    void Start()
    {
        //계층창의 스폰오브젝트를 찾아서 모든 Transform 컴포넌트를 찾아씀. 
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length >  0)
        {
            StartCoroutine(this.CreateEnemy());
        }

    }

    // 적을 생성하는 코루틴 함수 
    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            //현재 생성된 적 케릭터 숫자 산출 
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
            //적 캐릭터의 최대 생성 개수보다 작을 때만 적 캐릭터를 생성
            if (enemyCount < maxEnemy)
            {
                //적 캐릭터의 생성 시간만큼 대기! 
                yield return new WaitForSeconds(createTime);
                //불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);
                //적 동적으로 생성
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }

        }
    }



    void Update()
    {
        
    }
}
