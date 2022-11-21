using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private static SceneController m_Instance;
    
    public static SceneController Instance
    {
        get { return m_Instance; }
    }

    private GameObject m_selectedObject;
    private bool m_selectedHasChanged = false;
    
    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public void SelectObjectHandler(GameObject newObject)
    {
        if (m_selectedObject)
        {
            m_selectedObject.GetComponent<SpawnedObjectBehavior>().UpdateSelectStatus(false);
        }

        if (newObject == m_selectedObject)
        {
            m_selectedObject = null;
            return;
        }
        m_selectedObject = newObject;
        m_selectedObject.GetComponent<SpawnedObjectBehavior>().UpdateSelectStatus(true);
    }

    public void TurnOffHasChanged()
    {
        m_selectedHasChanged = true;
    }
    
    private void LateUpdate()
    {
        if (m_selectedHasChanged)
        {
            m_selectedHasChanged = m_selectedObject.transform.hasChanged = false;
        }
    }
}
