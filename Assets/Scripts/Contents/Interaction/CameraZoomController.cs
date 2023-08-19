using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace _Cinemachince
{
    public class CameraZoomController : MonoBehaviour
    {
        private CinemachineFreeLook _cinemachineFreeLook;

        [SerializeField]
        private float zoomMultiply = 20f;
        [SerializeField]
        private float zoomDamping = 20f;

        private float targetZoomFactor = 0f;

        [SerializeField]
        private float minZoom = 0f;
        [SerializeField]
        private float maxZoom = 1f;

        private void Awake()
        {
            // 변수 설정
            _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        }

        private void Update()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            targetZoomFactor = Mathf.Clamp(targetZoomFactor + -scrollInput * zoomMultiply, minZoom, maxZoom);
            _cinemachineFreeLook.m_YAxis.Value = Mathf.Lerp(_cinemachineFreeLook.m_YAxis.Value, targetZoomFactor, zoomDamping * Time.deltaTime);
        }
    }
}
