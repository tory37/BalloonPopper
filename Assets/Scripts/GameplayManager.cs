using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager instance = null;

    #region Serialized Fields
    [Header("Starting Values")]
    [SerializeField] private float startingTimer = 0;
    [SerializeField] private float startingTimerDecrement = 0;
    [SerializeField] private float timerDecrementIncreasePerBalloon = 0;
    [SerializeField] private float timeIncrementPerRightBalloon = 0;
    [SerializeField] private float timeDecrementPerWrongBalloon = 0;
    #endregion

    private float currentTimer = 1000;
    private float currentTimerDecrement = 0;
    private List<BalloonColor> currentBalloonColors = new List<BalloonColor>();
    private BalloonColor currentDisplayColor;
    private Dictionary<PowerupType, int> availablePowerups = new Dictionary<PowerupType, int>();

    #region Initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //TODO: Load this from saved data
            instance.availablePowerups.Add(PowerupType.PAUSE_TIMER, 5);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance.currentTimerDecrement = instance.startingTimerDecrement;
        GameMaster.SetLatestScore(0);
        GameplayMenuManager.UpdateScore(0);
    }
    #endregion

    #region Update
    void Update()
    {
        modifyTimer(-instance.currentTimerDecrement * Time.deltaTime);
        instance.checkGameOver();
    }
    #endregion

    #region Timer
    private void modifyTimer(float time)
    {
        instance.currentTimer += time;
        GameplayMenuManager.SetTimer(instance.currentTimer, instance.startingTimer);
    }
    #endregion

    #region Balloons
    public static void RegisterStartingBalloon(Balloon balloon)
    {
        BalloonColor newColor = instance.getAvailableBalloonColor();
        balloon.SetColor(newColor);
        instance.currentBalloonColors.Add(newColor);
        instance.setNextDisplayBalloon();
    }

    public void OnBalloonPopped(Balloon balloon)
    { 
        AudioManager.PlayShiftedClip(SoundClip.BALLOON_POP);
        balloon.TriggerPopAnimation();
        // Handle score and timer
        // If Balloon is correct
        if (balloon.GetColor() == instance.currentDisplayColor) {
            instance.modifyTimer(instance.timeIncrementPerRightBalloon);
            instance.currentTimerDecrement += instance.timerDecrementIncreasePerBalloon;
            ScoreManager.IncrementRecentScore();
            GameplayMenuManager.DisplayNotification(NotificationType.RIGHT);
        }
        // If Balloon is not correct
        else
        {
            instance.modifyTimer(-instance.timeDecrementPerWrongBalloon * Time.deltaTime);
            GameplayMenuManager.DisplayNotification(NotificationType.WRONG);
        }

        // Setup next balloon

        BalloonColor nextColor = instance.getAvailableBalloonColor();
        instance.currentBalloonColors.Remove(balloon.GetColor());
        instance.currentBalloonColors.Add(nextColor);
        GameplayMenuManager.SetBalloon(balloon, nextColor);
        instance.setNextDisplayBalloon();
    }

    private void setNextDisplayBalloon()
    {
        int randomColorIndex = Random.Range(0, instance.currentBalloonColors.Count);
        BalloonColor randomPoppableBalloonColor = instance.currentBalloonColors[randomColorIndex];
        instance.currentDisplayColor = randomPoppableBalloonColor;
        GameplayMenuManager.SetDisplayBalloon(randomPoppableBalloonColor);
    }

    private BalloonColor getAvailableBalloonColor()
    {
        List<BalloonColor> possibleColors = EnumHelper.Without<BalloonColor>(instance.currentBalloonColors);
        BalloonColor newColor = possibleColors[Random.Range(0, possibleColors.Count)];
        return newColor;
    }
    #endregion

    #region Game Over
    private void checkGameOver()
    {
        if (instance.currentTimer <= 0)
        {
            instance.doGameOver();
        }
    }

    private void doGameOver()
    {
        //Debug.Log("Game Over.  Correct Balloons: " + instance.rightCount);
        instance.currentTimer = 0;
        instance.currentTimerDecrement = 0;
        GameMaster.SetLatestScore(ScoreManager.GetRecentScore());
        GameMaster.GoToScene(GameScene.GAME_OVER);
    }
    #endregion

    #region Mods
    private enum Stat
    {
        DECREMENT,
        DECREMENT_INCREASE_PER_BALLOON,
        INCREMENT_PER_RIGHT_BALLOON,
        DECREMENT_PER_WRONG_BALLOON
    }

    public static void PauseTimer(float duration)
    {
        instance.StartCoroutine(instance.modStat(Stat.DECREMENT, 0, duration));
        instance.StartCoroutine(instance.modStat(Stat.DECREMENT_PER_WRONG_BALLOON, 0, duration));
        instance.StartCoroutine(instance.modStat(Stat.INCREMENT_PER_RIGHT_BALLOON, 0, duration));
    }

    private IEnumerator modStat(Stat stat, float newValue, float duration)
    {
        float trueStat = 0f;

        switch (stat) {
            case Stat.DECREMENT:
                trueStat = instance.currentTimerDecrement;
                instance.currentTimerDecrement = newValue;
                yield return new WaitForSeconds(duration);
                instance.currentTimerDecrement = trueStat;
                break;
            case Stat.DECREMENT_INCREASE_PER_BALLOON:
                trueStat = instance.timerDecrementIncreasePerBalloon;
                instance.timerDecrementIncreasePerBalloon = newValue;
                yield return new WaitForSeconds(duration);
                instance.timerDecrementIncreasePerBalloon = trueStat;
                break;
            case Stat.DECREMENT_PER_WRONG_BALLOON:
                trueStat = instance.timeDecrementPerWrongBalloon;
                instance.timeDecrementPerWrongBalloon = newValue;
                yield return new WaitForSeconds(duration);
                instance.timeDecrementPerWrongBalloon = trueStat;
                break;
            case Stat.INCREMENT_PER_RIGHT_BALLOON:
                trueStat = instance.timeIncrementPerRightBalloon;
                instance.timeIncrementPerRightBalloon = newValue;
                yield return new WaitForSeconds(duration);
                instance.timeIncrementPerRightBalloon = trueStat;
                break;
        };
    }
    #endregion

    #region Powerups
    public static int GetAvailablePowerups(PowerupType type)
    {
        return instance.availablePowerups[type];
    }

    public static bool CanUserPowerup(PowerupType type)
    {
        return instance.availablePowerups[type] > 0;
    }

    public static void UsePowerup(PowerupType type)
    {
        instance.availablePowerups[type]--;
    }
    #endregion
}
