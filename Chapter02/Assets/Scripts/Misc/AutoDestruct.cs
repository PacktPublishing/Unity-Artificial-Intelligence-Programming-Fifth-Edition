using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField]
    public float DestructTime = 2.0f;

    void Start()
    {
        Destroy(gameObject, DestructTime);
    }
}
