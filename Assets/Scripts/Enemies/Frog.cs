using System.Collections;
using UnityEngine;

public class FrogEnemy : MonoBehaviour, IDamageable
{
    public Transform tongueOrigin;
    public GameObject tongueObject;
    public LineRenderer tongueRenderer;
    public float tongueLength = 20f;
    public float tongueDuration = 0.5f;
    public float tongueCooldown = 2f;
    public LayerMask playerLayer;
    public int damage = 1;
    public Sprite diedSprite;
    public Sprite attackSprite;
    public Sprite defaultSprite;

    private SpriteRenderer spriteRenderer;
    private float lastTongueTime;
    private Vector2 startPosition;

    private bool isDied;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tongueObject.SetActive(false);
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDied) return;

        if (Time.time - lastTongueTime >= tongueCooldown)
        {
            StartCoroutine(DoTongueAttack());
        }
    }

    IEnumerator DoTongueAttack()
    {
        lastTongueTime = Time.time;

        Vector2 dir = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
        Vector3 start = tongueOrigin.position;
        Vector3 end = start + (Vector3)(dir * tongueLength);

        tongueObject.SetActive(true);
        tongueRenderer.SetPosition(0, start);
        tongueRenderer.SetPosition(1, end);

        spriteRenderer.sprite = attackSprite;

        bool hasDamagedPlayer = false;
        float timer = 0f;

        while (timer < tongueDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(start, dir, tongueLength, playerLayer);
            if (hit.collider != null && !hasDamagedPlayer)
            {
                FindFirstObjectByType<PlayerMovement>()?.TakeDamage(damage);
                hasDamagedPlayer = true;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        tongueObject.SetActive(false);

        if (!isDied)
        spriteRenderer.sprite = defaultSprite;
    }

    public void Die()
    {
        isDied = true;
        tongueObject.SetActive(false);
        spriteRenderer.sprite = diedSprite;
    }

    public void Respawn()
    {
        isDied = false;
        transform.position = startPosition;
    }
}
