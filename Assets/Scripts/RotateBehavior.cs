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
    private float m_mouseZPos;

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
                // m_refVec = m_parentRef.right;
                // m_axis = m_parentRef.up;
                m_axis =Vector3.up;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                // m_refVec = m_parentRef.right;
                // m_axis = m_parentRef.forward;
                m_axis = Vector3.right;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                // m_refVec = m_parentRef.forward;
                // m_axis = m_parentRef.right;
                m_axis = Vector3.forward;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseDown()
    {
        GetComponent<MeshRenderer>().material = m_selectedMaterial;
        var dir = _getMouseDirection();
        m_mouseZPos = m_mainCameraRef.WorldToScreenPoint(transform.position).z;

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
        m_mousePositions.Clear();
    }

    private Vector3 _getMouseDirection()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = m_mouseZPos;
        return m_mainCameraRef.ScreenToWorldPoint(mousePos);
        // return Input.mousePosition - m_mainCameraRef.WorldToScreenPoint(transform.position);
    }

    private void Update()
    {
        if (m_mousePositions.Count <= 1) return;
        var dir = m_mousePositions.Dequeue();
        var start = m_mousePositions.Peek();
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - m_startingAngle;
        var diff = 0f;
        switch (m_type)
        {
            case ManipulationWidgetBehavior.ManipulationDirection.X:
                diff = (dir-start).x;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                diff = -(dir-start).y;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                diff = (dir-start).z;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // m_parentRef.rotation *= Quaternion.AngleAxis(angle, m_axis);
        m_parentRef.RotateAround(m_parentRef.position,m_axis, 20f* diff);
    }
}
