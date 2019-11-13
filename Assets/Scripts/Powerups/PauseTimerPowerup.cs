using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTimerPowerup : BasePowerup
{
    [SerializeField] private float pauseDuration = 0f;

    public void Trigger()
    {
        OnTrigger();
    }

    public override bool OnTrigger()
    {
        if (base.OnTrigger())
        {
            GameplayManager.PauseTimer(pauseDuration);
            return true;
        }

        return false;
    }
}
