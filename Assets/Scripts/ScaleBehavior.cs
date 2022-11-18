using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScaleBehavior : MonoBehaviour
{
   [SerializeField] private Material m_selectedMaterial;
   private Material m_unselectedMaterial;
   private ManipulationWidgetBehavior.ManipulationDirection m_type;
   private Camera m_mainCameraRef;
   private Transform m_spawnRef;
   
   private float m_mouseZPos;
   private Queue<Vector3> m_mousePositions = new Queue<Vector3>();
   private void Awake()
   {
      if (!transform.parent.GetComponent<ManipulationWidgetBehavior>())
      {
         throw new Exception("Parent must have the ManipulationWidgetBehavior");
      }

      m_type = transform.parent.GetComponent<ManipulationWidgetBehavior>().m_type;
      m_unselectedMaterial = GetComponent<MeshRenderer>().material;
      m_spawnRef = transform.parent.parent.GetChild(0);
      m_mainCameraRef = Camera.main;
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
      GetComponent<MeshRenderer>().material = m_unselectedMaterial;
   }

   private void Update()
   {
      if (m_mousePositions.Count <= 1) return;
      var endPos = m_mousePositions.Dequeue();
      var startPos = m_mousePositions.Peek();
      var diff = Vector3.zero;
      switch (m_type)
      {
         case ManipulationWidgetBehavior.ManipulationDirection.X:
            diff.x = (endPos - startPos).x;
            break;
         case ManipulationWidgetBehavior.ManipulationDirection.Y:
            diff.y = -(endPos - startPos).y;
            break;
         case ManipulationWidgetBehavior.ManipulationDirection.Z:
            diff.z = (endPos - startPos).z;
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
      m_spawnRef.localScale += diff;
   }
}