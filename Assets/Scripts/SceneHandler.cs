using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public void OpenGame(){
        PlayerPrefs.SetInt("ShouldLoad", 0);
        SceneManager.LoadScene("Game");
    }

    public void OpenMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame(){
        PlayerPrefs.SetInt("ShouldLoad", 1);
        SceneManager.LoadScene("Game");
    }
}
