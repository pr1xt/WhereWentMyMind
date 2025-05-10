using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MasterVolumeControl : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField sliderInput;
    public float maxValue = 1f;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 100f);
        slider.value = volume*100f;
        sliderInput.text = (100*volume).ToString();


        slider.onValueChanged.AddListener(OnSliderChanged);
        sliderInput.onEndEdit.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        sliderInput.text = value.ToString();
        PlayerPrefs.SetFloat("Volume", value/100f);
       
    }
    void OnInputChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            value = Mathf.Clamp(value, 0f, maxValue);
            slider.value = value; 
        }
        else
        {
            sliderInput.text =  slider.value.ToString();
        }
    }
}
