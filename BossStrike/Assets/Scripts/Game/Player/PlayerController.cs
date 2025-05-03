using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 720f; // degrees per second

    [SerializeField]
    private float _moveSpeed = 30f; // units per second

    private Animator _animator;
    private Rigidbody _rigidbody;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the player object.");
        }
    }

    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleInput()
    {
        moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector3.right;
            RotateToAngle(90f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector3.left;
            RotateToAngle(270f);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = Vector3.forward;
            RotateToAngle(0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection = Vector3.back;
            RotateToAngle(180f);
        }

        _animator.SetBool("IsRunning", moveDirection != Vector3.zero);
    }

    void MovePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            _rigidbody.linearVelocity = moveDirection * _moveSpeed * Time.deltaTime;
        }
        else
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }

    void RotateToAngle(float angleY)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, angleY, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
