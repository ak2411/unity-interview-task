using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehavior : MonoBehaviour
{
    [SerializeField] private ManipulationHelpers.ManipulationType m_type;
    [SerializeField] private Material m_selectedMaterial;
    
    private float m_mouseZPos;
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private Camera m_mainCameraRef;

    private Transform m_cubeRef;
    private Material m_unselectedMaterialRef;
    private Vector3 m_normal;

    private const float OFFSET = 0.2f;
    
    private void Awake()
    {
        m_cubeRef = transform.parent.parent.GetChild(0);
        m_mainCameraRef = Camera.main;
        m_unselectedMaterialRef = GetComponent<MeshRenderer>().material;
        _updatePosition();
    }

    private void _updatePosition()
    {
        m_normal = m_type switch
        {
            ManipulationHelpers.ManipulationType.X => m_cubeRef.right,
            ManipulationHelpers.ManipulationType.Y => m_cubeRef.up,
            ManipulationHelpers.ManipulationType.Z => m_cubeRef.forward,
            _ => throw new ArgumentOutOfRangeException()
        };
        var distance = Vector3.Distance(m_cubeRef.GetComponent<BoxCollider>().bounds.max, m_cubeRef.position);
        transform.position = m_cubeRef.position+((distance + OFFSET) * m_normal);
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
        m_cubeRef.hasChanged = false;
    }
    private void Update()
    {
        if(m_cubeRef.hasChanged) _updatePosition();
        if (m_mousePositions.Count <= 1) return;
        m_normal = m_type switch
        {
            ManipulationHelpers.ManipulationType.X => m_cubeRef.right,
            ManipulationHelpers.ManipulationType.Y => m_cubeRef.up,
            ManipulationHelpers.ManipulationType.Z => m_cubeRef.forward,
            _ => throw new ArgumentOutOfRangeException()
        };
        var endPos =  Vector3.Project(m_mousePositions.Dequeue(), m_normal);
        var startPos = Vector3.Project(m_mousePositions.Peek(), m_normal);
        var dist = Vector3.Distance(startPos, endPos);
        var dir = Vector3.Dot(startPos-endPos, m_normal) <0 ? -1:1;
        var diff = Vector3.zero;
        switch (m_type)
        {
            case ManipulationHelpers.ManipulationType.X:
                diff.x = dir * dist;
                break;
            case ManipulationHelpers.ManipulationType.Y:
                diff.y = dir * dist;
                break;
            case ManipulationHelpers.ManipulationType.Z:
                diff.z = dir * dist;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_cubeRef.localScale += diff;
    }
}
