// Level3Manager.cs
using UnityEngine;
using System;

public class Level3Manager : MonoBehaviour
{
    public static Level3Manager Instance { get; private set; }
    public event Action OnReadyToDecide;

    private bool talkedToA, talkedToB;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>Wird von PatientDialogueTrigger aufgerufen.</summary>
    public void RegisterDialogue(string patientID)
    {
        if (patientID == "A") talkedToA = true;
        else if (patientID == "B") talkedToB = true;

        if (talkedToA && talkedToB)
            OnReadyToDecide?.Invoke();
    }

    /// <summary>Wurde beides geredet?</summary>
    public bool IsReadyToDecide => talkedToA && talkedToB;
}