using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SlimeEnemy : MonoBehaviour, IDamageable
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 3f;
    public float attackCooldown = 1.5f;
    public float attackRange = 1.5f;
    public float jumpForce = 7f;
    public float jumpSpeed = 10f;
    public Sprite diedSprite;

    private SpriteRenderer spriteRenderer;
    private int currentPoint = 0;
    private Transform player;
    private bool chasing = false;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 startPosition;

    private float lastAttackTime = -Mathf.Infinity;
    private bool isJumpingToPlayer = false;
    private Vector2 jumpTargetPosition;
    private bool hasAttacked = false;

    private bool isDied = false;

    public GameObject keyObject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDied) return;
        if (isJumpingToPlayer) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            chasing = true;
        }
        else if (distanceToPlayer > detectionRadius + 1f)
        {
            chasing = false;
        }

        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (isDied) return;
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPoint];

        float direction = Mathf.Sign(targetPoint.position.x - transform.position.x);

        Vector3 pos = transform.position;
        pos.x = Vector3.MoveTowards(pos, targetPoint.position, patrolSpeed * Time.deltaTime).x;
        transform.position = pos;

        transform.localScale = new Vector3(-direction, 1, 1);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        if (isDied) return;
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = new Vector3(-direction, 1, 1);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            jumpTargetPosition = player.position;
            hasAttacked = false;

            Invoke(nameof(JumpToPlayer), 0.3f);

            lastAttackTime = Time.time;
            return;
        }
        
        Vector3 pos = transform.position;
        pos.x = Vector3.MoveTowards(pos, player.position, chaseSpeed * Time.deltaTime).x;
        transform.position = pos;
    }

    void JumpToPlayer()
    {
        if (isDied) return;
        if (isJumpingToPlayer) return;

        isJumpingToPlayer = true;

        Vector2 direction = (jumpTargetPosition - (Vector2)transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * jumpSpeed, jumpForce);
    }

    void AttackPlayer()
    {
        if (isDied) return;
        FindFirstObjectByType<PlayerMovement>().TakeDamage(1);
    }

    public void Die()
    {
        if (isDied) return;

        animator.enabled = false;
        spriteRenderer.sprite = diedSprite;
        isDied = true;

        if (keyObject != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0f, 2f, 0f);
            Instantiate(keyObject, spawnPosition, Quaternion.identity);
        }
    }

    public void Respawn()
    {
        isDied = false;
        animator.enabled = true;
        transform.position = startPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDied) return;

        if (collision.collider.CompareTag("Ground"))
        {
            isJumpingToPlayer = false;
        }

        if (collision.collider.CompareTag("Player"))
        {
            if (!hasAttacked)
            {
                hasAttacked = true;
                AttackPlayer();
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
