// IntermediateTrigger.cs
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class IntermediateTrigger : MonoBehaviour
{
    /// <summary>Statisches Event, das die Bett-Trigger freischaltet.</summary>
    public static event Action OnDecisionPhaseReady;

    [Header("Optionaler Marker")]
    public GameObject highlightObject;

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null) Debug.LogError($"{name}: Collider fehlt!");
        col.enabled = false;                         // zunächst nicht betretbar
        if (highlightObject) highlightObject.SetActive(false);
    }

    void Start()
    {
        if (Level3Manager.Instance != null)
            Level3Manager.Instance.OnReadyToDecide += Activate;
        else
            Debug.LogError("IntermediateTrigger: Kein Level3Manager in Szene!");
    }

    void OnDestroy()
    {
        if (Level3Manager.Instance != null)
            Level3Manager.Instance.OnReadyToDecide -= Activate;
    }

    /// <summary>Nach beiden Dialogen: Trigger & Marker freischalten.</summary>
    void Activate()
    {
        col.enabled = true;
        if (highlightObject) highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Statisches Event feuern
        OnDecisionPhaseReady?.Invoke();
        // Und parallel nochmal den Manager, falls noch wer dran hängt
        Level3Manager.Instance.FireDecisionStage();

        col.enabled = false;
        if (highlightObject) highlightObject.SetActive(false);
        enabled = false;
    }
}
