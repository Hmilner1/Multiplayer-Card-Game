using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroup : MonoBehaviour
{
    public List<MenuSection> menuSections;
    [SerializeField]
    private float animationTime;
    [SerializeField]
    private MenuSection startingSection;


    public void PopulateMenuList(MenuSection section)
    {
        if (menuSections == null)
        { 
            menuSections = new List<MenuSection>();
        }

        menuSections.Add(section);
        ResizeUI();
    }

    private void ResizeUI()
    {
        foreach (var section in menuSections)
        {
            if (section != startingSection)
            {
                LeanTween.scale(section.sectionObject, new Vector3(0, 0, 0), animationTime).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.rotateZ(section.sectionObject, 0, .2f).setEase(LeanTweenType.easeOutElastic);
            }
        }
    }


    public void OnSectionSelected(MenuSection section)
    {
        ResetGroups();
        LeanTween.scale(section.sectionObject, new Vector3(1f, 1f, 1f), animationTime).setEase(LeanTweenType.easeOutElastic);
        LeanTween.rotateZ(section.sectionObject, 1.5f, .2f).setEase(LeanTweenType.easeOutElastic);
        section.sectionObject.SetActive(true);
    }

    private void ResetGroups()
    {
        foreach (var section in menuSections)
        {
            if (section.sectionObject.activeInHierarchy == true)
            {
                LeanTween.scale(section.sectionObject, new Vector3(0, 0, 0), animationTime).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.rotateZ(section.sectionObject, 0, .2f).setEase(LeanTweenType.easeOutElastic);
            }
            section.sectionObject.SetActive(false);
        }
    }
}
