using UnityEngine;
using UnityEngine.UI;

public class SensivitySlider : MonoBehaviour
{
    public Slider Slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider.value = PlayerPrefs.GetFloat("sensX", 25);
        Slider.value = PlayerPrefs.GetFloat("sensY", 25);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("sensX", Slider.value);
        PlayerPrefs.SetFloat("sensY", Slider.value);
    }
}
