using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeFunction : MonoBehaviour
{
    // Debug.Log()

    private void Awake()
    {
        Debug.Log("Awake 함수가 실행됨");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start 함수가 실행됨");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable 함수가 실행됨");
    }
}
