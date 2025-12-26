using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectKey : MonoBehaviour{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.SetHasKey(true);
            Destroy(gameObject);
        }
    }
}
