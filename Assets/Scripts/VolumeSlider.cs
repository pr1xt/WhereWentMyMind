using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider Slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("Volume", Slider.value);
    }
}
