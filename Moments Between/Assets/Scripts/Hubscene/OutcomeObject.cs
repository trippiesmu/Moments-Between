// OutcomeObject.cs
using UnityEngine;

/// <summary>
/// Marker für alle Hub-Effekt-Objekte (Left/Right).
/// </summary>
public class OutcomeObject : MonoBehaviour
{
    [Tooltip("ID des Flashback-Levels, z.B. 'Level1', 'Level2'…")]
    public string levelID;

    [Tooltip("True = Left-Outcome (None), False = Right-Outcome (ChoseRight)")]
    public bool isLeftOutcome;
}