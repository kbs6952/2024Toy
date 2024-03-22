using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewCamera : MonoBehaviour
{
    Vector3 offset;
    [SerializeField] Transform playerTr;
    [SerializeField] float smoothValue = 5f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTr.position;  //  플레이어가 카메라를 보는 방향과 크기 플레이어 더하면 => 카메라 위치
    }

    // PlayerController Update에서 이동 시키고, LateUpdate 카메라를 움직여 준다.
    void LateUpdate()
    {
        #region 카메라가 플레이어와 같은 속도로 이동함
        //transform.position = playerTr.position + offset;  // 카메라의 위치 = 플레이어가 이동한 위치 + 카메라와 플레이어가 고정되어야할 방향과 거리

        //offset = transform.position - playerTr.position;  // 플레이어가 이동함에 따라 변화한 offset을 다시 갱신해준다. 
        #endregion

        // 선형 보간을 사용한 카메라 이동 

        // 두 점의 값이 주어졌을 때 0 ~ 1 Percent로 그 사이의 값을 추정하는 방법입니다.

        #region 선형 보간을 통해 부드러운 카메라 이동
        Vector3 targetCamPos = playerTr.position + offset;         // (Update될 때마다) 카메라가 최종적으로 도착해야할 위치 입니다.
        // 1번 매개 변수 : 카메라가 이동하기 전 위치, 2번 매개 변수 : Update가 끝날 때 최종적으로 이동할 위치, 3번 매개 변수 : a와 b의 거리 비율을 percent로 나타낸 값
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothValue * Time.deltaTime);    // 백터 방향과 크기를 가진 데이터를 가지고 있는대. 방향은 유지한 채로 크기만 쪼금씩 천천히 이동시켜 볼겁니다. 
        #endregion

    }
}
