using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;
    public GraphFunctionName function;          // Use enum for dropdown list in Unity

    Transform[] points;
    static GraphFunction[] functions = { SineFunction, MultiSineFunction }; // Relies on enum "GraphFunctionName" to match exactly

    void Awake()
    {
        float step = 2f / resolution;           // Translate to graph width of 2
        Vector3 scale = Vector3.one * step;     // Divide size of cubes to fill graph width
        Vector3 position;
        position.y = 0f;
        position.z = 0f;

        points = new Transform[resolution * resolution];

        // Initializing and spacing graph points on x and z
        for (int i = 0, z = 0; z < resolution; z++)
        {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                Transform point = Instantiate(pointPrefab, transform);
                position.x = (x + 0.5f) * step - 1f;            // Extra calculations account for cube width
                point.localPosition = position;
                point.localScale = scale;

                points[i] = point;
            }
        }
    }

    private void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];

        // Animating Graph
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;

            // Function
            //if (function == 0)        // If-else replaced by delegate "GraphFunction"
            //{
            //    position.y = SineFunction(position.x, t);
            //}
            //else
            //{
            //    position.y = MultiSineFunction(position.x, t);
            //}
            position.y = f(position.x, position.z, t);          // delegate use

            point.localPosition = position;
        }
    }

    static float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(Mathf.PI * (x + t));
    }

    static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y;
    }
}