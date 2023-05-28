using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;

    [Header("Options")]
    public fpsLimit framerate = fpsLimit.nolimit;
    public int sensitivity = 1;
    public int soundVolume = 5;

    [Header("UI")]
    public TMP_Dropdown fps_dropdown;
    public Slider sens_slider;
    public TMP_Text sensVal;
    public Slider volume_slider;
    public TMP_Text volVal;

    bool hasChanged;


    void Awake()
    {
        GameController gc = GameController.instance;

        framerate = gc.framerate;
        sens_slider.value = sensitivity = (int) gc.lookSensitivity;
        sensVal.text = sensitivity.ToString();
        volume_slider.value = soundVolume = (int) gc.soundVolume;
        volVal.text = soundVolume.ToString();
        EnumToDropdown();

    }

    // Update is called once per frame
    void Update()
    {
        if (hasChanged) { Apply(); }
    }

    public void DropdownToEnum() {

        int fps = fps_dropdown.value;

        switch (fps)
        {
            case 0:
                framerate = fpsLimit.nolimit;
                break;

            case 1:
                framerate = fpsLimit.limit30;
                break;

            case 2:
                framerate = fpsLimit.limit60;
                break;

            case 3:
                framerate = fpsLimit.limit90;
                break;

            case 4:
                framerate = fpsLimit.limit120;
                break;
        }

        hasChanged = true;
    }

    public void SensSliderChanged()
    {
        sensitivity = (int)sens_slider.value;
        sensVal.text = sensitivity.ToString();
        hasChanged = true;
    }

    public void VolumeSliderChanged()
    {
        soundVolume = (int)volume_slider.value;
        volVal.text = soundVolume.ToString();
        hasChanged = true;
    }

    void EnumToDropdown()
    {
        switch (framerate)
        {
            case fpsLimit.nolimit:
                fps_dropdown.value = 0;
                break;

            case fpsLimit.limit30:
                fps_dropdown.value = 1;
                break;

            case fpsLimit.limit60:
                fps_dropdown.value = 2;
                break;

            case fpsLimit.limit90:
                fps_dropdown.value = 3;
                break;

            case fpsLimit.limit120:
                fps_dropdown.value = 4;
                break;
        }
    }

    void Apply()
    {
        GameController gc = GameController.instance;

        gc.framerate = framerate;
        Application.targetFrameRate = (int)framerate;
        gc.lookSensitivity = sensitivity;
        gc.soundVolume = soundVolume;
        mixer.SetFloat("volume", -20f + soundVolume * 4f);

        hasChanged = false;
    }
}
