using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class SliderInput : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField sliderInput;
    private static readonly Regex validInput = new Regex(@"^(\d*\.?\d*)$");
    public float maxValue = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliderInput.text = slider.value.ToString("0.##");
        slider.onValueChanged.AddListener(OnSliderChanged);
        sliderInput.onValueChanged.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        sliderInput.text = value.ToString("0.##");
    }
    
    void OnInputChanged(string text)
    {
        if (validInput.IsMatch(text))
        {
            if (float.TryParse(text, out float value) && value <= maxValue)
            {
                sliderInput.text = value.ToString("0.##");
                slider.value = value;
            }
            else
            {
                sliderInput.text = maxValue.ToString("0.##");
                slider.value = maxValue;
            }
        }
        else
        {
            sliderInput.text = slider.value.ToString("0.##");
        }
    }
}