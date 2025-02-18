using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
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

// 퀴즈 카드의 위치 상태를 정의할 클래스가 반드시 구현할(약속) 메서드의 목록
public interface IQuizCardPositionState
{
    void Trasition(bool withAnimation, Action onComplete = null);
}

// 퀴즈 카드의 위치 상태 전이를 관리할 목적
public class QuizCardPositionStateContext
{
    private IQuizCardPositionState _currentState;

    public void SetState(IQuizCardPositionState state)
    {
        _currentState = state;
        _currentState.Trasition();
    }
}

public class QuizCardPositionState
{
    protected QuizCardController _quizCardController;
    protected RectTransform _rectTransform;
    protected CanvasGroup _canvasGroup;

    public QuizCardPositionState(QuizCardController quizCardController)
    {
        _quizCardController = quizCardController;
        _rectTransform = _quizCardController.gameObject.GetComponent<RectTransform>();
        _canvasGroup = _quizCardController.gameObject.GetComponent<CanvasGroup>();
    }
}

// 퀴즈 카드가 첫 번째 위치에 나타날 상태를 처리할 상태 클래스
public class QuizCardPositionStateFirst: QuizCardPositionState, IQuizCardPositionState
{
    public QuizCardPositionStateFirst(QuizCardController quizCardController) : base(quizCardController) { }

    public void Trasition(bool withAnimation, Action onComplete = null)
    {
        var animationDuration = (withAnimation) ? 0.2f : 0f;
        
        _rectTransform.DOAnchorPos(Vector2.zero, animationDuration);
        _rectTransform.DOScale(1f, animationDuration);
        _canvasGroup.DOFade(1f, animationDuration).OnComplete(() => onComplete?.Invoke());

        _rectTransform.SetAsLastSibling();
    }
}
// 퀴즈 카드가 두 번째 위치에 나타날 상태를 처리할 상태 클래스
public class QuizCardPositionStateSecond: QuizCardPositionState, IQuizCardPositionState
{
    public QuizCardPositionStateSecond(QuizCardController quizCardController) : base(quizCardController) { }

    public void Trasition(bool withAnimation, Action onComplete = null)
    {
        throw new NotImplementedException();
    }
}
// 퀴즈 카드가 사라질 상태를 처리할 상태 클래스
public class QuizCardPositionStateRemove: QuizCardPositionState, IQuizCardPositionState
{
    public QuizCardPositionStateRemove(QuizCardController quizCardController) : base(quizCardController) { }

    public void Trasition(bool withAnimation, Action onComplete = null)
    {
        
    }
}

public class QuizCardController : MonoBehaviour
{
    [SerializeField] private GameObject frontPanel;
    [SerializeField] private GameObject correctBackPanel;
    [SerializeField] private GameObject incorrectBackPanel;
    
    // Front Panel
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private GameObject threeOptionButtons;
    [SerializeField] private GameObject oxButtons;
    
    // Incorrect Back Panel
    [SerializeField] private TMP_Text heartCountText;
    
    // Timer
    [SerializeField] private GoyaTimer timer;
    
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
        timer.OnTimeout = () =>
        {
            // TODO: 오답 연출
            SetQuizCardPanelActive(QuizCardPanelType.InCorrectBackPanel);
        };
    }

    public void SetVisible(bool isVisible)
    {
        if (isVisible)
        {
            timer.InitTimer();
            timer.StartTimer();
        }
        else
        {
            timer.InitTimer();
        }
    }

    public void SetQuiz(QuizData quizData, QuizCardDelegate onCompleted)
    {
        // 1. 퀴즈
        // 2. 설명
        // 3. 타입 (0: OX퀴즈, 1: 보기 3개 객관식)
        // 4. 정답
        // 5. 보기 (1,2,3)
        
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
        
        // Incorrect Back Panel
        heartCountText.text = GameManager.Instance.heartCount.ToString();
    }

    /// <summary>
    /// 퀴즈의 정답을 선택하기 위한 버튼
    /// </summary>
    /// <param name="buttonIndex"></param>
    public void OnClickOptionButton(int buttonIndex)
    {
        // Timer 일시 정시
        timer.PauseTimer();
        
        if (buttonIndex == _answer)
        {
            Debug.Log("정답!");
            // TODO: 정답 연출
            
            SetQuizCardPanelActive(QuizCardPanelType.CorrectBackPanel);
        }
        else
        {
            Debug.Log("오답~");
            // TODO: 오답 연출
            
            SetQuizCardPanelActive(QuizCardPanelType.InCorrectBackPanel);
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
                frontPanel.SetActive(false);
                correctBackPanel.SetActive(true);
                incorrectBackPanel.SetActive(false);
                
                correctBackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition = _incorrectBackPanelPosition;
                break;
            case QuizCardPanelType.InCorrectBackPanel:
                frontPanel.SetActive(false);
                correctBackPanel.SetActive(false);
                incorrectBackPanel.SetActive(true);
                
                correctBackPanel.GetComponent<RectTransform>().anchoredPosition = _correctBackPanelPosition;
                incorrectBackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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
            GameManager.Instance.heartCount--;
            heartCountText.text = GameManager.Instance.heartCount.ToString();
            
            SetQuizCardPanelActive(QuizCardPanelType.Front);
            
            // 타이머 초기화 및 시작
            timer.InitTimer();
            timer.StartTimer();
        }
        else
        {
            // 하트가 부족해서 다시도전 불가
            // TODO: 하트 부족 알림
        }
    }
    
    #endregion
}
