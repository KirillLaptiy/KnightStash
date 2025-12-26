using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivateShield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerFight playerFight;

    private bool isPressed = false;

    private void Start()
    {
        EnableButton(false);
        playerFight = FindFirstObjectByType<PlayerFight>();
    }

    void Update()
    {
        playerFight.HandleShield(isPressed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void EnableButton(bool isEnabled)
    {
        gameObject.GetComponent<Button>().GetComponent<Image>().enabled = isEnabled;
        gameObject.GetComponent<Button>().interactable = isEnabled;
        gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().enabled = isEnabled;
    }
}
