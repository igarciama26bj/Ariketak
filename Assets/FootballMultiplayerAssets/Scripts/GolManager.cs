using UnityEngine;
using UnityEngine.Events;

public class GolManager : MonoBehaviour
{
    [SerializeField] private FootballTeam team;
    [SerializeField] private GameObject ballPrefab;
    public UnityEvent<FootballTeam> golEvent;

    void OnTriggerEnter(Collider other)
    {
        GameObject ball = other.gameObject;
        if (ball.CompareTag("Ball"))
        {
            Destroy(ball);
            Instantiate(ballPrefab, new Vector3(0, 5f, 0), Quaternion.identity);
        }
    }
}
