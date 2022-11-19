using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class DrawCircle : MonoBehaviour
{
    [SerializeField] private Material m_selectedMaterial;
    [SerializeField] private ManipulationWidgetBehavior.ManipulationDirection m_type;
    private const int SEGMENTS = 50;
    
    private LineRenderer m_lineRef;
    private MeshCollider m_colliderRef;
    private Transform m_cubeRef;
    private Transform m_rotateGizmoRef;
    private Camera m_mainCameraRef;
    private Material m_unselectedMaterialRef;

    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private float m_mouseZPos;
    private Vector3 m_rotationAxis;
    private Vector3 m_projectionAxis;

    private void Awake()
    {
        m_lineRef = GetComponent<LineRenderer>();
        m_lineRef.positionCount = SEGMENTS;
        m_lineRef.useWorldSpace = false;
        m_lineRef.startWidth = 0.1f;
        m_colliderRef = GetComponent<MeshCollider>();
        m_cubeRef = transform.parent.parent.GetChild(0);
        m_rotateGizmoRef = transform.parent;
        m_unselectedMaterialRef = GetComponent<LineRenderer>().material;
        m_mainCameraRef = Camera.main;
        switch (m_type)
        {
            case ManipulationWidgetBehavior.ManipulationDirection.X:
                m_rotationAxis = Vector3.right;
                m_projectionAxis = Vector3.up;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                m_rotationAxis =Vector3.up;
                m_projectionAxis = Vector3.right;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                m_rotationAxis = Vector3.forward;
                m_projectionAxis = Vector3.up;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        UpdateRadius();
    }

    public void UpdateRadius()
    {
        var rad = Vector3.Distance(m_cubeRef.GetComponent<BoxCollider>().bounds.max, m_cubeRef.position);
        float x;
        float y;
        float angle = 10f;
        for (int i = 0; i < SEGMENTS; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * rad;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * rad;
            var pointPos = m_type switch
            {
                ManipulationWidgetBehavior.ManipulationDirection.X => new Vector3(0, x, y) ,
                ManipulationWidgetBehavior.ManipulationDirection.Y => new Vector3(x, 0, y),
                ManipulationWidgetBehavior.ManipulationDirection.Z => new Vector3(x, y, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            m_lineRef.SetPosition(i, pointPos+ transform.InverseTransformPoint(m_cubeRef.position));
            angle += (380f / SEGMENTS);
        }

        Mesh mesh = new Mesh();
        m_lineRef.BakeMesh(mesh);
        m_colliderRef.sharedMesh = mesh;
    }
    
    private void OnMouseDown()
    {
        GetComponent<LineRenderer>().material = m_selectedMaterial;
        m_mouseZPos = m_mainCameraRef.WorldToScreenPoint(transform.position).z;
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
        GetComponent<LineRenderer>().material = m_unselectedMaterialRef;
        m_mousePositions.Clear();
        m_cubeRef.hasChanged = false;
        m_rotateGizmoRef.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (m_cubeRef.hasChanged) UpdateRadius();
        if (m_mousePositions.Count <= 1) return;
        var endPos= Vector3.ProjectOnPlane(m_mousePositions.Dequeue(), m_rotationAxis);
        var startPos = Vector3.ProjectOnPlane(m_mousePositions.Peek(), m_rotationAxis);
        var mag = Vector3.Distance(startPos, endPos);
        var dir = Vector3.Dot(endPos - startPos, m_projectionAxis) <0 ? -1:1;
        m_cubeRef.rotation *= Quaternion.AngleAxis(mag*dir*20f, m_rotationAxis);
        m_rotateGizmoRef.rotation *= Quaternion.AngleAxis(mag*dir*20f, m_rotationAxis);
    }
}
