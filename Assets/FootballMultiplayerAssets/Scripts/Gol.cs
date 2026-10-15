using UnityEngine;
using UnityEngine.Events;

public class Gol : MonoBehaviour
{
    [SerializeField] private FootballTeam team;
    public UnityEvent<FootballTeam> golEvent;

    void OnTriggerEnter(Collider other)
    {
        GameObject ball = other.gameObject;
        if (ball.CompareTag("Ball") && ball.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ball.transform.position = new Vector3(0, 5, 0);
        }
    }
}
