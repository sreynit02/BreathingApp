using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DebugControlle : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 10); // Extend ray 10 units forward
    }
}
