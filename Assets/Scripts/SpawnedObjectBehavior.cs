using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class SpawnedObjectBehavior : MonoBehaviour
{
    private bool m_selected;
    private void OnMouseDown()
    {
        SceneController.Instance.SelectObjectHandler(gameObject);
    }

    public void UpdateSelectStatus(bool newStatus)
    {
        foreach(Transform child in transform.parent)
        {
            if (child.name == name) continue;
            child.gameObject.SetActive(newStatus);
        }
    }
}
