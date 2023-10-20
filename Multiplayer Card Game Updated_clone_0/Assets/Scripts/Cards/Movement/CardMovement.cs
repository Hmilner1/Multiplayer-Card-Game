using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform parentAfterDrag;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent,true);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentAfterDrag,true);
        image.raycastTarget = true;
    }
}
