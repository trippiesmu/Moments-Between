using UnityEngine;

/// <summary>
/// Fährt das Auto automatisch geradeaus, bis canDrive auf false gesetzt wird.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CarController : MonoBehaviour
{
    [Header("Drive Settings")]
    [Tooltip("Forward driving speed.")]
    public float speed = 10f;

    [HideInInspector]
    public bool canDrive = true;

    void Update()
    {
        if (canDrive)
        {
            // Immer vorwärts bewegen
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Lenkt das Auto nach links.
    /// </summary>
    public void SteerLeft(float angle)
    {
        transform.Rotate(Vector3.up * -angle);
    }

    /// <summary>
    /// Lenkt das Auto nach rechts.
    /// </summary>
    public void SteerRight(float angle)
    {
        transform.Rotate(Vector3.up * angle);
    }
}