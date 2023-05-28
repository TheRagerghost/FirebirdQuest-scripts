using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum fpsLimit
{
    nolimit = 0,
    limit30 = 30,
    limit60 = 60,
    limit90 = 90,
    limit120 = 120
}

[RequireComponent(typeof(ControlsManager))]
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(DataManager))]
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static ControlsManager controls;
    public static StateManager state;
    public static DataManager data;
    public AudioMixer mixer;

    [Header("Options")]
    public fpsLimit framerate;
    public float lookSensitivity = 1f;
    public float soundVolume = 5f;

    [Header("Modificators")]
    [Range(.1f, 2.0f)]
    public float mod_wall_time = .7f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);

            controls = GetComponent<ControlsManager>();
            state = GetComponent<StateManager>();
            data = GetComponent<DataManager>();
            Application.targetFrameRate = (int)framerate;
            mixer.SetFloat("volume", -20f + soundVolume * 4f);
        }

        state.LoadLevel(1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
