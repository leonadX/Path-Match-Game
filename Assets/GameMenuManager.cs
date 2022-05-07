using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    // load home scene
    public void LoadMenu()
    {
        SceneManager.LoadScene("HomeScene");
    }
    
    // Resume Game
    public void ReplayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
