using UnityEngine;
using UnityEngine.InputSystem;

public class FootballerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Camera cameraReference;

    private InputAction moveAction;
    private Vector3 direction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
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

    void FixedUpdate()
    {
        if (direction.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Translate(speed * Time.fixedDeltaTime * transform.forward, Space.World);
        }
    }
}
