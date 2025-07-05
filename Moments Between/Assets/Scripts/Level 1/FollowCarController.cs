// FollowerCarController.cs
using UnityEngine;

public class FollowerCarController : MonoBehaviour
{
    [Tooltip("Zielpunkt, bis zu dem das Folger-Auto fahren soll")]
    public Transform targetPoint;
    [Tooltip("Fahrgeschwindigkeit des Folger-Autos")]
    public float speed = 5f;

    private bool moving = false;

    void Update()
    {
        if (!moving || targetPoint == null) return;

        Vector3 dir = (targetPoint.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            moving = false;
    }

    /// <summary>
    /// Startet die Bewegung des Folger-Autos zum Zielpunkt.
    /// </summary>
    public void StartMove()
    {
        if (targetPoint != null)
            moving = true;
    }
}