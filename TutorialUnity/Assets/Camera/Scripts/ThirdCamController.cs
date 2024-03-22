using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamController : MonoBehaviour
{
    [Header("카메라 제어 변수")]
    [SerializeField] private Transform target;          // 카메라가 찍을 대상
    [SerializeField] private float camDistance;         // 대상과 카메라와의 거리
    [SerializeField] private float rotSpeed;            // 카메라가 회전하는 속도 크기
    [SerializeField] private int limitAngle;            // 카메라의 제한 각도
    [SerializeField] private bool inverseX;             // 마우스 위아래 반전 체크
    [SerializeField] private bool inverseY;             // 마우스 좌우   반전 체크

    float rotationX;
    float rotationY;

    public Quaternion camLookRotation => Quaternion.Euler(0, rotationY, 0);

    // Update is called once per frame
    void Update()
    {
        float invertXValue = (inverseX) ? -1 : 1;
        float invertYValue = (inverseY) ? -1 : 1;        // 상하 이동의 반전 제어 기능 체크

        // 마우스의 입력 값을 받아온다.
        // 마우스를 위아래로 움직일 때 마다 rotationX의 값이 변화 되어 저장이 됩니다.
        // 상하 회전 구현
        rotationX -= Input.GetAxis("Mouse Y") * invertYValue * rotSpeed;        // 상하 회전에 대한 마우스 입력 값
        rotationX = Mathf.Clamp(rotationX, -limitAngle, limitAngle);

        // 좌우 회전 구현
        rotationY += Input.GetAxis("Mouse X") * invertXValue * rotSpeed;        // 좌우 회전에 대한 마우스 입력 값

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);         // 상하 회전에 대한 오일러 수치, 좌우 회전에 대한 오일러 수치를 반영한 회전 값을 targetRotation 저장

        transform.rotation = targetRotation;

        // 카메라가 플레이어를 쫓아서 이동하는 로직
        Vector3 focusPosition = target.position;                                                                    // 현재 플레이어의 위치
        transform.position = focusPosition  -  (targetRotation * new Vector3(0,0,camDistance));                     // 뒤에 있는 Vector에서 플레이어를 바라보는 방향 백터를 반환한다.
    }
}
