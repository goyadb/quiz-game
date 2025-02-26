using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PopupPanelController))]
public class StagePopupPanelController : MonoBehaviour
{
    [SerializeField] private GameObject stageCellPrefab;
    [SerializeField] private Transform contentTransform;

    [SerializeField] private GameObject scrollView;
    [SerializeField] private int cellColumnCount;

    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;

    private ScrollRect _scrollViewScrollRect;
    private RectTransform _scrollViewRectTransform;

    private List<(int index, StageCellButton[] stageCellButtons)> _visibleCells;    // 화면에 나타나는 셀 영역을 담고있는 리스트

    private int _maxStageCount = 100;

    private void Awake()
    {
        _scrollViewScrollRect = scrollView.GetComponent<ScrollRect>();
        _scrollViewRectTransform = scrollView.GetComponent<RectTransform>();
    }

    private void Start()
    {
        GetComponent<PopupPanelController>().SetTitleText("STAGE");

    }

    private (int start, int count) GetVisibleIndexRange()
    {
        var visibleRect = new Rect(
            _scrollViewScrollRect.content.anchoredPosition.x,
            _scrollViewScrollRect.content.anchoredPosition.y,
            _scrollViewRectTransform.rect.width,
            _scrollViewRectTransform.rect.height);

        var start = Mathf.FloorToInt(visibleRect.y / cellHeight);
        var visibleCount = Mathf.CeilToInt(visibleRect.height / cellHeight);

        // 버퍼 추가

        // Count 값 설정
        var count = Mathf.CeilToInt(_maxStageCount / cellColumnCount);
        count = Mathf.Min(count, start + visibleCount);

        return (start, count);
    }

    private StageCellButton CreateStageCellButton(int index, int row, int col)
    {
        var stageCellButtonObject = ObjectPool.Instance.GetObject();
        var stageCellButton = stageCellButtonObject.GetComponent<StageCellButton>();
        stageCellButton.SetStageCell(index, StageCellButton.StageCellType.Normal);

        // StageCellButton 위치 지정
        float centerIndex = 0;
        if (cellColumnCount % 2 == 0)
        {
            centerIndex = cellColumnCount / 2 - 0.5f;
        }
        else
        {
            centerIndex = cellColumnCount / 2;
        }

        var offset = col - centerIndex;

        var x = cellWidth * offset;
        var y = -cellHeight * row;
        stageCellButton.RectTransform.anchoredPosition = new Vector2(x, y);

        return stageCellButton;
    }

    private void ReloadData()
    {
        // Scroll View의 Content 사이즈 조절
        _scrollViewScrollRect.content.sizeDelta =
            new Vector2(0, Mathf.CeilToInt(_maxStageCount / cellColumnCount) * cellHeight);

        // 화면에 보이는 셀을 담고있는 _visibleCell 리스트 초기화
        _visibleCells = new List<(int index, StageCellButton[] stageCellButtons)>();

        ////////////////////////////////////////
        // 셀 생성

        // 만들어야 하는 셀 index 찾기
        var (start, count) = GetVisibleIndexRange();

        for (int i = start; i < count; i++)             // 셀 영역 하나씩 생성
        {
            List<StageCellButton> stageCellButtons = new List<StageCellButton>();

            for (int j = 0; j < cellColumnCount; j++)   // 한 줄의 셀 영역 생성
            {
                var index = i * cellColumnCount + j;
                if (index < _maxStageCount)
                {
                    // 셀 생성
                    var stageCellButton = CreateStageCellButton(index, i, j);
                    stageCellButtons.Add(stageCellButton);
                }
            }

            _visibleCells.Add((i, stageCellButtons.ToArray()));
        }
    }
}
