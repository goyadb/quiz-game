using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;            // 화면 상단의 타이틀 텍스트
    [SerializeField] private GameObject panelObject;        // 팝업창 오브젝트

    private Image _backgroundImage;
    
    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        
        var color = _backgroundImage.color;
        color.a = 0;
        _backgroundImage.color = color;
        
        panelObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Start()
    {
        ShowPopupPanel();
    }

    /// <summary>
    /// 타이틀 텍스트에 타이틀 지정 함수
    /// </summary>
    /// <param name="title">타이틀</param>
    public void SetTitleText(string title)
    {
        titleText.text = title;
    }

    /// <summary>
    /// 닫기 버튼을 클릭했을때 실행되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        HidePopupPanel();
    }

    private void ShowPopupPanel()
    {
        _backgroundImage.DOFade(0, 0);
        panelObject.GetComponent<CanvasGroup>().DOFade(0, 0);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-500f, 0);
        
        _backgroundImage.DOFade(1f, 0.2f);
        panelObject.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-70f, 0.2f);
    }

    private void HidePopupPanel()
    {
        _backgroundImage.DOFade(1, 0);
        panelObject.GetComponent<CanvasGroup>().DOFade(1, 0);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-70f, 0);
        
        _backgroundImage.DOFade(0f, 0.2f);
        panelObject.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-500f, 0.2f)
            .OnComplete(() => Destroy(gameObject));
    }
}
