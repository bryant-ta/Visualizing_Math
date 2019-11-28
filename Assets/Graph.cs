using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;
    public GraphFunctionName function;          // Use enum for dropdown list in Unity

    Transform[] points;
    static GraphFunction[] functions = { SineFunction, Sine2DFunction, MultiSine2DFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere, Torus }; // Relies on enum "GraphFunctionName" to match exactly

    const float pi = Mathf.PI;

    void Awake()
    {
        float step = 2f / resolution;           // Translate to graph width of 2
        Vector3 scale = Vector3.one * step;     // Divide size of cubes to fill graph width

        points = new Transform[resolution * resolution];

        // Initializing graph points - no longer set initital xz position bc xyz set in update
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }

        //Vector3 position;
        //position.y = 0f;
        //position.z = 0f;

        // Initializing and spacing graph points on x and z
        //for (int i = 0, z = 0; z < resolution; z++)
        //{
        //    position.z = (z + 0.5f) * step - 1f;
        //    for (int x = 0; x < resolution; x++, i++)
        //    {
        //        Transform point = Instantiate(pointPrefab, transform);
        //        position.x = (x + 0.5f) * step - 1f;            // Extra calculations account for cube width
        //        point.localPosition = position;
        //        point.localScale = scale;

        //        points[i] = point;
        //    }
        //}
    }

    private void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];

        // Animating Graph - 3d matrix output using variables uv
        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }

        // Animating Graph - XZ plane bound
        //for (int i = 0; i < points.Length; i++)
        //{
        //    Transform point = points[i];
        //    Vector3 position = point.localPosition;

        //    // Function
        //    //if (function == 0)        // If-else replaced by delegate "GraphFunction"
        //    //{
        //    //    position.y = SineFunction(position.x, t);
        //    //}
        //    //else
        //    //{
        //    //    position.y = MultiSineFunction(position.x, t);
        //    //}
        //    position.y = f(position.x, position.z, t);          // delegate use

        //    point.localPosition = position;
        //}
    }

    static Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= 1f + 10f * d;                // Need 1 to prevent divide by 0
        p.z = z;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (3f * u + 5f * v + t)) * 0.2f;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (7f * u + t)) * 0.1f;
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = 0.65f + Mathf.Sin(pi * (5f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(pi * (2f * v + t)) + Mathf.Cos(pi * v * t) * 0.05f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }
}