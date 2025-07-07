// IntermediateTrigger.cs
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class IntermediateTrigger : MonoBehaviour
{
    /// <summary>
    /// Wird gefeuert, sobald der Spieler in den Zwischenpunkt eintritt.
    /// </summary>
    public static event Action OnDecisionPhaseReady;

    [Header("Optionaler Marker")]
    [Tooltip("Ein Objekt (z.B. Plane/Partikel/Icon), das erst sichtbar wird, wenn der Trigger aktiviert ist.")]
    public GameObject highlightObject;

    private Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError($"{name}: Collider fehlt!");
        // Collider initial deaktivieren (sichtbar=false → nicht betretbar)
        col.enabled = false;
    }

    void Start()
    {
        // Marker unsichtbar schalten
        if (highlightObject != null)
            highlightObject.SetActive(false);

        // Abonniere den Level3Manager – feuert OnReadyToDecide, wenn beide Dialoge fertig sind
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

    /// <summary>
    /// Schaltet diesen Trigger und den Marker frei.
    /// </summary>
    private void Activate()
    {
        gameObject.SetActive(true);
        col.enabled = true;
        if (highlightObject != null)
            highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Event feuern, damit alle DecisionAreaTrigger sich aktivieren
        OnDecisionPhaseReady?.Invoke();

        // Selbst wieder abschalten
        col.enabled = false;
        if (highlightObject != null)
            highlightObject.SetActive(false);
        enabled = false;
    }
}
