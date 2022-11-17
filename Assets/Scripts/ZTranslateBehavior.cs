using UnityEngine;
using System.Collections.Generic;


public class ZTranslateBehavior: MonoBehaviour
{
    // [SerializeField] private ManipulationWidgetBehavior.ManipulationType m_manipulationType;
    private Vector3 m_startMousePos;
    private Vector3 m_currentMousePos;
    private Camera m_mainCamera;
    private float m_mouseZPos;
    private Vector3 m_offset;
    private Queue<Vector3> m_mousePositions = new Queue<Vector3>();

    private void Awake()
    {
        m_mainCamera = Camera.main;
    }
    private void OnMouseDown()
    {
        m_mouseZPos = m_mainCamera.WorldToScreenPoint(this.transform.position).z;
        var mousePos = Input.mousePosition;
        mousePos.z = m_mouseZPos;
        m_mousePositions.Enqueue(m_mainCamera.ScreenToWorldPoint(mousePos));
    }

    private void OnMouseDrag()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = m_mouseZPos;
        m_mousePositions.Enqueue(m_mainCamera.ScreenToWorldPoint(mousePos));
    }

    private void Update()
    {
        if (m_mousePositions.Count <= 1) return;
        var start = m_mousePositions.Dequeue();
        var end = m_mousePositions.Dequeue();
        var diff = end - start;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, start.y);
    }
}