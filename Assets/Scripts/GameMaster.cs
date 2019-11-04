using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance = null;

    [SerializeField]
    private string gameplaySceneName = "";

    #region Data
    [SerializeField]
    private GameMode currentGameMode;
    private int latestScore = 0;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void SetGameMode(GameMode mode)
    {
        instance.currentGameMode = mode;
    }

    public static GameMode GetCurrentGameMode()
    {
        return instance.currentGameMode;
    }

    public static string GetGameplaySceneName()
    {
        return instance.gameplaySceneName;
    }

    public static void SetLatestScore(int score)
    {
        instance.latestScore = score;
    }
}
