using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    [SerializeField]
    private Slider staminaSlider;
    public int stamina;

    private void Start()
    {
        stamina = 3;
        staminaSlider.value = stamina;
    }

    private void Update()
    {
        staminaSlider.value = stamina;
    }
}
