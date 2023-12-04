using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MenuSection : MonoBehaviour, IPointerClickHandler 
{
    public MenuGroup menuGroup;

    public GameObject sectionObject;
    private void Start()
    {
        menuGroup.PopulateMenuList(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menuGroup.OnSectionSelected(this);
    }

}
