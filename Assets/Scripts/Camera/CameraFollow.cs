using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private InputController _inputController = default;
    
    public CinemachineFreeLook playerCamera;

    private void Start()
    {

    }

    private void OnEnable()
    {
        _inputController.selectSpellInputEvent += OnSelectInput;
    }

    private void OnDisable()
    {
        _inputController.selectSpellInputEvent -= OnSelectInput;
    }

    public void OnSelectInput(float select)
    {
        if (select == 1)
        {
            playerCamera.m_XAxis.m_InputAxisName = "";
            playerCamera.m_XAxis.m_InputAxisValue = 0;
            playerCamera.m_YAxis.m_InputAxisName = "";
            playerCamera.m_YAxis.m_InputAxisValue = 0;
        }
        else
        {
            playerCamera.m_XAxis.m_InputAxisName = "Mouse X";
            playerCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}
