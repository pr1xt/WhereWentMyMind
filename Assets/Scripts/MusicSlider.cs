using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        slider.value = volume;

        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value/100f);
        }
    }
}
