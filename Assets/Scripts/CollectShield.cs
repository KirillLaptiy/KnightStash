using UnityEngine;

public class CollectShield : MonoBehaviour
{
    public void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            FindFirstObjectByType<PlayerFight>().SetHasShield(true);
            FindFirstObjectByType<ActivateShield>().EnableButton(true);
        }
    }
}
