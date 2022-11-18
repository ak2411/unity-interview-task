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

    private GameObject m_SelectedObject;
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
        if (m_SelectedObject)
        {
            m_SelectedObject.GetComponent<SpawnedObjectBehavior>().UpdateSelectStatus(false);
        }

        if (newObject == m_SelectedObject)
        {
            m_SelectedObject = null;
            return;
        }
        m_SelectedObject = newObject;
        m_SelectedObject.GetComponent<SpawnedObjectBehavior>().UpdateSelectStatus(true);
    }
}
