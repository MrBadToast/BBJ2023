using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace _Cinemachince
{
    public class CameraMoveController : MonoBehaviour
    {
        // cinemachine freelook 변수
        [SerializeField]
        private CinemachineFreeLook _cinemachineFreeLook;

        private void Awake()
        {
            // 변수 설정
            _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            
            // 마우스 입력에 반응하지 않도록 초기 설정
            _cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
            _cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        }

        private void Update()
        {
            // 입력이 들어올 경우, 마우스의 움직임에 따라 카메라를 움직이도록 만듦.
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _cinemachineFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
                _cinemachineFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
                _cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
            }
        }
    }
}
