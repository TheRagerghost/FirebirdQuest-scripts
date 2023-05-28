using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Smartphone : MonoBehaviour
{

    ControlsManager controls;
    bool isHidden = false;
    Vector3 hidden;
    Vector3 shown;

    public float offset = .5f;
    public float time = 1f;

    public TMP_Text timeText;
    public TMP_Text locText;
    public Location currentLoc;

    void Start()
    {
        controls = GameController.controls;
        hidden = shown = transform.localPosition;
        shown.y += offset;

        controls.smartphoneToggle.started += ctx => isHidden = !isHidden;
    }

    void Update()
    {
        Toggle();
        PrintTime();
    }

    public void Toggle()
    {
        Vector3 target = isHidden ? shown : hidden;
        if (Vector3.Distance(transform.localPosition, target) > .01f)
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, time * Time.deltaTime);
    }

    void PrintTime()
    {
        int hours = System.DateTime.Now.Hour;
        int minutes = System.DateTime.Now.Minute;
        string timeString = hours.ToString() + ":" + minutes.ToString();

        if(timeText.text != timeString) { timeText.text = timeString; }
    }

    public void SetCurrentLocation(Location loc)
    {
        currentLoc = loc;
        locText.text = currentLoc.title;
    }
}
