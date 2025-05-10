using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicVolumeControl : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField sliderInput;
    public float maxValue = 100f;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        slider.value = volume*100f;
        sliderInput.text = (100*volume).ToString();

        slider.onValueChanged.AddListener(OnSliderChanged);
        sliderInput.onEndEdit.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        sliderInput.text = value.ToString();
        PlayerPrefs.SetFloat("MusicVolume", value);
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(value / 100f);
    }

    void OnInputChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            value = Mathf.Clamp(value, 0, maxValue);
            slider.value = value;
        }
        else
        {
            sliderInput.text = slider.value.ToString();
        }
    }
}
