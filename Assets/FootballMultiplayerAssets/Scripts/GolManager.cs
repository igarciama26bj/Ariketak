using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GolManager : NetworkBehaviour
{
    [SerializeField] private FootballTeam team;
    [SerializeField] private GameObject ballPrefab;
    public UnityEvent<FootballTeam> golEvent;

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        GameObject ball = other.gameObject;
        if (ball.CompareTag("Ball") && ball.TryGetComponent(out NetworkObject netObj))
        {
            netObj.Despawn(true);
            golEvent.Invoke(team);

            GameObject newBall = Instantiate(ballPrefab);
            newBall.GetComponent<NetworkObject>().Spawn();
            newBall.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
