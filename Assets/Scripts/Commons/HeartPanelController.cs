using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HeartPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _heartRemoveImageObject;
    [SerializeField] private TMP_Text _heartCountText;
    
    private int _heartCount;
    
    // 1. 하트 추가 연출
    // 2. 하트 감소 연출
    // 3. 하트 부족 연출

    private void Start()
    {
        _heartRemoveImageObject.SetActive(false);
        
        InitHeartCount(10);

        DOVirtual.DelayedCall(3f, () =>
        {
            RemoveHeart();
        });
        
    }

    /// <summary>
    /// Heart Panel에 하트 수 초기화
    /// </summary>
    /// <param name="heartCount">하트 수</param>
    public void InitHeartCount(int heartCount)
    {
        _heartCount = heartCount;
        _heartCountText.text = _heartCount.ToString();
    }

    public void RemoveHeart()
    {
        // 하트 사라지는 연출
        _heartRemoveImageObject.SetActive(true);
        _heartRemoveImageObject.transform.localScale = Vector3.zero;
        _heartRemoveImageObject.GetComponent<Image>().color = Color.white;
        
        _heartRemoveImageObject.transform.DOScale(3f, 1f);
        _heartRemoveImageObject.GetComponent<Image>().DOFade(0f, 1f);
        
        // 하트 수 텍스트 떨어지는 연출
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _heartCountText.rectTransform.DOAnchorPosY(-40f, 1f);
            _heartCountText.DOFade(0f, 1f).OnComplete(() =>
            {
                // 하트 개수 감소
                _heartCount--;
                _heartCountText.text = _heartCount.ToString();
                
                var textLength = _heartCountText.text.Length;
                GetComponent<RectTransform>().sizeDelta = new Vector2(100 + textLength * 30f, 100f);

                _heartCountText.rectTransform.DOAnchorPosY(40f, 0f);
                _heartCountText.rectTransform.DOAnchorPosY(0, 1f);
                _heartCountText.DOFade(1f, 1f);
            });
        });
        
    }
}
