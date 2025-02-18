using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GamePanelController : MonoBehaviour
{
    
    private List<QuizData> _quizDataList;
    
    
    private int _lastStageIndex;
    
    private Queue<GameObject> _quizCardObjectQueue = new Queue<GameObject>();
   
    
    public void AddQuizCardObjcect(QuizData? quizData, bool isInit = false)
    {
        //isInit이 false일 경우 애니메이션 재생

        GameObject tempObject = null;
        
        //처음에 애니메이션을 진행하고 그 다음 아래 내용들 진행.

        if (!isInit)
        {
            var animationCard = _quizCardObjectQueue.Peek();
            animationCard.GetComponent<RectTransform>().DOAnchorPosY(-200f, 1f);
            animationCard.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            {
                if (_quizCardObjectQueue.Count > 0)
                {
            
                    tempObject = _quizCardObjectQueue.Dequeue();
            
                }
            
            
                if (_quizCardObjectQueue.Count > 0)
                {
                    var firstQuizCardObjcect = _quizCardObjectQueue.Peek();
                    //firstQuizCardObjcect.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    //firstQuizCardObjcect.transform.localScale = Vector3.one;   
                    firstQuizCardObjcect.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.5f);
                    firstQuizCardObjcect.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                    firstQuizCardObjcect.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                    {
                        firstQuizCardObjcect.GetComponent<QuizCardController>().SetVisible(true);

                        
                    });
                    
                    firstQuizCardObjcect.transform.SetAsLastSibling();
                    
                }
            
                //3 새로운 카드 뒤에 만들기
                if (quizData.HasValue)
                {
                    var quizCardObject = ObjectPool.Instance.GetObject();
                    quizCardObject.GetComponent<QuizCardController>().SetQuiz(quizData.Value, OnCompletedQuiz);
                    quizCardObject.GetComponent<QuizCardController>().SetVisible(false);
                    quizCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 160f);
                    quizCardObject.transform.localScale = Vector3.one*0.9f;
                    quizCardObject.GetComponent<CanvasGroup>().alpha = 0.5f;
                    quizCardObject.transform.SetAsFirstSibling();
                
                    _quizCardObjectQueue.Enqueue(quizCardObject);
            
                }
                
                
            });
            
            
            
                
                
            
            
            
            
        }
        else
        {
            //2. 뒤에 있는 카드 앞으로 옮기기
            if (_quizCardObjectQueue.Count > 0)
            {
                var firstQuizCardObjcect = _quizCardObjectQueue.Peek();
                firstQuizCardObjcect.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                firstQuizCardObjcect.transform.localScale = Vector3.one;
                firstQuizCardObjcect.GetComponent<CanvasGroup>().alpha = 1f;
                firstQuizCardObjcect.transform.SetAsLastSibling();
                firstQuizCardObjcect.GetComponent<QuizCardController>().SetVisible(true);
            
            }
        
            //3 새로운 카드 뒤에 만들기
            if (quizData.HasValue)
            {
                var quizCardObject = ObjectPool.Instance.GetObject();
                quizCardObject.GetComponent<QuizCardController>().SetQuiz(quizData.Value, OnCompletedQuiz);
                quizCardObject.GetComponent<QuizCardController>().SetVisible(false);
                quizCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 160f);
                quizCardObject.transform.localScale = Vector3.one*0.9f;
                quizCardObject.GetComponent<CanvasGroup>().alpha = 0.5f;
                quizCardObject.transform.SetAsFirstSibling();
                
                _quizCardObjectQueue.Enqueue(quizCardObject);

            }
        }
        
       
        

        if (tempObject != null)
        {
            ObjectPool.Instance.ReturnObject(tempObject);
        }
        
    }
    

    private void Start()
    {
        _lastStageIndex = UserInformations.LastStageIndex;
        InitQuizCards(_lastStageIndex);
    }

    
    
    
    private void InitQuizCards(int stageIndex)
    {
        _quizDataList = QuizDataController.LoadQuizData(stageIndex);
        
        AddQuizCardObjcect(_quizDataList[0], true);
        AddQuizCardObjcect(_quizDataList[1], true);
        
        
    }

    private void OnCompletedQuiz(int cardIndex)
    {
        if (cardIndex < _quizDataList.Count - 2)
        {
            AddQuizCardObjcect(_quizDataList[cardIndex+2]);
        }
        else
        {
            AddQuizCardObjcect(null);

            if (cardIndex == _quizDataList.Count - 1)
            {
                //스테이지 클리어 연출
                _lastStageIndex++;

                //새로운 스테이지 시작
                if (_lastStageIndex<Constants.MAX_STAGE_COUNT)
                {
                    InitQuizCards(_lastStageIndex);
                    
                }
                
            }
        }
       
       
    }

    
}