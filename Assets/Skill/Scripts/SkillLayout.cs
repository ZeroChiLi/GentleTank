using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLayout : MonoBehaviour
{
    public int skillCount;

    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rectTransform;
    private Vector2 leftDownPosition;
    private Vector2 cellSize;
    private Vector2 spacing;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        leftDownPosition = rectTransform.offsetMin;
        cellSize = gridLayoutGroup.cellSize;
        spacing = gridLayoutGroup.spacing;


        Debug.Log("rectTransform.anchoredPosition " + rectTransform.anchoredPosition);
        Debug.Log("rectTransform.localPosition " + rectTransform.localPosition);
        Debug.Log("rectTransform.offsetMin " + rectTransform.offsetMin);
        Debug.Log("rectTransform.pivot " + rectTransform.pivot);
        Debug.Log("rectTransform.position " + rectTransform.position);
        Debug.Log("rectTransform.rect " + rectTransform.rect);
        Debug.Log("rectTransform.sizeDelta " + rectTransform.sizeDelta);

    }

    public bool ContainPosition(Vector2 position)
    {
        //for (int i = 0; i < skillCount; i++)
        {
            Rect cellRect = new Rect(rectTransform.offsetMin.x, rectTransform.offsetMin.y + rectTransform.rect.height, cellSize.x, cellSize.y);
            if (cellRect.Contains(position))
            {
                Debug.Log("On Button");
            }
        }
        //Debug.Log("Left Down Position : " + );
        //gridLayoutGroup.cellSize;
        return false;
    }

}
