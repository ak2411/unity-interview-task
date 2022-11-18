using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBehavior : MonoBehaviour
{
   [SerializeField] private Material m_selectedMaterial;
   private Material m_unselectedMaterial;
   private ManipulationWidgetBehavior.ManipulationDirection m_type;
   private Camera m_mainCameraRef;
   private Transform m_parentRef;

   private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
   private float m_mouseZPos;
   private Vector3 m_originalPos;

   private void Awake()
   {
      if (!transform.parent.GetComponent<ManipulationWidgetBehavior>())
      {
         throw new Exception("Parent must have the ManipulationWidgetBehavior");
      }

      m_type = transform.parent.GetComponent<ManipulationWidgetBehavior>().m_type;
      m_unselectedMaterial = GetComponent<MeshRenderer>().material;
      m_parentRef = transform.parent.parent;
      m_mainCameraRef = Camera.main;
   }

   private void OnMouseDown()
   {
      m_originalPos = transform.position;
      m_mouseZPos = m_mainCameraRef.WorldToScreenPoint(transform.position).z;
      Debug.Log(m_mouseZPos);
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
      GetComponent<MeshRenderer>().material = m_unselectedMaterial;
   }

   private void Update()
   {
      if (m_mousePositions.Count < 1) return;
      var pos = m_mousePositions.Dequeue();
      var targetPos = transform.position;
      switch (m_type)
      {
         case ManipulationWidgetBehavior.ManipulationDirection.X:
            targetPos.x = pos.x;
            break;
         case ManipulationWidgetBehavior.ManipulationDirection.Y:
            targetPos.y = pos.y;
            break;
         case ManipulationWidgetBehavior.ManipulationDirection.Z:
            targetPos.z = pos.z;
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }

      m_parentRef.position += (targetPos-m_originalPos);
      m_originalPos = targetPos;
   }
}
