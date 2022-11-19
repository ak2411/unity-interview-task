using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newScaleBehavior : MonoBehaviour
{
    [SerializeField] private ManipulationWidgetBehavior.ManipulationDirection m_type;
    [SerializeField] private Material m_selectedMaterial;
    
    private float m_mouseZPos;
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private Camera m_mainCameraRef;

    private Transform m_cubeRef;
    private Transform m_scaleRef;
    private Material m_unselectedMaterialRef;
    private Vector3 m_normal;

    private const float OFFSET = 0.2f;
    
    private void Awake()
    {
        m_cubeRef = transform.parent.parent.GetChild(0);
        m_mainCameraRef = Camera.main;
        m_unselectedMaterialRef = GetComponent<MeshRenderer>().material;

        m_normal = m_type switch
        {
            ManipulationWidgetBehavior.ManipulationDirection.X => m_cubeRef.right,
            ManipulationWidgetBehavior.ManipulationDirection.Y => m_cubeRef.up,
            ManipulationWidgetBehavior.ManipulationDirection.Z => m_cubeRef.forward,
            _ => throw new ArgumentOutOfRangeException()
        };
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        var distance = Vector3.Distance(m_cubeRef.GetComponent<BoxCollider>().bounds.max, m_cubeRef.position);
        transform.position = (distance + OFFSET) * m_normal;
        transform.LookAt(-m_cubeRef.position);
    }

    private void OnMouseDown()
    {
        m_mouseZPos = m_mainCameraRef.WorldToScreenPoint(transform.position).z;
        GetComponent<MeshRenderer>().material = m_selectedMaterial;
        m_mousePositions.Enqueue(_getWorldMousePos());
    }
    private Vector3 _getWorldMousePos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = m_mouseZPos;
        return m_mainCameraRef.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDrag()
    {
        m_mousePositions.Enqueue(_getWorldMousePos());
    }
    private void OnMouseUp()
    {
        GetComponent<MeshRenderer>().material = m_unselectedMaterialRef;
        m_mousePositions.Clear();
    }
    private void Update()
    {
        if (m_mousePositions.Count <= 1) return;
        var endPos = m_mousePositions.Dequeue();
        var startPos = m_mousePositions.Peek();
        var diff = Vector3.zero;
        switch (m_type)
        {
            case ManipulationWidgetBehavior.ManipulationDirection.X:
                diff.x = (endPos - startPos).x;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                diff.y = -(endPos - startPos).y;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                diff.z = (endPos - startPos).z;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_cubeRef.localScale += diff;
        UpdatePosition();
    }
}
