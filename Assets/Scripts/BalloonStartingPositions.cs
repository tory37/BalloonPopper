using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BalloonStartingPositions : MonoBehaviour
{
    [SerializeField]
    private Dictionary<GameMode, List<Vector3>> positions = new Dictionary<GameMode, List<Vector3>>()
    {
        { GameMode.TWO, new List<Vector3> {
            new Vector3(),
            new Vector3()
        }},
        { GameMode.THREE, new List<Vector3> {
            new Vector3(),
            new Vector3(),
            new Vector3()
        } },
        { GameMode.FOUR, new List<Vector3> {
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3()
        } },
        { GameMode.FIVE, new List<Vector3> {
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3()
        } },
        { GameMode.SIX, new List<Vector3> {
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3()
        } }
    };

    public Dictionary<GameMode, List<Vector3>> StartingPositions
    {
        get
        {
            return positions;
        } 
        set
        {
            positions = value;
        }
    }
}
