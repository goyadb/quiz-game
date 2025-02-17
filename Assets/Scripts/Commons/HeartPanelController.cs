using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPanelController : MonoBehaviour
{
    private int _heartCount;
    
    // 1. 하트 추가 연출
    // 2. 하트 감소 연출
    // 3. 하트 부족 연출


    /// <summary>
    /// Heart Panel에 하트 수 초기화
    /// </summary>
    /// <param name="heartCount">하트 수</param>
    public void InitHeartCount(int heartCount)
    {
        _heartCount = heartCount;
    }
}
