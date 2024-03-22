using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour, ICollisionable
{
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }


    public void CollideWithPlayer(Transform player, float _pushPower)
    {
        Vector3 awayVector = (transform.position - player.position).normalized;  // 날라갈 발향 - 출발할 위치(Player) 

        rigid.AddForce(awayVector * _pushPower, ForceMode.Impulse);          // Player에서 날라가는 힘을 매개변수로 전달 
    }

    
}
