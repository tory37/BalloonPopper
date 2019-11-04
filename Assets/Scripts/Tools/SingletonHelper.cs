using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonHelper
{
    public static bool SetupSingleton(MonoBehaviour instance, MonoBehaviour currentChecker)
    {
        if (instance != null)
        {
            instance = currentChecker;
            return true;
        } else
        {
            GameObject.Destroy(currentChecker);
            return false;
        }
    }
}
