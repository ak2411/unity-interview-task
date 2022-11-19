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
    private Vector3 m_rotationAxis;
    private Vector3 m_projectionAxis;
    private float m_mouseZPos;
    private Vector3 testpos = Vector3.zero;
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
                m_rotationAxis =m_parentRef.up;
                m_projectionAxis = m_parentRef.right;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                // m_refVec = m_parentRef.right;
                // m_axis = m_parentRef.forward;
                m_rotationAxis = m_parentRef.forward;
                m_projectionAxis = -m_parentRef.up;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                // m_refVec = m_parentRef.forward;
                // m_axis = m_parentRef.right;
                m_rotationAxis = m_parentRef.right;
                m_projectionAxis = -m_parentRef.forward;
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
        m_mousePositions = new Queue<Vector3>();
    }

    private Vector3 _getMouseDirection()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = m_mouseZPos;
        return m_mainCameraRef.ScreenToWorldPoint(mousePos);
        // return Input.mousePosition - m_mainCameraRef.WorldToScreenPoint(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_parentRef.position, testpos);
    }

    private void Update()
    {
        if (m_mousePositions.Count <= 1) return;
        var endPos= Vector3.Project(m_mousePositions.Dequeue(), m_projectionAxis);
        testpos = endPos;
        var startPos = Vector3.Project(m_mousePositions.Peek(), m_projectionAxis);
        // var endPos= Vector3.ProjectOnPlane(m_mousePositions.Dequeue() - m_parentRef.position, m_axis);
        // var startPos = Vector3.ProjectOnPlane(m_mousePositions.Peek() - m_parentRef.position, m_axis);
        var mag = Vector3.Distance(startPos, endPos);
        var dir = Vector3.Dot(endPos - startPos, m_projectionAxis) <0 ? -1:1;
        Debug.Log(dir);
        float angle = 0f;
        // var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - m_startingAngle;
         // var diff = 0f;
        switch (m_type)
        {
            case ManipulationWidgetBehavior.ManipulationDirection.X:
                // angle = Vector3.Project(m_parentRef.position - endPos, m_parentRef.right);
                angle = (endPos-startPos).x;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Y:
                angle = (endPos-startPos).y;
                break;
            case ManipulationWidgetBehavior.ManipulationDirection.Z:
                angle = (endPos-startPos).z;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // m_parentRef.rotation *= Quaternion.AngleAxis(angle, m_axis);
        // m_parentRef.RotateAround(m_parentRef.position,m_axis, 20f* diff);
        //  endVec = Vector3.ProjectOnPlane(dir- m_parentRef.position, m_parentRef.right);
        // var startVec = Vector3.ProjectOnPlane(start -m_parentRef.position, m_parentRef.right);
        // Debug.Log(startVec-endVec);
        // angle = (startVec-endVec).y;
        // m_parentRef.Rotate(20f*(endPos-startPos));
        // m_parentRef.RotateAround(m_parentRef.position,m_rotationAxis, 20f* mag*dir);
        m_parentRef.rotation *= Quaternion.AngleAxis(mag*dir*20f, m_rotationAxis);
    }
}
