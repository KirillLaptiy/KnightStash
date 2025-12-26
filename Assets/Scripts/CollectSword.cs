using UnityEngine;
using UnityEngine.UI;

public class CollectSword : MonoBehaviour {

    public void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            FindFirstObjectByType<PlayerFight>().SetHasSword(true);
            FindFirstObjectByType<SwordAttack>().EnableButton(true);
        }
    }
}
