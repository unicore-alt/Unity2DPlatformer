using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _diveSpeed = 5f;

    [Header("Combat Settings")]
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Patrol Area")]
    [SerializeField] private Transform _patrolCenter;
    [SerializeField] private float _patrolRadius = 3f;

    [Header("Sprite Settings")]
    [SerializeField] private float _flipThreshold = 0.2f;
    [SerializeField] private float _targetReachThreshold = 0.01f;

    [Header("Gizmos")]
    [SerializeField] private float _gizmoPointRadius = 0.2f;
    [SerializeField] private Color _detectionGizmoColor = Color.red;
    [SerializeField] private Color _patrolGizmoColor = Color.cyan;

    private Animator _animator;
    private static readonly int FlyAnim = Animator.StringToHash("isFly");
    private static readonly int AttackAnim = Animator.StringToHash("isAttack");

    private Vector2 _patrolCenterPosition;
    private Vector2 _patrolTargetPosition;
    private bool _isDiving;
    private Transform _player;
    private float _attackTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _patrolCenterPosition = _patrolCenter ? (Vector2)_patrolCenter.position : (Vector2)transform.position;
        SetNewPatrolTarget();
    }

    private void Update()
    {
        switch (_isDiving)
        {
            case true:
                _animator.SetBool(FlyAnim, false);
                DiveAttack();
                break;
            default:
                _animator.SetBool(FlyAnim, true);
                _animator.SetBool(AttackAnim, false);
                Patrol();
                DetectPlayer();
                break;
        }
    }

    private void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, _patrolTargetPosition, _patrolSpeed * Time.deltaTime);

        if (Vector2.SqrMagnitude((Vector2)transform.position - _patrolTargetPosition) < _targetReachThreshold)
            SetNewPatrolTarget();
    }

    private void SetNewPatrolTarget()
    {
        float randomX = Random.Range(-_patrolRadius, _patrolRadius);
        _patrolTargetPosition = new Vector2(_patrolCenterPosition.x + randomX, _patrolCenterPosition.y);

        FlipSprite((_patrolTargetPosition - (Vector2)transform.position).normalized);
    }

    private void DetectPlayer()
    {
        Collider2D detected = Physics2D.OverlapCircle(transform.position, _detectionRange, _playerLayer);
        if (detected == null) return;

        _player = detected.transform;
        _isDiving = true;
    }

    private void DiveAttack()
    {
        if (_player == null)
        {
            ResetToPatrol();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        bool isInAttackRange = distanceToPlayer <= _attackRange;

        _animator.SetBool(AttackAnim, isInAttackRange);
        FlipSprite((_player.position - transform.position).normalized);

        switch (isInAttackRange)
        {
            case true:
            {
                _attackTimer += Time.deltaTime;
                if (!(_attackTimer >= _attackInterval)) return;
                if (_player.TryGetComponent(out PlayerHealth playerHealth))
                    playerHealth.TakeDamage(_damageAmount);

                _attackTimer = 0f;
                break;
            }
            default:
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.position, _diveSpeed * Time.deltaTime);
                _attackTimer = 0f;

                if (distanceToPlayer > _detectionRange)
                    ResetToPatrol();
                break;
            }
        }
    }

    private void ResetToPatrol()
    {
        _isDiving = false;
        _player = null;
        _animator.SetBool(FlyAnim, true);
        SetNewPatrolTarget();
    }

    private void FlipSprite(Vector2 direction)
    {
        if (!(Mathf.Abs(direction.x) > _flipThreshold)) return;
        float newScaleX = Mathf.Sign(direction.x) * Mathf.Abs(transform.localScale.x);
        if (!Mathf.Approximately(transform.localScale.x, newScaleX))
            transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _detectionGizmoColor;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        if (_patrolCenter == null) return;

        Gizmos.color = _patrolGizmoColor;
        Vector3 center = _patrolCenter.position;
        Vector3 left = center + Vector3.left * _patrolRadius;
        Vector3 right = center + Vector3.right * _patrolRadius;
        Gizmos.DrawLine(left, right);
        Gizmos.DrawWireSphere(left, _gizmoPointRadius);
        Gizmos.DrawWireSphere(right, _gizmoPointRadius);
    }
}