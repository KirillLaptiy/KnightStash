using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour, IPointerDownHandler
{
    private PlayerFight playerFight;

    private void Start()
    {
        EnableButton(false);
        playerFight = FindFirstObjectByType<PlayerFight>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        playerFight.SwordAttack();
    }

    public void EnableButton(bool isEnabled)
    {
        gameObject.GetComponent<Button>().GetComponent<Image>().enabled = isEnabled;
        gameObject.GetComponent<Button>().interactable = isEnabled;
        gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().enabled = isEnabled;
    }
}
