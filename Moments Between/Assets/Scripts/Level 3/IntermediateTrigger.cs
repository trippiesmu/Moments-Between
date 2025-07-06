// IntermediateTrigger.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IntermediateTrigger : MonoBehaviour
{
    void Start()
    {
        // Hinweis: Dieses GameObject _nicht_ per Script deaktivieren,
        // sondern im Inspector das Häkchen bei "Active" entfernen.
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Level3Manager.Instance.FireDecisionStage();
        // Nach Auslösen ausblenden
        gameObject.SetActive(false);
    }
}