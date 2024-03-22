using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamController : MonoBehaviour
{
    [Header("ī�޶� ���� ����")]
    [SerializeField] private Transform target;          // ī�޶� ���� ���
    [SerializeField] private float camDistance;         // ���� ī�޶���� �Ÿ�
    [SerializeField] private float rotSpeed;            // ī�޶� ȸ���ϴ� �ӵ� ũ��
    [SerializeField] private int limitAngle;            // ī�޶��� ���� ����
    [SerializeField] private bool inverseX;             // ���콺 ���Ʒ� ���� üũ
    [SerializeField] private bool inverseY;             // ���콺 �¿�   ���� üũ

    float rotationX;
    float rotationY;

    public Quaternion camLookRotation => Quaternion.Euler(0, rotationY, 0);

    // Update is called once per frame
    void Update()
    {
        float invertXValue = (inverseX) ? -1 : 1;
        float invertYValue = (inverseY) ? -1 : 1;        // ���� �̵��� ���� ���� ��� üũ

        // ���콺�� �Է� ���� �޾ƿ´�.
        // ���콺�� ���Ʒ��� ������ �� ���� rotationX�� ���� ��ȭ �Ǿ� ������ �˴ϴ�.
        // ���� ȸ�� ����
        rotationX -= Input.GetAxis("Mouse Y") * invertYValue * rotSpeed;        // ���� ȸ���� ���� ���콺 �Է� ��
        rotationX = Mathf.Clamp(rotationX, -limitAngle, limitAngle);

        // �¿� ȸ�� ����
        rotationY += Input.GetAxis("Mouse X") * invertXValue * rotSpeed;        // �¿� ȸ���� ���� ���콺 �Է� ��

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);         // ���� ȸ���� ���� ���Ϸ� ��ġ, �¿� ȸ���� ���� ���Ϸ� ��ġ�� �ݿ��� ȸ�� ���� targetRotation ����

        transform.rotation = targetRotation;

        // ī�޶� �÷��̾ �ѾƼ� �̵��ϴ� ����
        Vector3 focusPosition = target.position;                                                                    // ���� �÷��̾��� ��ġ
        transform.position = focusPosition  -  (targetRotation * new Vector3(0,0,camDistance));                     // �ڿ� �ִ� Vector���� �÷��̾ �ٶ󺸴� ���� ���͸� ��ȯ�Ѵ�.
    }
}