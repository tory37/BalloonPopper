using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private float startingTimer, startingTimerDecrement, timerDecrementIncreasePerBalloon, timeIncrementPerRightBalloon, timeDecrementPerWrongBalloon = 0;
    [SerializeField]
    private List<Vector3> startingPositions = new List<Vector3>();
    [SerializeField]
    private Vector3 displayBalloonPosition = new Vector3();
    [SerializeField]
    private List<Balloon> balloons = new List<Balloon>();
    [SerializeField]
    private RectTransform timerSliderFill = null;
    [SerializeField]
    private RectTransform timeSliderInner = null;
    [SerializeField]
    private Image timerSliderImage = null;
    [SerializeField]
    private Color timerSliderFullColor, timerSliderEmptyColor = new Color();
    [SerializeField]
    private Image notificationImage = null;
    [SerializeField]
    private List<Sprite> rightFrames, wrongFrames;
    [SerializeField]
    private float notificationAnimationSpeed = .2f;
    [SerializeField]
    private Sprite emptySprite = null;
    [SerializeField]
    private Text scoreText = null;

    private static GameplayManager instance;

    private float currentTimer = 1000;
    private float currentTimerDecrement = 0;
    private Balloon displayBalloon = null;
    private int rightCount = 0;
    private IEnumerator currentNotificationCoroutine = null;
    private List<Balloon> currentPoppableBalloons = new List<Balloon>();


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentTimerDecrement = startingTimerDecrement;
        for (int i = 0; i < instance.startingPositions.Count; i++)
        {
            instance.instantiateBalloon(instance.startingPositions[i]);
        }
        setNextDisplayBalloon();
    }

    void Update()
    {
        checkForBalloonPop();
        decrementTimer();
        checkGameOver();
    }

    private void checkForBalloonPop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 8, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 8)) {
                Balloon other = hit.collider.GetComponent<Balloon>();
                if (other)
                {
                    instance.onBalloonPopped(other);
                }
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
        Debug.Log("Balloon Popped! Clicked: " + balloon.Name + ", Display: " + instance.displayBalloon.Name);
 
        Vector3 position = balloon.transform.position;
        // If Balloon is correct
        if (balloon.Name == instance.displayBalloon.Name) {
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
        string randomPoppableBalloonName = instance.currentPoppableBalloons[Random.Range(0, instance.currentPoppableBalloons.Count - 1)].Name;
        int index = instance.balloons.FindIndex(balloonFromList => balloonFromList.Name == randomPoppableBalloonName);
        Debug.Log("New Balloon: " + instance.balloons[index].Name);
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
