using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehavior : MonoBehaviour
{
    [SerializeField] private Material m_selectedMaterial;
    private Material m_unselectedMaterial;
    private ManipulationWidgetBehavior.ManipulationDirection m_type;
    private Camera m_mainCameraRef;
    private Transform m_parentRef;
    
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private float m_startingAngle;
    private Vector3 m_refVec;
    private Vector3 m_axis;

    void Awake()
    {
        if (!transform.parent.GetComponent<ManipulationWidgetBehavior>())
        {
            throw new Exception("Parent must have the ManipulationWidgetBehavior");
        }

        m_type = transform.parent.GetComponent<ManipulationWidgetBehavior>().m_type;
        m_unselectedMaterial = GetComponent<MeshRenderer>().material;
        m_mainCameraRef = Camera.main;
        m_parentRef = transform.parent.parent;
        switch (m_type)
        {
            case ManipulationWidgetBehavior.ManipulationDirection.X:
                m_refVec = m_parentRef.right;
                m_axis = Vector3.up;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                m_refVec = m_parentRef.right;
                m_axis = Vector3.forward;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                m_refVec = m_parentRef.forward;
                m_axis = Vector3.right;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseDown()
    {
        GetComponent<MeshRenderer>().material = m_selectedMaterial;
        var dir = _getMouseDirection();
        m_startingAngle = (Mathf.Atan2(dir.y, dir.x)-Mathf.Atan2(m_refVec.y, m_refVec.x)) * Mathf.Rad2Deg;
        m_mousePositions.Enqueue(dir);
    }

    private void OnMouseDrag()
    {
        m_mousePositions.Enqueue(_getMouseDirection());
    }

    private void OnMouseUp()
    {
        GetComponent<MeshRenderer>().material = m_unselectedMaterial;
    }

    private Vector3 _getMouseDirection()
    {
        return Input.mousePosition - m_mainCameraRef.WorldToScreenPoint(transform.position);
    }

    private void Update()
    {
        if (m_mousePositions.Count < 1) return;
        var dir = m_mousePositions.Dequeue();
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - m_startingAngle;
        m_parentRef.rotation = Quaternion.AngleAxis(angle, m_axis);
    }
}
