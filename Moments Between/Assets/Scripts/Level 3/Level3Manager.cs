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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Rufe das nach jedem Patienten-Dialog auf
    public void RegisterDialogue(string patientID)
    {
        if (patientID == "A") talkedToA = true;
        else if (patientID == "B") talkedToB = true;

        if (talkedToA && talkedToB)
            OnReadyToDecide?.Invoke();
    }

    // Wird vom IntermediateTrigger aufgerufen, um die DecisionAreas freizugeben
    public void FireDecisionStage()
        => OnReadyToDecide?.Invoke();
}