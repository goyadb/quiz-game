using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour
{
    private GameObject _firstQuizCardObject;
    private GameObject _secondQuizCardObject;
    
    private List<QuizData> _quizDataList;
    
    private int _lastGeneratedQuizIndex;
    private int _lastStageIndex;
    
    private void Start()
    {
<<<<<<< HEAD
        _lastStageIndex = UserInformation.LastStageIndex;
        
=======
        _lastStageIndex = UserInformations.LastStageIndex;
>>>>>>> upstream/main
        InitQuizCards(_lastStageIndex);
    }

    private void InitQuizCards(int stageIndex)
    {
<<<<<<< HEAD
        _quizDataList = QuizDataController.LoadQuizData(stageIndex+1);

        _firstQuizCardObject = ObjectPool.Instance.GetObject();
        _firstQuizCardObject.GetComponent<QuizCardController>().SetQuiz(_quizDataList[0], 0, OnCompletedQuiz);
        
        _secondQuizCardObject = ObjectPool.Instance.GetObject();
        _secondQuizCardObject.GetComponent<QuizCardController>().SetQuiz(_quizDataList[1], 1, OnCompletedQuiz);
=======
        _quizDataList = QuizDataController.LoadQuizData(stageIndex);
        
        _firstQuizCardObject = ObjectPool.Instance.GetObject();
        _firstQuizCardObject.GetComponent<QuizCardController>()
            .SetQuiz(_quizDataList[0], 0, OnCompletedQuiz);
        
        _secondQuizCardObject = ObjectPool.Instance.GetObject();
        _secondQuizCardObject.GetComponent<QuizCardController>()
            .SetQuiz(_quizDataList[1], 1, OnCompletedQuiz);
>>>>>>> upstream/main
        
        SetQuizCardPosition(_firstQuizCardObject, 0);
        SetQuizCardPosition(_secondQuizCardObject, 1);

        // 마지막으로 생성된 퀴즈 인덱스
        _lastGeneratedQuizIndex = 1;
    }

    private void OnCompletedQuiz(int cardIndex)
    {
        if (cardIndex >= Constants.MAX_QUIZ_COUNT - 1)
        {
<<<<<<< HEAD
            // Todo: 스테이지 클리어 연출
            _lastStageIndex++;
            InitQuizCards(_lastStageIndex);
            return;
=======
            if (_lastStageIndex >= Constants.MAX_STAGE_COUNT - 1)
            {
                // TODO: 올 클리어 연출
                
                GameManager.Instance.QuitGame();
            }
            else
            {
                // TODO: 스테이지 클리어 연출
                _lastStageIndex += 1;
                InitQuizCards(_lastStageIndex);
                return;   
            }
>>>>>>> upstream/main
        }
        ChangeQuizCard();
    }

    private void SetQuizCardPosition(GameObject quizCardObject, int index)
    {
        var quizCardTransform = quizCardObject.GetComponent<RectTransform>();
        if (index == 0)
        {
            quizCardTransform.anchoredPosition = new Vector2(0, 0);
            quizCardTransform.localScale = Vector3.one;
            quizCardTransform.SetAsLastSibling();
        }
        else if (index == 1)
        {
            quizCardTransform.anchoredPosition = new Vector2(0, 160);
            quizCardTransform.localScale = Vector3.one * 0.9f;
            quizCardTransform.SetAsFirstSibling();
        }
    }

    private void ChangeQuizCard()
    {
        if (_lastGeneratedQuizIndex >= Constants.MAX_QUIZ_COUNT) return;
        
        var temp = _firstQuizCardObject;
        _firstQuizCardObject = _secondQuizCardObject;
        _secondQuizCardObject = ObjectPool.Instance.GetObject();

        if (_lastGeneratedQuizIndex < _quizDataList.Count - 1)
        {
            _lastGeneratedQuizIndex++;
            _secondQuizCardObject.GetComponent<QuizCardController>()
                .SetQuiz(_quizDataList[_lastGeneratedQuizIndex], _lastGeneratedQuizIndex, OnCompletedQuiz);
        }
        
        SetQuizCardPosition(_firstQuizCardObject, 0);
        SetQuizCardPosition(_secondQuizCardObject, 1);
        
        ObjectPool.Instance.ReturnObject(temp);
    }
}