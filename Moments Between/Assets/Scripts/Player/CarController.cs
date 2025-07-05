// CarController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CarController : MonoBehaviour
{
    [Header("Drive Settings")]
    [Tooltip("Vorwärtsgeschwindigkeit des Autos")]
    public float speed = 10f;

    [HideInInspector]
    public bool canDrive = true;

    void Update()
    {
        if (canDrive)
        {
            // Moviert in Fahrtrichtung (lokal)
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }

    /// <summary>
    /// Lenkt das Auto nach rechts um 'angle' Grad.
    /// </summary>
    public void SteerRight(float angle)
    {
        transform.Rotate(Vector3.up * angle);
    }
}