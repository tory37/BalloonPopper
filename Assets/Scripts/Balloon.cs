using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField]
    private BalloonColor color = BalloonColor.BLUE;

    public BalloonColor Color {
        get { return color; }
    }
}
