using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private string newGameScene = "SampleScene";

    public AudioSource mainChannel;
    public AudioClip bgMusic;


    private void Start()
    {
        mainChannel.PlayOneShot(bgMusic);
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreText.text = $"Most Waves Survived: {highScore}";
    }

    public void StartNewGame()
    {
        mainChannel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
