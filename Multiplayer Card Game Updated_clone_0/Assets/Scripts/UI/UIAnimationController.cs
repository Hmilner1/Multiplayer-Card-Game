using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public void OnMouseOverHand()
    {
        GameObject UIImage = this.gameObject;

        LeanTween.scale(UIImage, new Vector3(1.5f, 1.5f, 1.5f), .2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.rotateZ(UIImage, 1.5f, .2f).setEase(LeanTweenType.easeOutElastic);
    }

    public void OnMouseExitHand()
    {
        GameObject UIImage = this.gameObject;

        LeanTween.scale(UIImage, new Vector3(1, 1, 1), .2f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotateZ(UIImage, 0, .2f).setEase(LeanTweenType.easeOutElastic);
    }
}
