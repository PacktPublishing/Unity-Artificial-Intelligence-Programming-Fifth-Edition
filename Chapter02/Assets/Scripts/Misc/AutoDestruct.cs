using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField]
    public float destructTime = 2.0f;

    void Start()
    {
        Destroy(gameObject, destructTime);
    }
}
