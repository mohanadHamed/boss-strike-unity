using System.Collections;
using System.Net;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _rotationSpeed = 360f; // degrees per second
    [SerializeField]
    private float _attackDistance = 4f;
    [SerializeField]
    private float _beforeAttackDelay = 1f;
    
    [SerializeField]
    private float _afterAttackDelay = 1f;

    [SerializeField]
    private float _afterRockAttackDelay = 3f;

    [SerializeField]
    private float _bossFlyHeight = 20f;

    [SerializeField]
    private float _strikeHeight = 50f;
    [SerializeField]
    private float _descendSpeed = 100f;
    [SerializeField]
    private float _strikeHitRadius = 5f;

    [SerializeField]
    private float _shadowYPos = -1.5f;

    [SerializeField]
    private GameObject _shadowPrefab;

    [SerializeField]
    private GameObject _rocketPrefab;

    [SerializeField]
    private float _rocketSpeed = 50f;

    [SerializeField]
    private float _explodeRadius = 15f;

    private GameObject _targetPlayer;
    private bool _isAttacking = false;
    private bool _allowAttack = true;

    private Vector3 _targetPos;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!_isAttacking)
        {
            AcquireTarget();
            float distance = Vector3.Distance(transform.position, _targetPlayer.transform.position);

            if (distance > _attackDistance)
            {
                MoveTowardsTarget();
            }
            else if(_allowAttack)
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    void AcquireTarget()
    {
        if (_targetPlayer != null) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        _targetPlayer = players[Random.Range(0, players.Length)];
    }

    void MoveTowardsTarget()
    {
        if (_targetPlayer == null) return;

        var targetPosition = _targetPlayer.transform.position;
        targetPosition.y = _bossFlyHeight; // Set the height of the boss

        // Move forward
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        transform.position += direction * _moveSpeed * Time.deltaTime;

        // Smoothly rotate toward the target
        Vector3 lookDir = direction;
        lookDir.y = 0f; // Keep rotation horizontal only
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator PerformAttack()
    {
        _isAttacking = true;
        _allowAttack = false;

        if (_targetPlayer == null)
        {
            _isAttacking = false;
            _allowAttack = true;
            yield break;
        }

        LockBeforeAttack();

        // Randomly choose between flame attack and eagle strike
        // 0 = flame attack, 1 = eagle strike, 2 = rocket attack
        var randomAttack = Random.Range(0, 3);
        switch(randomAttack)
        {
            case 0:
                Debug.Log("Performing Flame Attack");
                yield return PerformFlameAttack();
                break;
            case 1:
                Debug.Log("Performing Eagle Strike");
                yield return PerformEagleStrike();
                break;
            case 2:
            default:
                Debug.Log("Performing Rocket Attack");
                yield return AttackWith3Rockets();
                break;
        }

        _targetPlayer = null; // so boss picks a new one next time
        _allowAttack = true;
    }

    private IEnumerator AttackWith3Rockets()
    {
        yield return PerformRocketAttack(true);
        yield return new WaitForSeconds(0.5f);
        yield return PerformRocketAttack(true);
        yield return new WaitForSeconds(0.5f);
        yield return PerformRocketAttack(false);

        yield return new WaitForSeconds(_afterRockAttackDelay);

        _isAttacking = false;
    }

    private IEnumerator PerformFlameAttack()
    {
        yield return new WaitForSeconds(_beforeAttackDelay);

        Vector3 origin = transform.position + Vector3.up * 1f;
        var endPoint = _targetPos + Vector3.up * 10f;

        Vector3 direction = (endPoint - origin).normalized;

        // Draw fire ray
        _lineRenderer.SetPosition(0, origin);
        _lineRenderer.SetPosition(1, endPoint);
        _lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.2f); // Show fire for brief time
        _lineRenderer.enabled = false;

        _isAttacking = false;

        // Raycast flame
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, _attackDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var playerController = hit.collider.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    GameplayManager.Instance.DecreasePlayerLives(playerController);
                    yield return playerController.ReceiveHit();
                }
            }
        }

        yield return new WaitForSeconds(_afterAttackDelay);
    }

    private IEnumerator PerformEagleStrike()
    {

        // 1. Get player position and spawn shadow
        Vector3 targetPos = _targetPlayer.transform.position;
        var shadowInstance = Instantiate(_shadowPrefab, new Vector3(targetPos.x, _shadowYPos, targetPos.z), Quaternion.identity);

        // 2. Move boss high above target position
        Vector3 strikeStartPos = new Vector3(targetPos.x, targetPos.y + _strikeHeight, targetPos.z);
        transform.position = strikeStartPos;

        // 3. Wait 1 second to lock strike position
        yield return new WaitForSeconds(_beforeAttackDelay);

        // 4. Descend toward target position
        while (transform.position.y > _shadowYPos)
        {
            Vector3 downward = Vector3.down;
            transform.position += downward * _descendSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(shadowInstance); // Destroy shadow after strike

        yield return null;
        // Reset Boss height
        transform.position = new Vector3(transform.position.x, _bossFlyHeight, transform.position.z);

        _isAttacking = false;

        // 5. Check collision with player
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, _strikeHitRadius);
        foreach (Collider col in hitPlayers)
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

        yield return new WaitForSeconds(_afterAttackDelay);
    }

    private IEnumerator PerformRocketAttack(bool canExplode)
    {
        yield return new WaitForSeconds(_beforeAttackDelay);

        Vector3 origin = transform.position + Vector3.up * 30f;
        var endPoint = canExplode ? _targetPos : Vector3.zero;

        Vector3 direction = (endPoint - origin).normalized;

        GameObject rocket = Instantiate(_rocketPrefab, origin, Quaternion.LookRotation(direction, Vector3.up));
        rocket.layer = LayerMask.NameToLayer("Rocket");

        // 2. Assign target to rocket
        RocketProjectile rocketScript = rocket.GetComponent<RocketProjectile>();
        if (rocketScript != null)
        {
            rocketScript.SetTarget(endPoint);
            rocketScript.SetSpeed(_rocketSpeed);
            rocketScript.SetExplodingRocket(canExplode);
            rocketScript.SetExplosionRadius(_explodeRadius);
        }
    }



    private void LockBeforeAttack()
    {
        _targetPos = _targetPlayer.transform.position;
        var lookDir = (_targetPos - transform.position).normalized;
        lookDir.y = 0f; // Keep rotation horizontal only
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = targetRotation;
    }
}
