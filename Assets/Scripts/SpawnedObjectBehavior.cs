using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectBehavior : MonoBehaviour
{
    private bool m_selected;
    private const string SPAWNEDOBJNAME = "Cube";
    private void OnMouseDown()
    {
        SceneController.Instance.SelectObjectHandler(gameObject);
    }

    public void UpdateSelectStatus(bool newStatus)
    {
        foreach(Transform child in transform)
        {
            if (child.name == SPAWNEDOBJNAME) continue;
            child.gameObject.SetActive(newStatus);
            Debug.Log("hi");
        }
    }
}
