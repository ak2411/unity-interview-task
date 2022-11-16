using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private  GameObject m_spawnObjectPrefab;
    private const float SPAWNDISTANCE = 5f;
    private Camera m_mainCamera;

    private GameObject m_currSpawnedObject;
    // Start is called before the first frame update
    void Awake()
    {
        m_mainCamera = Camera.main;
    }

    private void SpawnObject()
    {
        var cameraTransform = m_mainCamera.transform;
        var spawnPosition = cameraTransform.position + cameraTransform.forward * SPAWNDISTANCE;
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
}
