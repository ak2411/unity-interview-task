using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    private float speed = 2.0f;
    private Camera m_mainCamRef;
    private Vector3 m_prevPos = Vector3.positiveInfinity;
    private void Awake()
    {
        m_mainCamRef = Camera.main;
    }

    // Simple controller, adapted from https://gist.github.com/minebloxians/3e20e97a7a64925e08cac8fd8f0f11b1 and https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += m_mainCamRef.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -m_mainCamRef.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += m_mainCamRef.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -m_mainCamRef.transform.forward * speed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += m_mainCamRef.transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += -m_mainCamRef.transform.up * speed * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            if (m_prevPos.x != Mathf.Infinity)
            {
                var delta = Input.mousePosition - m_prevPos;
                var deltaVector = new Vector3(-delta.y * 0.2f, delta.x * 0.2f, 0);
                var newPos = new Vector3(transform.eulerAngles.x + deltaVector.x, transform.eulerAngles.y + deltaVector.y, 0);
                transform.eulerAngles = newPos;
                m_prevPos = newPos;
            }
            m_prevPos = Input.mousePosition;
        }
    }
}
