using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchRotation : MonoBehaviour
{
    private Light2D light2D;
    public float speed = 0.000001f;
    public float minRadius = 5.2f;
    public float maxRadius = 6f;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }


    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time * 3f) * 2f;
        light2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, (Mathf.Sin(Time.time / 2f)));
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

}
