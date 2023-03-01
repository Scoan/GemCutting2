using System;
using UnityEngine;

namespace MyCamControls
{
    public class CatalogCameraRotator : MonoBehaviour
    {
        public float m_rotateRate = 1.0f;
        
        public void Update()
        {
            transform.Rotate(new Vector3(00f, m_rotateRate * Time.deltaTime, 0.0f));
        }
    }
}