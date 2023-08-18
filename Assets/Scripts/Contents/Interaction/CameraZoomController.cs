using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace _Cinemachince
{
    public class CameraZoomController : MonoBehaviour
    {
        public CinemachineFreeLook _cinemachineFreeLook;
        public float zoomSpeed;

        private void Awake()
        {
            // 변수 설정
            _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            zoomSpeed = 100f;
        }

        private void Update()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // 마우스 스크롤을 통해 줌 인 및 줌 아웃
            float current = _cinemachineFreeLook.m_YAxis.Value;
            _cinemachineFreeLook.m_YAxis.Value += Mathf.Lerp(0, zoomSpeed * scrollInput, 0.5f) * Time.deltaTime;
        }
    }
}
