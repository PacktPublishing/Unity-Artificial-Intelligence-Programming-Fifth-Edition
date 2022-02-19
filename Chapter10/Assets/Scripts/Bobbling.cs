using UnityEngine;

public class Bobbling : MonoBehaviour {
    // Range over which height varies.
    public float heightScale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    public float xScale = 1.0f;

    void Update() {
        float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
        Vector3 pos = transform.position;
        pos.y = height;
        transform.position = pos;
    }
}
