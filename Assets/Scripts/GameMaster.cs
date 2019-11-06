using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance = null;

    [SerializeField] private GameSceneToString sceneNames = new GameSceneToString();

    #region Data
    [SerializeField] private GameMode currentGameMode;
    private Dictionary<GameMode, int> modeToHighScoreDict = new Dictionary<GameMode, int>();
    private int latestScore = 0;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (setupSingleton())
        {
            setupHighScoreDict();
        }
    }

    private bool setupSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
        else
        {
            Destroy(this.gameObject);
            return false;
        }
    }

    private void setupHighScoreDict()
    {
        foreach(GameMode mode in System.Enum.GetValues(typeof(GameMode)))
        {
            modeToHighScoreDict.Add(mode, 0);
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

    public static void SetLatestScore(int score)
    {
        instance.latestScore = score;
    }

    public static void GoToScene(GameScene scene) {
        SceneManager.LoadScene(instance.sceneNames[scene]);
    }
}
