using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasMainMenu = null;
    [SerializeField]
    private GameObject canvasModeSelection = null;

    void Start()
    {
        GoToMainMenu();
    }

    public void GoToModeSelection()
    {
        canvasMainMenu.SetActive(false);
        canvasModeSelection.SetActive(true);
    }

    public void GoToMainMenu()
    {
        canvasModeSelection.SetActive(false);
        canvasMainMenu.SetActive(true);
    }

    public void onTwoModeClick()
    {
        goToGameplay(GameMode.TWO);
    }

    public void onThreeModeClick()
    {
        goToGameplay(GameMode.THREE);
    }

    public void onFourModeClick()
    {
        goToGameplay(GameMode.FOUR);
    }

    public void onFiveModeClick()
    {
        goToGameplay(GameMode.FIVE);
    }

    public void onSixModeClick()
    {
        goToGameplay(GameMode.SIX);
    }

    private void goToGameplay(GameMode mode)
    {
        GameMaster.SetGameMode(mode);
        GameMaster.GoToScene(GameScene.GAMEPLAY);
    }
}
