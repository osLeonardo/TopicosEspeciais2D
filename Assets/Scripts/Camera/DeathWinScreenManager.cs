﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWinScreenManager : MonoBehaviour
{
    public void LoadLevel01()
    {
        SceneManager.LoadScene("Level01");
    }

    public void LoadLevel02()
    {
        SceneManager.LoadScene("Level02");
    }
}