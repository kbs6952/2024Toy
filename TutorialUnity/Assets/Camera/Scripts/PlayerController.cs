using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CameraSetting
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("플레이어 입력 제어 변수")]
        [SerializeField] private float moveSpeed;       // 플레이어의 기본 속도
        [SerializeField] private float runSpeed;        // 플레이어가 달릴 때의 속도
        [SerializeField] private float jumpForce;       // 플레이어의 점프력

        private CharacterController cCon;               // Rigidbody대신 Character에 물리 충돌 기능과 이동을 위한 컴포넌트

        [Header("카메라 제어 변수")]
        [SerializeField] ThirdCamController thirdCam;        // 3인칭 카메라 컨트롤러가 있는 게임 오브젝트를 연결시켜줘야 한다.
        [SerializeField] float smoothRotation = 100f;        // 카메라의 자연스러운 회전을 위한 가중치
        Quaternion targetRotation;                           // 키보드 입력을 하지 않았을 때 카메라 방향으로 회전하기 위하여 회전 각도를 저장하는 변수

        [Header("점프 제어 변수")]
        [SerializeField] private float gravityModifier = 3f; // 플레이어가 땅에 떨어지는 속도를 제어할 변수
        [SerializeField] private Vector3 groundCheckPoint; // 땅을 판별하기 위한 체크 포인트
        [SerializeField] private float groundCheckRadius;    // 땅 체크하는 구의 크기 반지름
        [SerializeField] private LayerMask groundLayer;      // 체크할 레이어가 땅인지 판별하는 변수
        private bool isGrounded;                             // true이면 점프가 가능, false이면 점프 제한

        private float activeMoveSpeed;                  // 실제로 플레이어가 이동할 속력을 저장할 변수
        private Vector3 movement;                       // 플레이어가 움직이는 방향과 거리가 포함된 최종 Vector 값
        // Start is called before the first frame update
        void Start()
        {
            cCon = GetComponent<CharacterController>();
        }

        // Update is called once per frame 컴퓨터가 좋을 수록 frame이 많이 생성되고 Update도 많이 호출 됩니다.
        void Update()
        {
            // 1. Input 클래스를 이용하여 키보드 입력을 제어

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 2. 키보드 Input과 입력 값을 확인하기 위한 변수 선언
            Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;           // 키보드 입력값을 저장하는 백터 
            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // 키보드로 상하좌우 키 한개만 입력을 하면 0보다 큰 값을 moveAmount에 저장한다.
                                                                                           // 현재 위치 + 속도 * 가속도 = 이동 거리
                                                                                           // 이동거리만큼 (매 프레임마다 움직입니다 * Time.deltaTime)=> 프레임수와 상관없이 같은 시간에 같은 거리를 움직입니다.
            // 3. 플레이어 캐릭터 이동할 방향을 지정할 변수 선언

            //Vector3 firstMovement = transform.forward * moveDir.z + transform.right * moveDir.x;

            Vector3 moveDirection = thirdCam.camLookRotation * moveInput; // 플레이어가 이동할 방향을 저장하는 변수 moveDirection.  

            // 4. 플레이어의 이동 속도를 다르게 해주는 코드 (달리기 기능) - 
            if (Input.GetKey(KeyCode.LeftShift))  // Key Down : 누를 때 한번, Key : Key버튼을 떼기 전까지 계속        
                activeMoveSpeed = runSpeed;
            else 
                activeMoveSpeed = moveSpeed;

            
            // 5. 점프를 하기 위한 계산식 - 

            float yValue = movement.y;                               // 떨어지고 있는 y의 크기를 저장
            movement = moveDirection * activeMoveSpeed;               // 좌표에 이동할 x,0,z 백터 값을 저장  -> y떨어지고 있는 값이 0으로 초기화
            movement.y = yValue;                                      // 중력에 힘이 계속 받도록 잃어버린 변수를 다시 불러온다.

            // 다중 점프 되는 문제점이 발생 -> 현재 상태가 공중인 상태인지 아닌지 판단할 필요가 있다. 

            GroundCheck();

            if (cCon.isGrounded)
            {
                movement.y = 0;                                        // 공중인 상태일 때 y가 계속 -인 값이 저장됩니다.
                Debug.Log("현재 플레이어가 땅에 있는 상태입니다.");
            }


            // 점프키를 입력하여 점프 구현
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                movement.y = jumpForce;
            }

            movement.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            // 6. CharacterController 사용하여 캐릭터를 움직인다.
            if (moveAmount > 0) // moveDir 0일 때 moveMent가 0이 된다.
            {
               targetRotation = Quaternion.LookRotation(moveDirection);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotation * Time.deltaTime);
            cCon.Move(movement * Time.deltaTime); 
        }

        private void GroundCheck() // 플레이어가 땅인지 아닌지 판별하는 함수
        {
            isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius, groundLayer); // 레이어가 ground인 groundCheckRadius이고 위치 시작점이 checkPoint인 물리 충돌이 발생하면 True, false
        }

        private void OnDrawGizmos() // 눈에 안보이는 땅체크 함수를 가시화 하기 위해 선언
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius);
        }
    }


}