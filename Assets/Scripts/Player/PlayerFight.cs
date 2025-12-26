using System.Collections;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 1;
    public float rotationDuration = 0.4f;
    public GameObject shieldVisual;
    public Transform swordTransform;

    private LineRenderer lineRenderer;
    private bool hasSword = false;
    private bool hasShield = false;
    private PlayerMovement playerMovement;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public void SwordAttack()
    {
        if (hasSword)
        {
            Attack();
            StartCoroutine(PlayAttackTopDown());
        }
    }

    public void HandleShield(bool isActive)
    {
        if (hasShield && isActive)
        {
            shieldVisual.SetActive(true);
            playerMovement.SetShieldActive(true);
        }
        else
        {
            shieldVisual.SetActive(false);
            playerMovement.SetShieldActive(false);
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemyCollider in hits)
        {
            IDamageable enemy = enemyCollider.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.Die();
                break;
            }
        }
    }

    IEnumerator PlayAttackTopDown()
    {
        swordTransform.gameObject.SetActive(true);
        swordTransform.localScale = new Vector3(0.5f, 0.5f, 1f);

        swordTransform.localEulerAngles = new Vector3(0f, 0f, 180f);

        float elapsed = 0f;
        float startY = 0.2f;
        float endY = -0.2f;
        float forwardOffset = 1f;

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            t = t * 2;

            float posY = Mathf.Lerp(startY, endY, t);

            swordTransform.localPosition = new Vector3(forwardOffset, posY, 0f);

            float angle = Mathf.Lerp(120f, -120f, t);

            swordTransform.localEulerAngles = new Vector3(0f, 0f, angle + 180f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        swordTransform.localPosition = Vector3.zero;
        swordTransform.localEulerAngles = Vector3.zero;
        swordTransform.localScale = Vector3.one;
        swordTransform.gameObject.SetActive(false);
    }

    public void SetHasSword(bool hasSword)
    {
        this.hasSword = hasSword;
    }

    public void SetHasShield(bool hasShield)
    {
        this.hasShield = hasShield;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
