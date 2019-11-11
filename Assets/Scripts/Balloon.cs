using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    [SerializeField] private BalloonColor color;

    [Header("Components")]
    [SerializeField] private Animator outterAnim = null;
    [SerializeField] private Animator innerAnim = null;
    [SerializeField] private Image outterImage = null;

    [Header("Keys")]
    [SerializeField] private string triggerKey = "";

    private void Start()
    {
        GameplayManager.RegisterStartingBalloon(this);
    }

    public void TriggerPopAnimation()
    {
        outterAnim.SetTrigger(triggerKey);
        innerAnim.SetTrigger(triggerKey);
    }

    public BalloonColor GetColor()
    {
        return color;
    }

    public void SetColor(BalloonColor newColor)
    {
        color = newColor;
        outterImage.color = GameplayMenuManager.GetBalloonColor(color);
    }
}
