using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CarCustomiser : MonoBehaviour
{
    // Singleton
    private static CarCustomiser m_Instance;
    public static CarCustomiser Instance
    {
        get => m_Instance;
        private set => m_Instance = value;
    }

    [SerializeField] private int m_BaseColorIndex = 0;

    private MeshRenderer m_Renderer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    public void SetCarColor(Vector3 newColor)
    {
        m_Renderer.materials[m_BaseColorIndex].color = new Color(newColor.x, newColor.y, newColor.z);
    }
}
