using UnityEngine;

public class Gol : MonoBehaviour
{
    [SerializeField] private GameTeam team;

    void OnTriggerEnter(Collider other)
    {
        GameObject ball = other.gameObject;
        if (ball.CompareTag("Ball") && ball.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ball.transform.position = new Vector3(0, 10, 0);
        }
    }
}
