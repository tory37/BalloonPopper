using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePowerup : MonoBehaviour
{
    [SerializeField] protected string _title = "BasePowerup";
    [SerializeField] protected float _coolDownSeconds = 0f;
    [SerializeField] protected Image disabledCover = null;
    [SerializeField] protected Text availableCount = null;
    [SerializeField] protected PowerupType type;

    protected bool isDisabled = false;

    protected void Start()
    {
        disabledCover.fillAmount = 0;
        setAvailableCount();
    }

    public virtual bool OnTrigger()
    {
        if (!isDisabled && GameplayManager.CanUserPowerup(type))
        {
            GameplayManager.UsePowerup(type);
            setAvailableCount();
            StartCoroutine(cooldown());
            return true;
        }

        return false;
    }

    public void Disable()
    {
        disabledCover.fillAmount = 1;
        isDisabled = true;
    }

    public void Enable()
    {
        disabledCover.fillAmount = 0;
        isDisabled = false;
    }

    protected IEnumerator cooldown()
    {
        Disable();
        float currentTimer = _coolDownSeconds;
        float timerDecrement = .1f;
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(timerDecrement);
            disabledCover.fillAmount = currentTimer / _coolDownSeconds;
            currentTimer -= timerDecrement;
        }
        Enable();
    }

    private void setAvailableCount()
    {
        availableCount.text = GameplayManager.GetAvailablePowerups(type).ToString();
    }
}
