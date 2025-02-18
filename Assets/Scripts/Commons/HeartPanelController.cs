using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class HeartPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _heartRemoveImageObjcet;
    [SerializeField] private TMP_Text _heartCountText;

    [SerializeField] private AudioClip _heartRemoveAudioClip;
    [SerializeField] private AudioClip _heartAddAudioClip;
    [SerializeField] private AudioClip _heartEmptyAudioClip;
    
    private AudioSource _audioSource;
    
    private int _heartCount;
    
    
    //1. 하트 추가 연출
    //2. 하트 감소 연출
    //3. 하트 부족 연출


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _heartCount = GameManager.Instance.heartCount;
        
    }
    
    
    private void Start()
    {
        
        _heartRemoveImageObjcet.SetActive(false);
        
        InitHeartCount(_heartCount);

        // 두트윈 애니메이션 딜레이
        //DOVirtual.DelayedCall(0.5f, () => { RemoveHeart(); });
        
        
    }
    
    
    //Heart Panel에 하트 수 초기화
    public void InitHeartCount(int HeartCount)
    {
        _heartCount = HeartCount;
        _heartCountText.text = _heartCount.ToString();
    }


    private void ChangeTextAnimation(bool isAdd, Action onComplete = null)
    {
        float duration = 0.2f;
        float yPos = 40f;
        
        _heartCountText.rectTransform.DOAnchorPosY(-yPos, duration);
        _heartCountText.DOFade(0f, duration).OnComplete(() =>
        {
            if (isAdd)
            {
                var currentHeartCount = _heartCountText.text;
                _heartCountText.text = (int.Parse(currentHeartCount)+1).ToString();
                
            }
            else
            {
                
                var currentHeartCount = _heartCountText.text;
                
                _heartCountText.text = (int.Parse(currentHeartCount)-1).ToString();
                
                
            }
            
            var textLength = _heartCountText.text.Length;
            GetComponent<RectTransform>().sizeDelta = new Vector2(100+textLength*30f,100f);

            
            _heartCountText.rectTransform.DOAnchorPosY(yPos, 0);
            _heartCountText.rectTransform.DOAnchorPosY(0, duration);
            _heartCountText.DOFade(1,duration).OnComplete(() =>
            {
                
                

                DOVirtual.DelayedCall(0.5f, () =>
                {
                    onComplete?.Invoke();
                });
            });
            
            
        });
        
        
    }
    
    public void AddHeart(int heartCount)
    {
        //효과음 추가
        
        GameManager.Instance.heartCount ++;
        
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < 3; i++)
        {
            sequence.AppendCallback(() =>
            {
                ChangeTextAnimation(true);
                //효과음 재생

                if (UserInformations.IsPlaySFX)
                {
                    _audioSource.PlayOneShot(_heartAddAudioClip);
                }
            });
            sequence.AppendInterval(0.5f);

        }
        
        
    }

    public void EmptyHeart()
    {
        if (UserInformations.IsPlaySFX)
        {
            _audioSource.PlayOneShot(_heartEmptyAudioClip);
        }
        
        GetComponent<RectTransform>().DOPunchPosition(new Vector3(20f, 0, 0), 1f, 7);
        


    }
    
    
    public void RemoveHeart(Action onComplete=null)
    {
        if (UserInformations.IsPlaySFX)
        {
            _audioSource.PlayOneShot(_heartRemoveAudioClip);
        }
        
        
        _heartRemoveImageObjcet.SetActive(true);
        _heartRemoveImageObjcet.transform.localScale = Vector3.zero;
        _heartRemoveImageObjcet.GetComponent<Image>().color = new Color(233,45,176,255);
        
        _heartRemoveImageObjcet.transform.DOScale(3f, 1f);
        _heartRemoveImageObjcet.GetComponent<Image>().DOFade(0f, 1f).OnComplete(()=>_heartRemoveImageObjcet.SetActive(false));


        DOVirtual.DelayedCall(1f, () =>
        {
                ChangeTextAnimation(false, onComplete);
                
        });




    }
    
    
}
