using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIResizeWindow : MonoBehaviour, IDragHandler
{
    private Vector2 originalLocalPointerPosition;
    private Vector2 originalSizeDelta;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the original position and size of the UI element
        originalSizeDelta = ((RectTransform)transform).sizeDelta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Get the current mouse position
        Vector2 mousePosition = eventData.position;
        // Convert the mouse position to world space
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // Update the position of the UI element
        transform.position = worldPosition;
    }
}
