using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XRotateBehavior : MonoBehaviour
{
    // [SerializeField] private ManipulationWidgetBehavior.ManipulationType m_manipulationType;
    private Vector3 m_startMousePos;
    private Vector3 m_currentMousePos;
    private Camera m_mainCamera;
    private float m_mouseZPos;
    private Vector3 m_offset;
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
    private const float ROTATIONSENSITIVITY = 20f;
    private float baseAngle = 0.0f;

    private void Awake()
    {
        m_mainCamera = Camera.main;
    }
    
 
    // function OnMouseDown() {
    //     var dir = Camera.main.WorldToScreenPoint(transform.position);
    //     dir = Input.mousePosition - dir;
    //     baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //     baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    // }
    //
    // function OnMouseDrag() {
    //     var dir = Camera.main.WorldToScreenPoint(transform.position);
    //     dir = Input.mousePosition - dir;
    //     var angle =  Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - baseAngle;
    //     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    // }
    private void OnMouseDown()
    {
        // m_mouseZPos = m_mainCamera.WorldToScreenPoint(this.transform.position).z;
        // var mousePos = Input.mousePosition;
        // mousePos.z = m_mouseZPos;
        // m_mousePositions.Enqueue(m_mainCamera.ScreenToWorldPoint(mousePos));
        var dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        baseAngle -= Mathf.Atan2(transform.parent.parent.right.y, transform.parent.parent.right.x) * Mathf.Rad2Deg;
        m_mousePositions.Enqueue(dir);
    }

    private void OnMouseDrag()
    {
        // var mousePos = Input.mousePosition;
        //         mousePos.z = m_mouseZPos;
        //         m_mousePositions.Enqueue(m_mainCamera.ScreenToWorldPoint(mousePos));
        var dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        m_mousePositions.Enqueue(dir);

    }

    private void Update()
    {
        if (m_mousePositions.Count <= 1) return;
        var dir = m_mousePositions.Dequeue();
        // var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        // angle =    (angle > 0) ? angle : angle + 360;
        // transform.parent.parent.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
        var angle =  Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - baseAngle;
        Debug.Log(angle);
        transform.parent.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // var start = m_mousePositions.Dequeue();
        // var end = m_mousePositions.Dequeue();
        // var angle = Vector3.SignedAngle(start-end, transform.parent.parent.right,Vector3.up);
        // Debug.Log(angle);
        // transform.parent.parent.RotateAround(transform.parent.parent.position, Vector3.up, angle);
        // transform.parent.parent.rotation *= Quaternion.AngleAxis(angle*20f, Vector3.up);

        // var angle = Mathf.Atan2(end.y-start.y, end.x-start.x) * Mathf.Rad2Deg;
        // angle =    (angle > 0) ? angle : angle + 360;
        // transform.parent.parent.rotation = Quaternion.Euler(new Vector3(0,angle, 0)); 
    }
}