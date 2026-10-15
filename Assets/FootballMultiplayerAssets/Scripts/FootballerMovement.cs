using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootballerMovement : NetworkBehaviour
{
    private GameObject cameraReference;
    [SerializeField] private float speed;
    [SerializeField] private float shotForce;
    [SerializeField] private Collider shotCollider;

    private InputAction moveAction;
    private InputAction shotAction;
    private Vector3 direction;
    private float shoting;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (GameObject.FindGameObjectWithTag("GroupFollowCamera").TryGetComponent(out CinemachineTargetGroup ctg))
        {
            cameraReference = ctg.gameObject;
            ctg.AddMember(transform, 1, 0.5f);
        }
        moveAction = InputSystem.actions.FindAction("Move");
        shotAction = InputSystem.actions.FindAction("Shot");
    }

    void Update()
    {
        if (!IsOwner) return;
        MoveAction();
        ShotAction();
    }

    private void MoveAction()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        Vector3 cameraForward = cameraReference.transform.forward;
        Vector3 cameraRight = cameraReference.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        direction = cameraForward * input.y + cameraRight * input.x;
    }

    private void ShotAction()
    {
        shoting = shotAction.ReadValue<float>();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (direction.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Translate(speed * Time.fixedDeltaTime * transform.forward, Space.World);
        }

        shotCollider.gameObject.SetActive(shoting == 1);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.CompareTag("Ball") && gameObject.TryGetComponent(out Rigidbody rb))
        {
            Vector3 directionVector = (gameObject.transform.position - transform.position).normalized;
            rb.AddForce(directionVector * shotForce, ForceMode.Impulse);
        }
    }
}
