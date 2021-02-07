using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractionSlider : MonoBehaviour
{
    public static InteractionSlider instance;

    [SerializeField]Slider slider;
    [SerializeField] TextMeshProUGUI sliderNumberText;
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] GameObject sliderPanel;
    private void Awake() {
        instance = this;
    }

    void Start()
    {
        sliderNumberText.text = slider.value.ToString();
    }


    Action<int> action;
    public void ShowSlider(Action<int> _action, int maxValue, string _header){
        slider.value = 0;
        action = _action;
        slider.maxValue = maxValue;
        slider.minValue = 0;
        header.text = _header;
        sliderPanel.SetActive(true);
    }

    public void OnValueChanged(){
        sliderNumberText.text = slider.value.ToString();
    }
    public void OnButtonPressed(){
        action?.Invoke((int)slider.value);
        HideSlider();
    }

    void HideSlider(){
        sliderPanel.SetActive(false);
        action = null;
    }
}
