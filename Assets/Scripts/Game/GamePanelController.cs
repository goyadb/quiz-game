using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GamePanelController : MonoBehaviour
{
    private GameObject _firstQuizCardObject;
    private GameObject _secondQuizCardObject;
    
    private List<QuizData> _quizDataList;
    
    private int _lastGeneratedQuizIndex;
    private int _lastStageIndex;
    
    private Queue<GameObject> _quizCardQueue = new Queue<GameObject>();

    public void AddQuizCardObject(QuizData? quizData, bool isInit = false)
    {
        // _quizCardQueue에 이미 Quiz Card Object가 있었다면,

        GameObject tempObject = null;
        
        // 1. 해당 오브젝트를 Deque 하고,
        if (_quizCardQueue.Count > 0 && !isInit)
        {
            tempObject = _quizCardQueue.Dequeue();
        }

        if (_quizCardQueue.Count > 0)
        {
            // 2. 가장 첫 번째 오브젝트를 Peek 해서 사이즈와 위치 조절하고
            var firstQuizCardObject = _quizCardQueue.Peek();
            firstQuizCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            firstQuizCardObject.transform.localScale = Vector3.one;
            firstQuizCardObject.transform.SetAsLastSibling();
            firstQuizCardObject.GetComponent<QuizCardController>().SetVisible(true);
        }
        
        // 3. 새로운 quizCardObject를 Enqueue 함
        if (quizData.HasValue)
        {
            var quizCardObject = ObjectPool.Instance.GetObject();
            quizCardObject.GetComponent<QuizCardController>().SetQuiz(quizData.Value, OnCompletedQuiz);
            quizCardObject.GetComponent<QuizCardController>().SetVisible(false);
            quizCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 160);
            quizCardObject.transform.localScale = Vector3.one * 0.9f;
            quizCardObject.transform.SetAsFirstSibling();
            _quizCardQueue.Enqueue(quizCardObject);
        }
        
        if (tempObject != null) 
            ObjectPool.Instance.ReturnObject(tempObject);
    }
    
    private void Start()
    {
        _lastStageIndex = UserInformations.LastStageIndex;
        InitQuizCards(_lastStageIndex);
    }

    private void InitQuizCards(int stageIndex)
    {
        _quizDataList = QuizDataController.LoadQuizData(stageIndex);
        
        AddQuizCardObject(_quizDataList[0], true);
        AddQuizCardObject(_quizDataList[1], true);
        
        // _firstQuizCardObject = ObjectPool.Instance.GetObject();
        // _firstQuizCardObject.GetComponent<QuizCardController>()
        //     .SetQuiz(_quizDataList[0], 0, OnCompletedQuiz);
        //
        // _secondQuizCardObject = ObjectPool.Instance.GetObject();
        // _secondQuizCardObject.GetComponent<QuizCardController>()
        //     .SetQuiz(_quizDataList[1], 1, OnCompletedQuiz);
        //
        // SetQuizCardPosition(_firstQuizCardObject, 0);
        // SetQuizCardPosition(_secondQuizCardObject, 1);
        //
        // // 마지막으로 생성된 퀴즈 인덱스
        // _lastGeneratedQuizIndex = 1;
    }

    private void OnCompletedQuiz(int cardIndex)
    {
        if (cardIndex < _quizDataList.Count - 2)
        {
            AddQuizCardObject(_quizDataList[cardIndex + 2]);            
        }
        else
        {
            AddQuizCardObject(null);
            if (cardIndex == _quizDataList.Count - 1)
            {
                // TODO: 스테이지 클리어
                _lastStageIndex++;
                
                // 새로운 스테이지 시작
                if (_lastStageIndex < Constants.MAX_STAGE_COUNT)
                    InitQuizCards(_lastStageIndex);
            }
        }
        
        // if (cardIndex >= _quizDataList.Count)
        // {
        //     if (_lastStageIndex >= Constants.MAX_STAGE_COUNT - 1)
        //     {
        //         // TODO: 올 클리어 연출
        //         GameManager.Instance.QuitGame();
        //     }
        //     else
        //     {
        //         // TODO: 스테이지 클리어 연출
        //         _lastStageIndex += 1;
        //         InitQuizCards(_lastStageIndex);
        //         return;   
        //     }
        // }
        // else
        // {
        //     // ChangeQuizCard();
        //     if (_quizDataList.Count > cardIndex + 1)
        //         AddQuizCardObject(_quizDataList[cardIndex + 1]);
        //     else
        //         AddQuizCardObject(null);
        // }
    }

    // private void SetQuizCardPosition(GameObject quizCardObject, int index)
    // {
    //     var quizCardTransform = quizCardObject.GetComponent<RectTransform>();
    //     if (index == 0)
    //     {
    //         quizCardTransform.anchoredPosition = new Vector2(0, 0);
    //         quizCardTransform.localScale = Vector3.one;
    //         quizCardTransform.SetAsLastSibling();
    //         
    //         quizCardObject.GetComponent<QuizCardController>()
    //             .SetVisible(true);
    //     }
    //     else if (index == 1)
    //     {
    //         quizCardTransform.anchoredPosition = new Vector2(0, 160);
    //         quizCardTransform.localScale = Vector3.one * 0.9f;
    //         quizCardTransform.SetAsFirstSibling();
    //         
    //         quizCardObject.GetComponent<QuizCardController>()
    //             .SetVisible(false);
    //     }
    // }

    // private void ChangeQuizCard()
    // {
        // if (_lastGeneratedQuizIndex >= Constants.MAX_QUIZ_COUNT) return;
        //
        // var temp = _firstQuizCardObject;
        // _firstQuizCardObject = _secondQuizCardObject;
        // _secondQuizCardObject = ObjectPool.Instance.GetObject();
        //
        // if (_lastGeneratedQuizIndex < _quizDataList.Count - 1)
        // {
        //     _lastGeneratedQuizIndex++;
        //     _secondQuizCardObject.GetComponent<QuizCardController>()
        //         .SetQuiz(_quizDataList[_lastGeneratedQuizIndex], _lastGeneratedQuizIndex, OnCompletedQuiz);
        // }
        //
        // SetQuizCardPosition(_firstQuizCardObject, 0);
        // SetQuizCardPosition(_secondQuizCardObject, 1);
        //
        // ObjectPool.Instance.ReturnObject(temp);
    // }
}