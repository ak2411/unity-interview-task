using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class TranslateBehavior : MonoBehaviour
{
    [SerializeField] private ManipulationHelpers.ManipulationType m_type;
    [SerializeField] private Material m_selectedMaterial;

    private float m_mouseZPos;
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private Camera m_mainCameraRef;
    
    private Transform m_cubeRef;
    private Material m_unselectedMaterialRef;
    private Vector3 m_normal;
    
    private const float OFFSET = 0.5f;
    
    private void Awake()
    {
        m_cubeRef = transform.parent.parent.GetChild(0);
        m_mainCameraRef = Camera.main;
        m_unselectedMaterialRef = GetComponent<MeshRenderer>().material;

        m_normal = m_type switch
        {
            ManipulationHelpers.ManipulationType.X => Vector3.right,
            ManipulationHelpers.ManipulationType.Y => Vector3.up,
            ManipulationHelpers.ManipulationType.Z => Vector3.forward,
            _ => throw new ArgumentOutOfRangeException()
        };
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var distance = Vector3.Distance(m_cubeRef.GetComponent<BoxCollider>().bounds.max, m_cubeRef.position);
        transform.position = m_cubeRef.position+ (distance + OFFSET) * m_normal;
        transform.LookAt(2*transform.position - m_cubeRef.position);
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
        m_mousePositions = new Queue<Vector3>();
        m_cubeRef.hasChanged = false;
    }

    private void Update()
    {
        if(m_cubeRef.hasChanged) UpdatePosition();
        if (m_mousePositions.Count <= 1) return;
        var endPos = m_mousePositions.Dequeue();
        var startPos = m_mousePositions.Peek();
        m_cubeRef.position += Vector3.Project(startPos - endPos, m_normal);
    }
}
