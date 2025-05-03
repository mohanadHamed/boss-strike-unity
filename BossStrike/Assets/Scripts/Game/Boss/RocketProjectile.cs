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

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        // Move toward target
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        // Rotate rocket to face target (assumes rocket points up by default)
       // Quaternion lookRotation = Quaternion.LookRotation(direction, transform.forward);
       // transform.rotation = lookRotation;
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
                    PerformRocketAttackAgainsBoss(playerController);
                    Destroy(gameObject);
                }
            }
            yield break;
        }

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;


        var shouldExplode = collideWithFloor || collideWithPlayer || collideWithBoss;

        if (shouldExplode)
        {
            // Instantiate explosion effect
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            //explosion.transform.localScale = new Vector3(_explodeRadius, _explodeRadius, _explodeRadius);
        }

        if (collideWithPlayer)
        {
            var playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                GameplayManager.Instance.DecreasePlayerLives(playerController);
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
                        GameplayManager.Instance.DecreasePlayerLives(playerController);
                        yield return playerController.ReceiveHit();
                    }
                }
            }
        }
        else if (collideWithBoss)
        {
            if (_throwingPlayerController != null)
            {
                GameplayManager.Instance.IncreasePlayerScore(_throwingPlayerController.PlayerNumber, 1000);
            }
            GameplayManager.Instance.DecreaseBossLives();
        }

        Destroy(gameObject);
    }

    private void PerformRocketAttackAgainsBoss(PlayerController playerController)
    {
        var targetPosition = GameplayManager.Instance.BossInstance.transform.position;
        Vector3 origin = transform.position + Vector3.up * 30f;
        var endPoint = targetPosition;

        Vector3 direction = (endPoint - origin).normalized;

        GameObject rocket = Instantiate(GameplayManager.Instance.RocketPrefab, origin, Quaternion.LookRotation(direction, Vector3.up));

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
