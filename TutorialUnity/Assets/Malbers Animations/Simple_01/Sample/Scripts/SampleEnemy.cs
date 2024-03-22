using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionable
{
    public void CollideWithPlayer(Transform player, float _pushPower); // Player(공)과 부딪힌 객체가 특정 방향으로 날라가는 기능을 인터페이스 구현하겠다.
}


public class SampleEnemy : MonoBehaviour, ICollisionable
{
    // 플레이어의 방향( 정중앙 )
    // 밖으로 떨이지면 안되는 게임 -> 정중앙의 위치를 고수하는 것.
    // 적의 행동을 만드는 알고리즘 AI 행동 패턴 

    [SerializeField] private GameObject centerPoint;
    [SerializeField] private float enemyMoveSpeed;
    private Rigidbody rigidbody;
    private Vector3 targetDirection;

    [SerializeField] private float pushPower;

    public GameObject CenterPoint
    { 
        get => centerPoint;
        set => centerPoint = value;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        centerPoint = GameObject.Find("CenterPivot");
        // 방향이 한번만 결정되고. Enemy 그 방향으로만 움직이기 때문에 (총알 피하기)
        //targetDirection = (playerObject.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy가 정중앙의 위치를 계속해서 이동하는 게임
        targetDirection = (centerPoint.transform.position - transform.position).normalized;
        rigidbody.AddForce(targetDirection * enemyMoveSpeed, ForceMode.Force);

        if(transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
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
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // 적과 충돌했을 때 적이 밖으로 더 잘 날라가게 해주는 기능을 추가해본다.

        //    Vector3 powerVector = (transform.position - collision.transform.position).normalized;    // 충돌(플레이어)가 Enemy 방향을 구한다 ( normalized를 통해 힘의 크기를 뺀 방향만 구할 수 있다.)

        //    rigidbody.AddForce(powerVector * pushPower, ForceMode.Impulse);                         // EnemyRigidbody. AddForce 함수를 이용해서 Enemy가 충돌할 때 더 크게 날라가도록 변경하였다.

        //}
    }

    public void CollideWithPlayer(Transform player, float _pushPower)
    {
        // 플레이어와 충돌했을 때 객체가 날라가는 로직을 작성해주면 됩니다.
        Debug.Log("Collide인터페이스가 호출됨!");

        Vector3 awayVector = (transform.position - player.position).normalized;  // 날라갈 발향 - 출발할 위치(Player) 

        rigidbody.AddForce(awayVector * _pushPower, ForceMode.Impulse);          // Player에서 날라가는 힘을 매개변수로 전달 

    }
}
