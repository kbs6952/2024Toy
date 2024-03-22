using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    // 플레이어의 방향( 정중앙 )
    // 밖으로 떨이지면 안되는 게임 -> 정중앙의 위치를 고수하는 것.
    // 적의 행동을 만드는 알고리즘 AI 행동 패턴 

    public GameObject centerPoint;
    public float enemyMoveSpeed;
    public Rigidbody rigidbody;

    private Vector3 targetDirection;

    [SerializeField] private float pushPower;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // 방향이 한번만 결정되고. Enemy 그 방향으로만 움직이기 때문에 (총알 피하기)
        //targetDirection = (playerObject.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy가 정중앙의 위치를 계속해서 이동하는 게임
        targetDirection = (centerPoint.transform.position - transform.position).normalized;
        rigidbody.AddForce(targetDirection * enemyMoveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestoryZone"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("적과 충돌하였음!");
            // 적과 충돌했을 때 적이 밖으로 더 잘 날라가게 해주는 기능을 추가해본다.

            Vector3 powerVector = (transform.position - collision.transform.position).normalized;    // 충돌(플레이어)가 Enemy 방향을 구한다 ( normalized를 통해 힘의 크기를 뺀 방향만 구할 수 있다.)
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();               // Enemy가 갖고 있는 Rigidbody를 참조해서 Enemy의 물리 효과를 구현할 수 있다.
            enemyRigidbody.AddForce(powerVector * pushPower, ForceMode.Impulse);                     // EnemyRigidbody. AddForce 함수를 이용해서 Enemy가 충돌할 때 더 크게 날라가도록 변경하였다.

        }
    }
}
