using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class DrawCircle : MonoBehaviour
{
    public float m_radius;
    [SerializeField] private ManipulationWidgetBehavior.ManipulationDirection m_type;
    private const int SEGMENTS = 50;
    [SerializeField]private bool m_updateRadius = false;
    private LineRenderer m_lineRef;
    private MeshCollider m_colliderRef;
    private Transform m_cubeRef;
    private void Awake()
    {
        m_lineRef = GetComponent<LineRenderer>();
        m_lineRef.positionCount = SEGMENTS;
        m_lineRef.useWorldSpace = false;
        m_lineRef.startWidth = 0.1f;
        m_colliderRef = GetComponent<MeshCollider>();
        m_cubeRef = transform.parent.parent.GetChild(0);
        UpdateRadius();
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }

    public void UpdateRadius()
    {
        
        var rad = Vector3.Distance(m_cubeRef.GetComponent<BoxCollider>().bounds.max, m_cubeRef.position);
        m_radius = rad;
        m_updateRadius = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero,Vector3.up);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero,Vector3.right);
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(Vector3.zero,Vector3.forward);

    }

    private void Update()
    {
        if (!m_updateRadius) return;
        float x;
        float y;
        float angle = 10f;
        for (int i = 0; i < SEGMENTS; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * m_radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * m_radius;
            var pointPos = m_type switch
            {
                ManipulationWidgetBehavior.ManipulationDirection.X => new Vector3(0, x, y),
                ManipulationWidgetBehavior.ManipulationDirection.Y => new Vector3(x, 0, y),
                ManipulationWidgetBehavior.ManipulationDirection.Z => new Vector3(x, y, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            m_lineRef.SetPosition(i, pointPos);
            angle += (380f / SEGMENTS);
        }

        Mesh mesh = new Mesh();
        m_lineRef.BakeMesh(mesh);
        m_colliderRef.sharedMesh = mesh;
        m_updateRadius = false;
    }
}
