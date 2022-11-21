using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private  GameObject m_spawnObjectPrefab;
    private Camera m_mainCamera;

    private GameObject m_currSpawnedObject;
    void Awake()
    {
        m_mainCamera = Camera.main;
    }

    private void SpawnObject()
    {
        m_currSpawnedObject = Instantiate(m_spawnObjectPrefab, GetWorldMousePos(), Quaternion.identity);
    }

    private Vector3 GetWorldMousePos()
    {
        var mousePos =Input.mousePosition;
        mousePos.z = m_mainCamera.WorldToScreenPoint(this.transform.position).z;
        return  m_mainCamera.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDown()
    {
        SpawnObject();
    }

    private void OnMouseDrag()
    {
        if (!m_currSpawnedObject) return;
        m_currSpawnedObject.transform.position = GetWorldMousePos();
    }

    private void OnMouseUp()
    {
        m_currSpawnedObject = null;
    }

    private void Update()
    {
        transform.LookAt(2*transform.position - m_mainCamera.transform.position);
    }
}
