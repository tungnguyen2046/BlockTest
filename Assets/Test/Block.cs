using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public int ID;
    public TextMeshProUGUI blockName;
    [SerializeField] Canvas canvas;
    [SerializeField] DropZone dropZone;
    [SerializeField] float snapDistance = 150;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Vector2 startPos;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if(eventData.pointerEnter.GetComponent<DropZone>() != dropZone)
        {
            rectTransform.anchoredPosition = startPos;
            if(dropZone.items.Contains(this)) dropZone.items.Remove(this);
        }
        else 
        {
            if(!dropZone.items.Contains(this)) dropZone.items.Add(this);
            dropZone.SortItems();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, dropZone.GetComponent<RectTransform>().anchoredPosition.y);
            SnapToClosetBlock();
        }
    }

    private void SnapToClosetBlock()
    {
        List<RectTransform> rectItems = new List<RectTransform>();
        foreach(Block item in dropZone.items)
        {
            if(item != this) rectItems.Add(item.GetComponent<RectTransform>());
        }

        float closetDistance = float.MaxValue;
        RectTransform closetRect = null;
        for(int i = 0; i < rectItems.Count; i++)
        {
            float distance = Vector2.Distance(rectTransform.anchoredPosition, rectItems[i].anchoredPosition);
            if(distance < closetDistance)
            {
                closetRect = rectItems[i];
                closetDistance = distance;
            }
        }

        if(closetDistance <= snapDistance)
        {
            float offset = rectTransform.anchoredPosition.x < closetRect.anchoredPosition.x ? -100 : 100;
            rectTransform.anchoredPosition = new Vector2(closetRect.anchoredPosition.x + offset, closetRect.anchoredPosition.y);
        }
    }
}
