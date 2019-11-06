using System.Collections;
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

    [Header("UI Elements")]
    [SerializeField] private Image displayBalloon = null;
    [SerializeField] private RectTransform timerSliderFill = null;
    [SerializeField] private RectTransform timeSliderInner = null;
    [SerializeField] private Image timerSliderImage = null;
    [SerializeField] private Text scoreText = null;

    [Header("Sprites")]
    [SerializeField] private NotificationTypeToSpriteList notificationAnimations = new NotificationTypeToSpriteList();
    [SerializeField] private Image notificationImage = null;
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private BalloonColorToSprite balloonsSprites = new BalloonColorToSprite();

    [Header("Vars")]
    [SerializeField] private Color timerSliderFullColor = Color.green;
    [SerializeField] private Color timerSliderEmptyColor = Color.red;
    [SerializeField] private float notificationAnimationSpeed = .2f;

    private IEnumerator currentNotificationCoroutine = null;

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

    void Start()
    {

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
        Image balloonImage = clickedBallon.GetComponent<Image>();
        balloonImage.sprite = instance.balloonsSprites[color];
        clickedBallon.Color = color;
    }

    public static void SetDisplayBalloon(BalloonColor color)
    {
        Debug.Log("Changing display to " + color.ToString());
        instance.displayBalloon.sprite = instance.balloonsSprites[color];
        Debug.Log(instance.balloonsSprites[color]);
    }
    #endregion
}
