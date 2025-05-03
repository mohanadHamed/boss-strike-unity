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

    private GameObject targetPlayer;
    private bool isAttacking = false;
    private Vector3 _targetPos;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isAttacking)
        {
            AcquireTarget();
            float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (distance > _attackDistance)
            {
                MoveTowardsTarget();
            }
            else
            {
                StartCoroutine(PerformAttack());
            }
        }
    }

    void AcquireTarget()
    {
        if (targetPlayer != null) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        targetPlayer = players[Random.Range(0, players.Length)];
    }

    void MoveTowardsTarget()
    {
        if (targetPlayer == null) return;

        // Move forward
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        direction.y = 0f; // Keep movement horizontal only

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
        isAttacking = true;

        if (targetPlayer == null)
        {
            isAttacking = false;
            yield break;
        }

        yield return LockBeforeAttack();

        yield return PerformFlameAttack();

        targetPlayer = null; // so boss picks a new one next time
        isAttacking = false;
    }

    private IEnumerator PerformFlameAttack()
    {
        Vector3 origin = transform.position + Vector3.up * 1f;
        var endPoint = _targetPos + Vector3.up * 10f;

        Vector3 direction = (endPoint - origin).normalized;

        // Draw fire ray
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.2f); // Show fire for brief time
        lineRenderer.enabled = false;

        // Raycast flame
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, _attackDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var playerController = hit.collider.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    GameplayManager.Instance.DecreasePlayerLives(playerController.PlayerNumber);
                    yield return playerController.ReceiveHit();
                }
            }
        }

        yield return new WaitForSeconds(_afterAttackDelay);
    }

    private IEnumerator LockBeforeAttack()
    {
        _targetPos = targetPlayer.transform.position;
        var lookDir = (_targetPos - transform.position).normalized;
        lookDir.y = 0f; // Keep rotation horizontal only
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = targetRotation;
        yield return new WaitForSeconds(_beforeAttackDelay);
    }
}
