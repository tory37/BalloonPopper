using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField]
    private string name = "";

    public string Name {
        get { return name; }
    }
}
