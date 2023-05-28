using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void LoadLevel(int id)
    {
        if (id < 0)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(id);
        }
    }

    public void LoadData()
    {
        GameController.data.LoadGame();
    }
}
