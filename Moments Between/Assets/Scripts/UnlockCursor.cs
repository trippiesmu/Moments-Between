using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Cursor nicht gefangen
        Cursor.visible = true; // Cursor sichtbar machen
    }
}