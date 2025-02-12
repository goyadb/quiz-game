using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoController : MonoBehaviour
{
    [SerializeField] private RectTransform topLogoRectTransform;
    [SerializeField] private RectTransform middleLogoRectTransform;
    [SerializeField] private RectTransform bottomLogoRectTransform;

    private void Start()
    {
        // 3개의 로고 이미지의 위치를 변경해서 로고가 입체적으로 보이게 하는 애니메이션 실행
        topLogoRectTransform.DOAnchorPosY(15f, 2f);
        bottomLogoRectTransform.DOAnchorPosY(-15f, 2f);
    }
}
