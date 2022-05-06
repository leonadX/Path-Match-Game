using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI HighscoreText;

    // Start is called before the first frame update
    void Start()
    {
        int highscore = PlayerPrefs.GetInt("HighScore");
        if (highscore == 0)
            HighscoreText.text = "Best Record: -- seconds";
        else
            HighscoreText.text = "Best Record: " + highscore.ToString() + " seconds";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
