using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class AnimationStates
{
    public const string Run = "IsRunning";
    public const string Hit = "Hit";
}

public class PlayerController : MonoBehaviour
{
    public PlayerNumber PlayerNumber => _playerNumber;

    [SerializeField]
    private PlayerNumber _playerNumber = PlayerNumber.PlayerOne;

    [SerializeField]
    private float _rotationSpeed = 720f; // degrees per second

    [SerializeField]
    private float _moveSpeed = 30f; // units per second

    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;

    private Animator _animator;
    private Rigidbody _rigidbody;

    private Vector3 _moveDirection = Vector3.zero;

    private bool _receivingHit = false;

    private readonly Dictionary<PlayerInput, KeyCode> _player1InputMappings = new()
    {
        { PlayerInput.MoveLeft, KeyCode.A },
        { PlayerInput.MoveRight, KeyCode.D },
        { PlayerInput.MoveUp, KeyCode.W },
        { PlayerInput.MoveDown, KeyCode.S },
        { PlayerInput.Attack, KeyCode.Space }
    };

    private readonly Dictionary<PlayerInput, KeyCode> _player2InputMappings = new()
    {
        { PlayerInput.MoveLeft, KeyCode.Keypad4 },
        { PlayerInput.MoveRight, KeyCode.Keypad6 },
        { PlayerInput.MoveUp, KeyCode.Keypad8 },
        { PlayerInput.MoveDown, KeyCode.Keypad2 },
        { PlayerInput.Attack, KeyCode.Return }
    };

    private Dictionary<PlayerNumber, Dictionary<PlayerInput, KeyCode>> _inputMappings;

    public KeyCode GetKeyInputForPlayerInput(PlayerInput input)
    {
        if (_inputMappings.TryGetValue(_playerNumber, out var playerInputMapping) && playerInputMapping.TryGetValue(input, out var key))
        {
            return key;
        }
        return KeyCode.None;
    }

    public IEnumerator ReceiveHit()
    {
        if(_receivingHit)
        {
            yield break;
        }

        _receivingHit = true;
        _moveDirection = Vector3.zero;
        _animator.ResetTrigger(AnimationStates.Run);
        _animator.SetTrigger(AnimationStates.Hit);
        _rigidbody.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        _animator.ResetTrigger(AnimationStates.Hit);

        if(GameplayManager.Instance.GetLivesForPlayer(_playerNumber) <= 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);
        _receivingHit = false;
    }


    private void Awake()
    {
        _inputMappings = new Dictionary<PlayerNumber, Dictionary<PlayerInput, KeyCode>>
        {
            { PlayerNumber.PlayerOne,  _player1InputMappings},
            { PlayerNumber.PlayerTwo,  _player2InputMappings}
        };
    }

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

        _meshRenderer.sharedMaterial = GameManager.Instance.GetMaterialForPlayer(_playerNumber);
    }

    private void Update()
    {
        if (GameplayManager.Instance.IsGameOver) return;

        HandleInput();

        GameplayManager.Instance.IncreasePlayerScore(_playerNumber, _moveDirection != Vector3.zero ? 2 : 1);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        if(_receivingHit)
        {
            return;
        }

        _moveDirection = Vector3.zero;

        if (Input.GetKey(_inputMappings[_playerNumber][PlayerInput.MoveRight]))
        {
            _moveDirection = Vector3.right;
            RotateToAngle(90f);
        }
        else if (Input.GetKey(_inputMappings[_playerNumber][PlayerInput.MoveLeft]))
        {
            _moveDirection = Vector3.left;
            RotateToAngle(270f);
        }
        else if (Input.GetKey(_inputMappings[_playerNumber][PlayerInput.MoveUp]))
        {
            _moveDirection = Vector3.forward;
            RotateToAngle(0f);
        }
        else if (Input.GetKey(_inputMappings[_playerNumber][PlayerInput.MoveDown]))
        {
            _moveDirection = Vector3.back;
            RotateToAngle(180f);
        }

        _animator.SetBool(AnimationStates.Run, _moveDirection != Vector3.zero);
    }

    private void MovePlayer()
    {
        if (_receivingHit || _moveDirection == Vector3.zero)
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            _rigidbody.linearVelocity = _moveDirection * _moveSpeed * Time.deltaTime;
        }
    }

    private void RotateToAngle(float angleY)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, angleY, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
