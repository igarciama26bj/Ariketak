using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootballerMovement : NetworkBehaviour
{
    private GameObject cameraReference;
    [SerializeField] private float speed;
    [SerializeField] private float shotForce;
    [SerializeField] private float kickForce;
    [SerializeField] private Collider shotCollider;

    private InputAction moveAction;
    private InputAction shotAction;
    private Vector3 direction;

    private readonly NetworkVariable<bool> isShooting = new(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    private bool lastShotInput;

    public override void OnNetworkSpawn()
    {
        isShooting.OnValueChanged += OnShootingChanged;
        OnShootingChanged(false, isShooting.Value);

        if (!IsOwner) return;

        if (GameObject.FindGameObjectWithTag("GroupFollowCamera").TryGetComponent(out CinemachineTargetGroup ctg))
        {
            cameraReference = ctg.gameObject;
            ctg.AddMember(transform, 1, 0.5f);
        }
        
        moveAction = InputSystem.actions.FindAction("Move");
        shotAction = InputSystem.actions.FindAction("Shot");
    }

    public override void OnNetworkDespawn()
    {
        isShooting.OnValueChanged -= OnShootingChanged;
    }

    void Update()
    {
        if (!IsOwner) return;

        MoveAction();
        ShotAction();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (direction.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            transform.Translate(speed * Time.fixedDeltaTime * transform.forward, Space.World);
        }
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
        bool currentShotInput = shotAction.IsPressed();

        if (currentShotInput == lastShotInput)
            return;

        lastShotInput = currentShotInput;
        RequestSetShootingRpc(currentShotInput);
    }

    [Rpc(SendTo.Server)]
    private void RequestSetShootingRpc(bool shooting, RpcParams rpcParams = default)
    {
        if (rpcParams.Receive.SenderClientId != OwnerClientId)
            return;

        isShooting.Value = shooting;
    }

    private void OnShootingChanged(bool oldValue, bool newValue)
    {
        shotCollider.gameObject.SetActive(newValue);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !isShooting.Value) return;
        
        GameObject gameObject = other.gameObject;
        if (gameObject.CompareTag("Ball") && gameObject.TryGetComponent(out Rigidbody rb)) {
            Vector3 directionVector = (gameObject.transform.position - transform.position).normalized;
            rb.AddForce(directionVector * shotForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        GameObject gameObject = collision.gameObject;
        if (gameObject.CompareTag("Ball") && gameObject.TryGetComponent(out Rigidbody rb)) {
            Vector3 directionVector = (gameObject.transform.position - transform.position).normalized;
            rb.AddForce(directionVector * kickForce, ForceMode.Impulse);
        }
    }
}
