using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public Camera mainCamera;
    public float padding = 2f;

    private GameObject[] visibleObjects;

    void Start()
    {
        // Предполагается, что объекты для контроля помечены тегом "VisibleObject"
        visibleObjects = GameObject.FindGameObjectsWithTag("VisibleObject");
    }

    void Update()
    {
        Vector3 camPos = mainCamera.transform.position;
        Rect visibleArea = new Rect(
            camPos.x - mainCamera.orthographicSize * mainCamera.aspect - padding,
            camPos.y - mainCamera.orthographicSize - padding,
            (mainCamera.orthographicSize * 2) * mainCamera.aspect + padding * 2,
            mainCamera.orthographicSize * 2 + padding * 2);

        foreach (var obj in visibleObjects)
        {
            if (obj == null) continue;

            Vector2 objPos = obj.transform.position;
            bool isVisible = visibleArea.Contains(objPos);

            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                // Если объекта нет SpriteRenderer — пропускаем
                continue;
            }

            if (sr.enabled != isVisible)
            {
                sr.enabled = isVisible;
            }
        }

    }
}
