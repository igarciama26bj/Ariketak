using UnityEngine;

public class Die : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Monster"))
            Destroy(gameObject);
    }
}
