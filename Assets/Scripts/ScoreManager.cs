using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance = null;

    private int recentScore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static int GetRecentScore()
    {
        return instance.recentScore;
    }

    public static void ResetScore()
    {
        instance.recentScore = 0;
    }

    public static void IncrementRecentScore()
    {
        instance.recentScore++;
        GameplayMenuManager.UpdateScore(instance.recentScore);
    }
}
