using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    /// <summary>
    /// Seconds until destroyed.
    /// </summary>
    [SerializeField, Range(0.01f, 300f)] public float CountDown = 5;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, CountDown);
    }

}
