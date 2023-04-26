using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


[DefaultExecutionOrder(2100)]
[RequireComponent(typeof (Slider))]

public class VolumeSliders : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }

    //public AudioMixer mixer;

    //[SerializeField]
    //    private string volumeName;

    [SerializeField]
    private TextMeshProUGUI volumeLabel;

    private void Awake()
    {
        //Debug.Log("VolumeSliders is Awake, volume: " + MusicManager.Instance.gameMusic.volume);

        slider.value = MusicManager.Instance.gameMusic.volume;
        UpdateValueOnChange(slider.value);

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValueOnChange(slider.value);
        });
    }

    public void UpdateValueOnChange(float newVolume)
    {
        //Debug.Log("UpdateValueOnChange, new volume: " + newVolume);
        //if(mixer != null)
        //    mixer.SetFloat(volumeName, Mathf.Log(newVolume) * 20f);
        //
        //MusicManager.Instance.gameMusic.volume = newVolume;

        MusicManager.Instance.gameMusic.volume = newVolume;

        if(volumeLabel != null)
            volumeLabel.text = Mathf.Round(newVolume * 100f) + "%";

    }

    public void ResetSlider(float newVolume)
    {
        slider.value = newVolume;
        //Debug.Log("ResetSlider, slider Volume changed to: " + slider.value);

        if(volumeLabel != null)
            volumeLabel.text = Mathf.Round(newVolume * 100f) + "%";

           
        //Debug.Log("ResetSlider, volumeLabel set to: " + volumeLabel.text);
    }
}
