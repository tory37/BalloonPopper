using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public BalloonColor Color;

    private void Start()
    {
        GameplayManager.RegisterStartingBalloon(Color);
    }
}
