using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct StartingPositions
{
    public GameMode mode;
    public List<Vector3> positions;
}

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

    [Header("Object Positions")]
    [SerializeField] private Vector3 displayBalloonPosition = new Vector3();
    [SerializeField] private List<Vector3> twoBalloonPostions = new List<Vector3>();
    [SerializeField] private List<Vector3> threeBalloonPostions = new List<Vector3>();
    [SerializeField] private List<Vector3> fourBalloonPostions = new List<Vector3>();
    [SerializeField] private List<Vector3> fiveBalloonPostions = new List<Vector3>();
    [SerializeField] private List<Vector3> sixBalloonPostions = new List<Vector3>();

    [Header("Objects")]
    [SerializeField] private List<Balloon> balloons = new List<Balloon>();

    [Header("UI Eleemnts")]
    [SerializeField] private RectTransform timerSliderFill = null;
    [SerializeField] private RectTransform timeSliderInner = null;
    [SerializeField] private Image timerSliderImage = null;
    [SerializeField] private Color timerSliderFullColor = Color.green;
    [SerializeField] private Color timerSliderEmptyColor = Color.red;
    [SerializeField] private Image notificationImage = null;
    [SerializeField] private List<Sprite> rightFrames = new List<Sprite>();
    [SerializeField] private List<Sprite> wrongFrames = new List<Sprite>();
    [SerializeField] private float notificationAnimationSpeed = .2f;
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private Text scoreText = null;
    #endregion

    private float currentTimer = 1000;
    private float currentTimerDecrement = 0;
    private Balloon displayBalloon = null;
    private int rightCount = 0;
    private IEnumerator currentNotificationCoroutine = null;
    private List<Balloon> currentPoppableBalloons = new List<Balloon>();
    private Dictionary<GameMode, List<Vector3>> startingPositions;

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
        instantiateStartingBalloons();
        setNextDisplayBalloon();
    }

    void Update()
    {
        checkForBalloonPop();
        decrementTimer();
        checkGameOver();
    }

    private void instantiateStartingBalloons()
    {
        startingPositions = new Dictionary<GameMode, List<Vector3>>();
        startingPositions.Add(GameMode.TWO, twoBalloonPostions);
        startingPositions.Add(GameMode.THREE, threeBalloonPostions);
        startingPositions.Add(GameMode.FOUR, fourBalloonPostions);
        startingPositions.Add(GameMode.FIVE, fiveBalloonPostions);
        startingPositions.Add(GameMode.SIX, sixBalloonPostions);

        List<Vector3> positions = instance.startingPositions[GameMaster.GetCurrentGameMode()];
        for (int i = 0; i < positions.Count; i++)
        {
            instance.instantiateBalloon(positions[i]);
        }
    }

    private void checkForBalloonPop()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 8, Color.blue);

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                checkForPop(Camera.main.ScreenPointToRay(touch.position), 8);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            checkForPop(Camera.main.ScreenPointToRay(Input.mousePosition), 8);
        }
    }

    private void checkForPop(Ray ray, float distance)
    {
        Debug.Log("Checking for pop");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8))
        {
            Debug.Log("hit");
            Balloon other = hit.collider.GetComponent<Balloon>();
            if (other)
            {
                instance.onBalloonPopped(other);
            }
        }
    }

    private void decrementTimer()
    {
        instance.currentTimer -= instance.currentTimerDecrement * Time.deltaTime;
        setTimerUI();
    }

    private void checkGameOver()
    {
        if (instance.currentTimer <= 0)
        {
            instance.doGameOver();
        }
    }

    public GameplayManager getInstance()
    {
        return instance;
    }

    private void onBalloonPopped(Balloon balloon)
    {
        Debug.Log("Balloon Popped! Clicked: " + balloon.Color + ", Display: " + instance.displayBalloon.Color);
 
        Vector3 position = balloon.transform.position;
        // If Balloon is correct
        if (balloon.Color == instance.displayBalloon.Color) {
            instance.currentTimer += instance.timeIncrementPerRightBalloon;
            instance.currentTimerDecrement += instance.timerDecrementIncreasePerBalloon;
            instance.rightCount += 1;
            setNotificationText(true);
            instance.SetPointUI();
        }
        // If Balloon is not correct
        else
        {
            instance.currentTimer -= instance.timeDecrementPerWrongBalloon;
            setNotificationText(false);
        }

        instance.currentPoppableBalloons.Remove(balloon);
        GameObject.Destroy(balloon.gameObject);

        instance.instantiateBalloon(position);
        setNextDisplayBalloon();
        setTimerUI();
    }

    private void instantiateBalloon(Vector3 position)
    {
        int randomBalloon = Random.Range(0, instance.balloons.Count);
        instance.currentPoppableBalloons.Add(GameObject.Instantiate(instance.balloons[randomBalloon], position, Quaternion.identity));
    }

    private void setNextDisplayBalloon()
    {
        if (instance.displayBalloon != null)
        {
            GameObject.Destroy(instance.displayBalloon.gameObject);
        }
        BalloonColor randomPoppableBalloonColor = instance.currentPoppableBalloons[Random.Range(0, instance.currentPoppableBalloons.Count - 1)].Color;
        int index = instance.balloons.FindIndex(balloonFromList => balloonFromList.Color == randomPoppableBalloonColor);
        instance.displayBalloon = GameObject.Instantiate(instance.balloons[index], instance.displayBalloonPosition, Quaternion.identity);
    }

    private void doGameOver()
    {
        Debug.Log("Game Over.  Correct Balloons: " + instance.rightCount);
        instance.currentTimer = 0;
        instance.currentTimerDecrement = 0;
}

    private void setTimerUI()
    {
        float percentage = instance.currentTimer / instance.startingTimer;
        float width = instance.timeSliderInner.rect.width;
        float right = width - (width * percentage);
        RectTransform rect = instance.timerSliderFill;
        rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
        instance.timerSliderImage.color = Color.Lerp(instance.timerSliderEmptyColor, instance.timerSliderFullColor, percentage);
    }

    private void setNotificationText(bool isRight)
    {
        stopNotificationCoroutine();

        if (isRight)
        {
            instance.currentNotificationCoroutine = showNotification(instance.rightFrames);
        } else
        {
            instance.currentNotificationCoroutine = showNotification(instance.wrongFrames);
        }

        StartCoroutine(instance.currentNotificationCoroutine);
    }

    private IEnumerator showNotification(List<Sprite> frames)
    {
        //destroy all game objects
        for (int i = 0; i < frames.Count; i++)
        {
            instance.notificationImage.sprite = frames[i];
            yield return new WaitForSeconds(notificationAnimationSpeed);
        }

        instance.notificationImage.sprite = null;
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

    private void SetPointUI()
    {
        instance.scoreText.text = instance.rightCount.ToString();
    }
}
