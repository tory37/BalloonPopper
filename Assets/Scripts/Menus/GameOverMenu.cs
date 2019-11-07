using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;

    private void Start()
    {
        scoreText.text = ScoreManager.GetRecentScore().ToString();
    }

    public void GoToMainMenu()
    {
        GameMaster.GoToScene(GameScene.MAIN_MENU);
    }
}
