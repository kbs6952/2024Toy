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
        [Header("�÷��̾� �Է� ���� ����")]
        [SerializeField] private float moveSpeed;       // �÷��̾��� �⺻ �ӵ�
        [SerializeField] private float runSpeed;        // �÷��̾ �޸� ���� �ӵ�
        [SerializeField] private float jumpForce;       // �÷��̾��� ������

        private CharacterController cCon;               // Rigidbody��� Character�� ���� �浹 ��ɰ� �̵��� ���� ������Ʈ

        [Header("ī�޶� ���� ����")]
        [SerializeField] ThirdCamController thirdCam;        // 3��Ī ī�޶� ��Ʈ�ѷ��� �ִ� ���� ������Ʈ�� ���������� �Ѵ�.
        [SerializeField] float smoothRotation = 100f;        // ī�޶��� �ڿ������� ȸ���� ���� ����ġ
        Quaternion targetRotation;                           // Ű���� �Է��� ���� �ʾ��� �� ī�޶� �������� ȸ���ϱ� ���Ͽ� ȸ�� ������ �����ϴ� ����

        [Header("���� ���� ����")]
        [SerializeField] private float gravityModifier = 3f; // �÷��̾ ���� �������� �ӵ��� ������ ����
        [SerializeField] private Vector3 groundCheckPoint; // ���� �Ǻ��ϱ� ���� üũ ����Ʈ
        [SerializeField] private float groundCheckRadius;    // �� üũ�ϴ� ���� ũ�� ������
        [SerializeField] private LayerMask groundLayer;      // üũ�� ���̾ ������ �Ǻ��ϴ� ����
        private bool isGrounded;                             // true�̸� ������ ����, false�̸� ���� ����

        private float activeMoveSpeed;                  // ������ �÷��̾ �̵��� �ӷ��� ������ ����
        private Vector3 movement;                       // �÷��̾ �����̴� ����� �Ÿ��� ���Ե� ���� Vector ��
        // Start is called before the first frame update
        void Start()
        {
            cCon = GetComponent<CharacterController>();
        }

        // Update is called once per frame ��ǻ�Ͱ� ���� ���� frame�� ���� �����ǰ� Update�� ���� ȣ�� �˴ϴ�.
        void Update()
        {
            // 1. Input Ŭ������ �̿��Ͽ� Ű���� �Է��� ����

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 2. Ű���� Input�� �Է� ���� Ȯ���ϱ� ���� ���� ����
            Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;           // Ű���� �Է°��� �����ϴ� ���� 
            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // Ű����� �����¿� Ű �Ѱ��� �Է��� �ϸ� 0���� ū ���� moveAmount�� �����Ѵ�.
                                                                                           // ���� ��ġ + �ӵ� * ���ӵ� = �̵� �Ÿ�
                                                                                           // �̵��Ÿ���ŭ (�� �����Ӹ��� �����Դϴ� * Time.deltaTime)=> �����Ӽ��� ������� ���� �ð��� ���� �Ÿ��� �����Դϴ�.
            // 3. �÷��̾� ĳ���� �̵��� ������ ������ ���� ����

            //Vector3 firstMovement = transform.forward * moveDir.z + transform.right * moveDir.x;

            Vector3 moveDirection = thirdCam.camLookRotation * moveInput; // �÷��̾ �̵��� ������ �����ϴ� ���� moveDirection.  

            // 4. �÷��̾��� �̵� �ӵ��� �ٸ��� ���ִ� �ڵ� (�޸��� ���) - 
            if (Input.GetKey(KeyCode.LeftShift))  // Key Down : ���� �� �ѹ�, Key : Key��ư�� ���� ������ ���        
                activeMoveSpeed = runSpeed;
            else 
                activeMoveSpeed = moveSpeed;

            
            // 5. ������ �ϱ� ���� ���� - 

            float yValue = movement.y;                               // �������� �ִ� y�� ũ�⸦ ����
            movement = moveDirection * activeMoveSpeed;               // ��ǥ�� �̵��� x,0,z ���� ���� ����  -> y�������� �ִ� ���� 0���� �ʱ�ȭ
            movement.y = yValue;                                      // �߷¿� ���� ��� �޵��� �Ҿ���� ������ �ٽ� �ҷ��´�.

            // ���� ���� �Ǵ� �������� �߻� -> ���� ���°� ������ �������� �ƴ��� �Ǵ��� �ʿ䰡 �ִ�. 

            GroundCheck();

            if (cCon.isGrounded)
            {
                movement.y = 0;                                        // ������ ������ �� y�� ��� -�� ���� ����˴ϴ�.
                Debug.Log("���� �÷��̾ ���� �ִ� �����Դϴ�.");
            }


            // ����Ű�� �Է��Ͽ� ���� ����
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                movement.y = jumpForce;
            }

            movement.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            // 6. CharacterController ����Ͽ� ĳ���͸� �����δ�.
            if (moveAmount > 0) // moveDir 0�� �� moveMent�� 0�� �ȴ�.
            {
               targetRotation = Quaternion.LookRotation(moveDirection);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotation * Time.deltaTime);
            cCon.Move(movement * Time.deltaTime); 
        }

        private void GroundCheck() // �÷��̾ ������ �ƴ��� �Ǻ��ϴ� �Լ�
        {
            isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius, groundLayer); // ���̾ ground�� groundCheckRadius�̰� ��ġ �������� checkPoint�� ���� �浹�� �߻��ϸ� True, false
        }

        private void OnDrawGizmos() // ���� �Ⱥ��̴� ��üũ �Լ��� ����ȭ �ϱ� ���� ����
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius);
        }
    }


}