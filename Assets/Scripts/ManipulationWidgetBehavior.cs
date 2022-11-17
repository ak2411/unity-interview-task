using System;
using UnityEngine;

public class ManipulationWidgetBehavior : MonoBehaviour
{
      [SerializeField] public ManipulationDirection m_type;
      private GameObject m_cone;
      private GameObject m_sphere;
      private Vector3 m_RotateAngle;
      public enum ManipulationDirection
      {
            X, Y, Z
      }
      void Awake()
      {
            switch(m_type)
            {
                  case ManipulationDirection.X:
                        m_RotateAngle = new Vector3(0f, 0f, 0f);
                        break;
                  case ManipulationDirection.Y:
                        m_RotateAngle = new Vector3(0f, -90f, -90f);

                        break;
                  case ManipulationDirection.Z:
                        m_RotateAngle = new Vector3(-90f, 0f, -90f);
                        break;
                  default:
                        throw new ArgumentOutOfRangeException();
            }

            transform.Rotate(m_RotateAngle);
      }
}