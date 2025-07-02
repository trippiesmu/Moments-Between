using UnityEngine;
using System;

public enum FlashbackChoice { None, ChoseLeft, ChoseRight }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Aktuelle Entscheidung im Flashback-Level
    public FlashbackChoice flashbackChoice = FlashbackChoice.None;

    // Event, das ausgelöst wird, wenn flashbackChoice geändert wird
    public event Action<FlashbackChoice> OnFlashbackChoiceChanged;

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

    /// <summary>
    /// Setzt eine Entscheidung und feuert das Event.
    /// </summary>
    public void SetFlashbackChoice(FlashbackChoice choice)
    {
        flashbackChoice = choice;
        OnFlashbackChoiceChanged?.Invoke(choice);
    }
}