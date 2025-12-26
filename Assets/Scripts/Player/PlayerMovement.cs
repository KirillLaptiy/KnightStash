using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Animator animator;
    public LayerMask groundLayer;
    public LayerMask lavaLayer;
    public float acceleration = 10f;

    private Rigidbody2D rb;
    private Joystick joystick;

    private Vector2 startPosition;

    private float moveInput;
    private bool wantJump = false;
    private bool isOnGround;
    private bool isInLava;
    private int health = 3;

    private bool isShieldActive;

    private bool hasKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joystick = FindFirstObjectByType<Joystick>();

        startPosition = rb.position;
    }

    void Update()
    {
        isInLava = Physics2D.OverlapCircle(rb.position, 1f, lavaLayer);

        if (isInLava || health <= 0)
        {
            Die();
            return;
        }

        float target = joystick.Direction.x > 0 ? 1 : joystick.Direction.x < 0 ? -1 : 0;
        moveInput = Mathf.MoveTowards(moveInput, target, Time.deltaTime * acceleration);


        isOnGround = Physics2D.OverlapCircle(rb.position, 1f, groundLayer);

        if (joystick.Direction.y > 0.5 && isOnGround && !wantJump)
        {
            wantJump = true;
        }

        animator.SetBool("isJumping", !isOnGround);
        animator.SetFloat("Speed", moveInput * moveSpeed);
    }

    void FixedUpdate()
    {
        if (wantJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            wantJump = false;
        }

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed * (isShieldActive ? 0.2f : 1f), rb.linearVelocity.y);
    }

    void Die()
    {
        rb.position = startPosition;

        FindFirstObjectByType<PlayerHealthUI>().Heal(3);
        health = 3;

        SlimeEnemy[] slimes = FindObjectsByType<SlimeEnemy>(FindObjectsSortMode.None);

        foreach (SlimeEnemy slime in slimes)
        {
            slime.Respawn();
        }

        FrogEnemy[] frogs = FindObjectsByType<FrogEnemy>(FindObjectsSortMode.None);

        foreach (FrogEnemy frog in frogs)
        {
            frog.Respawn();
        }

        hasKey = false;

        FindFirstObjectByType<PlayerFight>().SetHasSword(false);
        FindFirstObjectByType<PlayerFight>().SetHasShield(false);

        FindFirstObjectByType<CollectSword>().Reset();
        FindFirstObjectByType<CollectShield>().Reset();

        FindFirstObjectByType<OpenDoor>().Reset();

        FindFirstObjectByType<ActivateShield>().EnableButton(false);
        FindFirstObjectByType<SwordAttack>().EnableButton(false);
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, 3);

        FindFirstObjectByType<PlayerHealthUI>().TakeDamage(damage);
    }

    public void SetShieldActive(bool active)
    {
        this.isShieldActive = active;
    }

    public void SetHasKey(bool value)
    {
        this.hasKey = value;
    }

    public bool HasKey
    {
        get {  return this.hasKey; }
    }
}
