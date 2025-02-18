using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public struct QuizData
{
    public int index;
    public string question;
    public string description;
    public int type;
    public int answer;
    public string firstOption;
    public string secondOption;
    public string thirdOption;
}

public class QuizCardController : MonoBehaviour
{
    [SerializeField] private GameObject frontPanel;
    [SerializeField] private GameObject correctBackPanel;
    [SerializeField] private GameObject incorrectBackPanel;
    
    [SerializeField] private GameObject currectImage;
    
    // Front Panel
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private GameObject threeOptionButtons;
    [SerializeField] private GameObject oxButtons;
    
    // Incorrect Back Panel
    
    
    [SerializeField] GoyaTimer timer;
    [SerializeField] HeartPanelController heartPanelController;
    private enum QuizCardPanelType { Front, CorrectBackPanel, InCorrectBackPanel }

    public delegate void QuizCardDelegate(int cardIndex);
    private event QuizCardDelegate onCompleted;
    private int _answer;
    private int _quizCardIndex;
    
    private Vector2 _correctBackPanelPosition;
    private Vector2 _incorrectBackPanelPosition;
    

    private void Awake()
    {
        // 숨겨진 패널의 좌표 저장
        _correctBackPanelPosition = correctBackPanel.GetComponent<RectTransform>().anchoredPosition;
        _incorrectBackPanelPosition = incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Start()
    {
        //동작의 정의는 이곳. Ontimeout, 함수가 있는 곳이 다르기에 이런 식으로 설정
        timer.OnTimeout = () =>
        {
            SetQuizCardPanelActive(QuizCardPanelType.InCorrectBackPanel);
        };

        

    }
    

    public void SetVisible(bool isVisible)
    {
        if (isVisible)
        {
            timer.InitTimer();
            timer.StartTimer();
            //문제를 셋액티브 트루
            frontPanel.SetActive(true);
            frontPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        }
        else
        {
            timer.InitTimer();
            //문제를 셋액티브 팔스
            frontPanel.SetActive(false);
        }
        
    }
    
    public void SetQuiz(QuizData quizData, QuizCardDelegate onCompleted)
    {
        // 1. 퀴즈
        // 2. 설명
        // 3. 타입 (0: OX퀴즈, 1: 보기 3개 객관식)
        // 4. 정답
        // 5. 보기 (1,2,3)
        timer.InitTimer();
        // 퀴즈 카드 인덱스 할당
        _quizCardIndex = quizData.index;
        
        // Front Panel 표시
        SetQuizCardPanelActive(QuizCardPanelType.Front);
        
        // 퀴즈 데이터 표현
        questionText.text = quizData.question;
        _answer = quizData.answer;
        descriptionText.text = quizData.description;

        if (quizData.type == 0)
        {
            // 3지선다 퀴즈
            threeOptionButtons.SetActive(true);
            oxButtons.SetActive(false);
            
            var firstButtonText = optionButtons[0].GetComponentInChildren<TMP_Text>();
            firstButtonText.text = quizData.firstOption;
            var secondButtonText = optionButtons[1].GetComponentInChildren<TMP_Text>();
            secondButtonText.text = quizData.secondOption;
            var thirdButtonText = optionButtons[2].GetComponentInChildren<TMP_Text>();
            thirdButtonText.text = quizData.thirdOption;
        }
        else if (quizData.type == 1)
        {
            // OX 퀴즈
            threeOptionButtons.SetActive(false);
            oxButtons.SetActive(true);
        }
        
        this.onCompleted = onCompleted;
        
        
        heartPanelController.InitHeartCount(GameManager.Instance.heartCount);
    }

    /// <summary>
    /// 퀴즈의 정답을 선택하기 위한 버튼
    /// </summary>
    /// <param name="buttonIndex"></param>
    public void OnClickOptionButton(int buttonIndex)
    {
        
        timer.PauseTimer();
        heartPanelController.InitHeartCount(GameManager.Instance.heartCount);
        if (buttonIndex == _answer)
        {
            Debug.Log("정답!");
            // TODO: 정답 연출
            //카드가 커지면서 흐려지는 이미지 하나를 따로 만드는.
            currectImage.SetActive(true);
            currectImage.transform.localScale = Vector3.one;
            currectImage.GetComponent<Image>().color = Color.white;
        
            currectImage.transform.DOScale(3f, 1f);
            currectImage.GetComponent<Image>().DOFade(0f, 1f).OnComplete(()=>
            {
                currectImage.SetActive(false);
                SetQuizCardPanelActive(QuizCardPanelType.CorrectBackPanel);
                DOVirtual.DelayedCall(0.2f, () =>timer.InitTimer());

            });


            
            
            
        }
        else
        {
            Debug.Log("오답~");
            // TODO: 오답 연출
            GetComponent<RectTransform>().DOPunchPosition(new Vector3(20f, 0, 0), 1f, 7).OnComplete(() =>
            {
                SetQuizCardPanelActive(QuizCardPanelType.InCorrectBackPanel);
                DOVirtual.DelayedCall(0.2f, () =>timer.InitTimer());
            });

            
            
            
        }
    }
    
    private void SetQuizCardPanelActive(QuizCardPanelType quizCardPanelType)
    {
        switch (quizCardPanelType)
        {
            case QuizCardPanelType.Front:
                frontPanel.SetActive(true);
                correctBackPanel.SetActive(false);
                incorrectBackPanel.SetActive(false);
                
                correctBackPanel.GetComponent<RectTransform>().anchoredPosition = _correctBackPanelPosition;
                incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition = _incorrectBackPanelPosition;
                break;
            case QuizCardPanelType.CorrectBackPanel:

                GetComponent<Image>().DOFade(0f, 0.2f);
                transform.DOScaleX(0, 0.2f).OnComplete(() =>
                {
                    frontPanel.SetActive(false);
                    correctBackPanel.SetActive(true);
                    incorrectBackPanel.SetActive(false);
                    
                    correctBackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition = _incorrectBackPanelPosition;

                    transform.DOScaleX(1, 0.2f);
                    GetComponent<Image>().DOFade(1, 0.2f);
                });
                
                
                    
                break;
            case QuizCardPanelType.InCorrectBackPanel:
                
                
                GetComponent<Image>().DOFade(0f, 0.2f);
                transform.DOScaleX(0, 0.2f).OnComplete(() =>
                {
                    frontPanel.SetActive(false);
                    correctBackPanel.SetActive(false);
                    incorrectBackPanel.SetActive(true);
                
                    correctBackPanel.GetComponent<RectTransform>().anchoredPosition = _correctBackPanelPosition;
                    incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.DOScaleX(1, 0.2f);
                    GetComponent<Image>().DOFade(1, 0.2f);
                });

                
                
                
                break;
        }    
    }
    
    public void OnClickExitButton()
    {
        
    }

    #region Correct Back Panel
    /// <summary>
    /// 다음 버튼 이벤트
    /// </summary>
    public void OnClickNextQuizButton()
    {
        onCompleted?.Invoke(_quizCardIndex);
    }
    
    #endregion

    #region Incorrect Back Panel

    /// <summary>
    /// 다시도전 버튼 이벤트
    /// </summary>
    public void OnClickRetryQuizButton()
    {
        if (GameManager.Instance.heartCount > 0)
        {
            
            heartPanelController.RemoveHeart(() =>
            {
                timer.InitTimer();
                timer.StartTimer();
                
                GetComponent<Image>().DOFade(0f, 0.2f);
                transform.DOScaleX(0, 0.2f).OnComplete(() =>
                {
                    SetQuizCardPanelActive(QuizCardPanelType.Front);
                    transform.DOScaleX(1, 0.2f);
                    GetComponent<Image>().DOFade(1, 0.2f);
                });
                
                
                                
                
            });
            GameManager.Instance.heartCount--;

            
        }
        else
        {
            // 하트가 부족해서 다시도전 불가
            // TODO: 하트 부족 알림
            
            heartPanelController.EmptyHeart();
            Debug.Log("empty heart");
        }
    }
    
    #endregion
}
