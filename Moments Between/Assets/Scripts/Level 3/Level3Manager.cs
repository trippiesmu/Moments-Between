// Level3Manager.cs
using UnityEngine;
using System;

public class Level3Manager : MonoBehaviour
{
    public static Level3Manager Instance { get; private set; }

    [Header("Zwischenschritt")]
    [Tooltip("Muss der Intermediate-Trigger erst betreten werden?")]
    public bool requireIntermediateStep = true;
    [Tooltip("Referenz auf das IntermediateTrigger-GameObject")]
    public GameObject intermediateTrigger;

    public event Action OnReadyToDecide;
    private bool talkedToA, talkedToB;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterDialogue(string patientID)
    {
        if (patientID == "A") talkedToA = true;
        if (patientID == "B") talkedToB = true;

        if (talkedToA && talkedToB)
        {
            if (requireIntermediateStep && intermediateTrigger != null)
                intermediateTrigger.SetActive(true);
            else
                OnReadyToDecide?.Invoke();
        }
    }

    public void FireDecisionStage()
    {
        OnReadyToDecide?.Invoke();
    }
}