using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Sprite openedDoor;

    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private Sprite closedDoor;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        closedDoor = spriteRenderer.sprite;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Reset()
    {
        boxCollider2D.enabled = true;
        spriteRenderer.sprite = closedDoor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player") && playerMovement.HasKey && spriteRenderer.sprite != openedDoor)
        {
            spriteRenderer.sprite = openedDoor;
            playerMovement.SetHasKey(false);
            boxCollider2D.enabled = false;
        }
    }
}
