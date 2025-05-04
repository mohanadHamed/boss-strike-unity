using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 720f; // degrees per second

    [SerializeField]
    private GameObject _explosionPrefab;

    private LayerMask playerLayer;

    private float _explodeRadius;

    private Vector3 _targetPosition;
    private float _speed = 10f;

    private bool _canExplode = false;

    private PlayerController _throwingPlayerController;

    private AudioSource _audioSource;

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetExplodingRocket(bool exploding)
    {
        _canExplode = exploding;
    }

    public void SetSpeed(float s)
    {
        _speed = s;
    }

    public void SetExplosionRadius(float radius)
    {
        _explodeRadius = radius;
    }

    public void SetThrowingPlayerController(PlayerController playerController)
    {
        _throwingPlayerController = playerController;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        // Move toward target
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }


    private IEnumerator OnCollisionEnter(Collision collision)
    {
        var collideWithPlayer = collision.collider.CompareTag("Player");
        var collideWithFloor = collision.collider.CompareTag("Floor");
        var collideWithBoss = collision.collider.CompareTag("Boss");

        if (!_canExplode)
        {
            if (collideWithPlayer)
            {
                var playerController = collision.collider.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    DisableVisibilityAndCollision();
                    PerformRocketAttackAgainstBoss(playerController);
                    Destroy(gameObject);
                }
            }
            yield break;
        }

        DisableVisibilityAndCollision();

        var shouldExplode = collideWithFloor || collideWithPlayer || collideWithBoss;

        if (shouldExplode)
        {
            // Instantiate explosion effect
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            var explosionAudioSource = explosion.GetComponent<AudioSource>();
            GameplayManager.Instance.PlaySfxAudio(explosionAudioSource, GameplayManager.Instance.ExplodeAudio);
        }

        if (collideWithPlayer)
        {
            var playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                yield return playerController.ReceiveHit();
            }
        }
        else if (collideWithFloor)
        {
            Collider[] players = Physics.OverlapSphere(transform.position, _explodeRadius, playerLayer);
            foreach (Collider col in players)
            {
                if (col.CompareTag("Player"))
                {
                    var playerController = col.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        yield return playerController.ReceiveHit();
                    }
                }
            }
        }
        else if (collideWithBoss)
        {
            GameplayManager.Instance.PlaySfxAudio(_audioSource, GameplayManager.Instance.BossHitAudio);
            if (_throwingPlayerController != null)
            {
                GameplayManager.Instance.IncreasePlayerScore(_throwingPlayerController.PlayerNumber, 1000);
            }

            yield return GameplayManager.Instance.DecreaseBossLives();
        }

        Destroy(gameObject);
    }

    private void DisableVisibilityAndCollision()
    {
        var rigidBocdy = GetComponent<Rigidbody>();
        var collider = GetComponent<Collider>();
        var renderer = GetComponentInChildren<Renderer>();

        rigidBocdy.isKinematic = true;
        collider.enabled = false;
        renderer.enabled = false;
    }

    private void PerformRocketAttackAgainstBoss(PlayerController playerController)
    {
        var targetPosition = GameplayManager.Instance.BossInstance.transform.position;
        Vector3 origin = transform.position + Vector3.up * 30f;
        var endPoint = targetPosition;

        Vector3 direction = (endPoint - origin).normalized;

        GameplayManager.Instance.PlaySfxAudio(_audioSource, GameplayManager.Instance.LaunchRocketAudio);

        GameObject rocket = Instantiate(GameplayManager.Instance.RocketPrefab, origin, Quaternion.LookRotation(direction, Vector3.up));
        rocket.layer = LayerMask.NameToLayer("PlayerRocket");

        // 2. Assign target to rocket
        RocketProjectile rocketScript = rocket.GetComponent<RocketProjectile>();
        if (rocketScript != null)
        {
            rocketScript.SetThrowingPlayerController(playerController);
            rocketScript.SetTarget(endPoint);
            rocketScript.SetSpeed(50);
            rocketScript.SetExplodingRocket(true);
            rocketScript.SetExplosionRadius(_explodeRadius);
        }
    }
}
