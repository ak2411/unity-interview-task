using UnityEngine;

public class ManipulationWidgetBehavior : MonoBehaviour
{
      // private GameObject m_ManipulatedGameObject;
      public enum ManipulationType
      {
            ROTATE, MOVE, SCALE
      }
      void Awake()
      {
            // m_ManipulatedGameObject = this.transform..gameObject;
      }

      public void rotate(Vector3 start, Vector3 end)
      {
            
      }

      public void move(Vector3 start, Vector3 end)
      {
            Vector3 mouseChange = end - start;
            this.transform.position = start+end;
      }

      public void scale(Vector3 start, Vector3 end)
      {
            
      }
}