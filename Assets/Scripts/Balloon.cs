using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    public BalloonColor Color;

    private void Start()
    {
        GameplayManager.RegisterStartingBalloon(this);
    }

    public void TriggerPopAnimation()
    {
        GetComponent<Animator>().SetTrigger(GameMaster.GetBalloonPopTriggerKey());
    }

    public void SetColor(BalloonColor color)
    {
        GetComponent<Image>().color = GameplayMenuManager.GetBalloonColor(color);
    }
}
