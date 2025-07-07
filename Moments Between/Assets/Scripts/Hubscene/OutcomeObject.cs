// OutcomeObject.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OutcomeObject : MonoBehaviour
{
    [Tooltip("ID des Flashback-Levels, z.B. 'Level1', 'Level2'…")]
    public string levelID;

    [Tooltip("Outcome-Typ: ChoseLeft oder ChoseRight")]
    public FlashbackChoice outcomeType;

    void Start()
    {
        // standardmäßig ausblenden
        gameObject.SetActive(false);

        // falls die Choice schon existiert, nur das richtige Outcome zeigen
        if (GameManager.Instance.HasChoice(levelID))
        {
            var choice = GameManager.Instance.GetChoice(levelID);
            gameObject.SetActive(choice == outcomeType);
        }

        GameManager.Instance.OnChoiceChanged += HandleChoiceChanged;
    }

    private void HandleChoiceChanged(string id, FlashbackChoice choice)
    {
        if (id != levelID) return;
        gameObject.SetActive(choice == outcomeType);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnChoiceChanged -= HandleChoiceChanged;
    }
}