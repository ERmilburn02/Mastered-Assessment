using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotater : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = Vector3.up * m_Speed * Time.deltaTime;
        transform.Rotate(rot);
    }
}
