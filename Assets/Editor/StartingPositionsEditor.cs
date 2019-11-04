using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
[CustomEditor(typeof(BalloonStartingPositions))]
public class StartingPositionsEditor : Editor
{
    private Dictionary<GameMode, bool> folds = new Dictionary<GameMode, bool>()
        {
            { GameMode.TWO, false },
            { GameMode.THREE, false },
            { GameMode.FOUR, false },
            { GameMode.FIVE, false },
            { GameMode.SIX, false }
        };

    StartingPositionsEditor()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Dictionary<GameMode, List<Vector3>> positions = ((BalloonStartingPositions)target).StartingPositions;

        GUILayout.Label("Positions");

        foreach (KeyValuePair<GameMode, List<Vector3>> entry in positions)
        {
            folds[entry.Key] = EditorGUILayout.Foldout(folds[entry.Key], entry.Key.ToString());

            if (folds[entry.Key])
            {
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    entry.Value[i] = EditorGUILayout.Vector3Field("", entry.Value[i]);
                }
            }
        }
    }

}
