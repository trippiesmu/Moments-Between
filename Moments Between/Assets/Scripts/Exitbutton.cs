using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // Diese Methode kann im Button-Inspector zugewiesen werden
    public void ExitGame()
    {
        // Editor-Modus beenden (nur sichtbar in Unity-Editor)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Im Build: Anwendung beenden
#endif
    }
}