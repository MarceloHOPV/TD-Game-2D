using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class Circulo : MonoBehaviour
{
    [FormerlySerializedAs("raio")]
    [SerializeField] private float radius = 1f;
    [FormerlySerializedAs("segmentos")]
    [SerializeField] private int segments = 60;

    void Awake()
    {
        Draw();
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        Draw();
    }

    private void Draw()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.loop = true;
        line.useWorldSpace = false;
        line.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = 2f * Mathf.PI * i / segments;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            line.SetPosition(i, point);
        }
    }
}
