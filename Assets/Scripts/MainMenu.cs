﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayerGame()
    {
        SceneManager.LoadScene("Hole1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
