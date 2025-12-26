using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

[RequireComponent(typeof(Button))]
public class UIButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip clickSound;
    public Color hoverColor = Color.gray;
    private AudioSource audioSource;
    private Button button;
    private Color defaultColor;

    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = FindFirstObjectByType<AudioSource>();
        defaultColor = button.targetGraphic.color;

        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.targetGraphic.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.targetGraphic.color = defaultColor;
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
