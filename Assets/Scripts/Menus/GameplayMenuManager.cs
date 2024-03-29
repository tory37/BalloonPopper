﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NotificationType
{
    RIGHT,
    WRONG,
    START,
    GAME_OVER
}

public class GameplayMenuManager : MonoBehaviour
{
    private static GameplayMenuManager instance = null;

    #region Serialized Fields
    [Header("UI Elements")]
    [SerializeField] private Image displayBalloon = null;
    [SerializeField] private RectTransform timerSliderFill = null;
    [SerializeField] private RectTransform timeSliderInner = null;
    [SerializeField] private Image timerSliderImage = null;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private GameModeToRectTransform gameModePanels = new GameModeToRectTransform();
    [SerializeField] private Image notificationImage = null;

    [Header("Sprites")]
    [SerializeField] private NotificationTypeToSpriteList notificationAnimations = new NotificationTypeToSpriteList();
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private BalloonColorToColor balloonColors = new BalloonColorToColor();

    [Header("Vars")]
    [SerializeField] private Color timerSliderFullColor = Color.green;
    [SerializeField] private Color timerSliderEmptyColor = Color.red;
    [SerializeField] private float notificationAnimationSpeed = .2f;
    #endregion

    private IEnumerator currentNotificationCoroutine = null;

    #region Initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.disableAllBalloonPanels();
            ShowGamemodePanel(GameMaster.GetCurrentGameMode());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Score
    public static void UpdateScore(int score)
    {
        instance.scoreText.text = score.ToString(); 
    }
    #endregion

    #region Notifications
    public static void DisplayNotification(NotificationType type)
    {
        instance.stopNotificationCoroutine();
        instance.StartCoroutine(instance.showNotification(type));
    }

    private IEnumerator showNotification(NotificationType type)
    {
        List<Sprite> notificationFrames = notificationAnimations[type];

        for (int i = 0; i < notificationFrames.Count; i++)
        {
            instance.notificationImage.sprite = notificationFrames[i];
            yield return new WaitForSeconds(notificationAnimationSpeed);
            yield return new WaitForSeconds(0);
        }

        //instance.notificationImage.sprite = null;
        stopNotificationCoroutine();
    }

    private void stopNotificationCoroutine()
    {
        if (instance.currentNotificationCoroutine != null)
        {
            StopCoroutine(instance.currentNotificationCoroutine);
        }

        instance.notificationImage.sprite = instance.emptySprite;
    }
    #endregion

    #region Timer
    public static void SetTimer(float currentTimer, float startingTimer)
    {
        float percentage = currentTimer / startingTimer;
        float width = instance.timeSliderInner.rect.width;
        float right = width - (width * percentage);
        RectTransform rect = instance.timerSliderFill;
        rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
        instance.timerSliderImage.color = Color.Lerp(instance.timerSliderEmptyColor, instance.timerSliderFullColor, percentage);
    }
    #endregion

    #region Balloons
    public static void SetBalloon(Balloon clickedBallon, BalloonColor color)
    {
        clickedBallon.SetColor(color);
    }

    public static void SetDisplayBalloon(BalloonColor color)
    {
        instance.displayBalloon.color = instance.balloonColors[color];
    }

    private void disableAllBalloonPanels()
    {
        foreach (KeyValuePair<GameMode, RectTransform> entry in instance.gameModePanels)
        {
            entry.Value.gameObject.SetActive(false);
        }
    }

    public static void ShowGamemodePanel(GameMode mode)
    {
        foreach (KeyValuePair<GameMode, RectTransform> entry in instance.gameModePanels)
        {
            if (entry.Key == mode)
            {
                entry.Value.gameObject.SetActive(true);
            } else
            {
                entry.Value.gameObject.SetActive(false);
            }
        }
    }

    public static Color GetBalloonColor(BalloonColor color)
    {
        return instance.balloonColors[color];
    }
    #endregion
}
