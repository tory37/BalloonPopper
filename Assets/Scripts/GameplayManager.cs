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

    #region Initialization
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

    // Start is called before the first frame update
    void Start()
    {
        currentTimerDecrement = startingTimerDecrement;
        setNextDisplayBalloon();
    }
    #endregion

    #region Update
    void Update()
    {
        modifyTimer(-instance.currentTimerDecrement * Time.deltaTime);
        checkGameOver();
    }
    #endregion

    #region Timer
    private void modifyTimer(float time)
    {
        currentTimer += time;
        GameplayMenuManager.SetTimer(currentTimer, startingTimer);
    }
    #endregion

    #region Balloons
    public static void RegisterStartingBalloon(BalloonColor color)
    {
        instance.currentBalloonColors.Add(color);
    }

    public void OnBalloonPopped(Balloon balloon)
    { 
        // Handle score and timer
        // If Balloon is correct
        if (balloon.Color == instance.currentDisplayColor) {
            instance.modifyTimer(instance.timeIncrementPerRightBalloon);
            instance.currentTimerDecrement += instance.timerDecrementIncreasePerBalloon;
            ScoreManager.IncrementRecentScore();
            GameplayMenuManager.DisplayNotification(NotificationType.RIGHT);
        }
        // If Balloon is not correct
        else
        {
            instance.modifyTimer(-instance.timeDecrementPerWrongBalloon);
            GameplayMenuManager.DisplayNotification(NotificationType.WRONG);
        }

        // Setup next balloon
        BalloonColor nextColor = EnumHelper.RandomExcluding<BalloonColor>(currentBalloonColors);
        currentBalloonColors.Remove(balloon.Color);
        currentBalloonColors.Add(nextColor);
        GameplayMenuManager.SetBalloon(balloon, nextColor);
        setNextDisplayBalloon();
    }

    private void setNextDisplayBalloon()
    {
        BalloonColor randomPoppableBalloonColor = instance.currentBalloonColors[Random.Range(0, instance.currentBalloonColors.Count)];
        currentDisplayColor = randomPoppableBalloonColor;
        GameplayMenuManager.SetDisplayBalloon(randomPoppableBalloonColor);
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
        //GameMaster.SetLatestScore(instance.rightCount);
        GameMaster.GoToScene(GameScene.GAME_OVER);
    }
    #endregion
}
