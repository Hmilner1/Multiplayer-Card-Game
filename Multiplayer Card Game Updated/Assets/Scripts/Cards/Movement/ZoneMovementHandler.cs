using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoneMovementHandler : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        CardMovement draggableItem = dropped.GetComponent<CardMovement>();
        draggableItem.parentAfterDrag = transform;
    }
}
