using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private  GameObject m_spawnObjectPrefab;
    private const float SPAWNDISTANCE = 5f;
    private Camera m_mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        m_mainCamera = Camera.main;
    }

    private void SpawnObject()
    {
        var cameraTransform = m_mainCamera.transform;
        var spawnPosition = cameraTransform.position + cameraTransform.forward * SPAWNDISTANCE;
        Instantiate(m_spawnObjectPrefab, spawnPosition, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        SpawnObject();
    }
}
