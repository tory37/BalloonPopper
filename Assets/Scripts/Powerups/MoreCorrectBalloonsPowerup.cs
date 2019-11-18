﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreCorrectBalloonsPowerup : BasePowerup
{
    [SerializeField] private float duration = 0f;

    public void Trigger()
    {
        OnTrigger();
    }

    public override bool OnTrigger()
    {
        if (base.OnTrigger())
        {
            GameplayManager.AddExtraCorrectBalloon(duration);
            return true;
        }

        return false;
    }
}
