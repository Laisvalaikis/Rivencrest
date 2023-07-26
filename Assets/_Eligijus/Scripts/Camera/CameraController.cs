using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Camera mainCamera;
    public bool CameraDragging = true;
    public float panSpeed = 20f;
    public Vector2 panLimitX;
    public Vector2 panLimitY;
    public Vector3 touchStart;
    public Vector3 touchStartScreen;
    private bool cameraWasMoving = false;
    private bool panning = false;
    private GameManager _gameManager;
    private CinemachineFramingTransposer _cinemachineFramingTransposer;
    [HideInInspector] public bool hasDraggingStarted;
    [HideInInspector] public float time = 1f;
    private Vector3 _movingDirection;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _cinemachineFramingTransposer =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (time > 0)
        {
            time -= Time.deltaTime;
        }

        if (!_gameManager.isDragAvailable && !panning)
        {
            Move(_movingDirection);
        }
        else
        {
            time = 1f;
        }
        
        if (CameraDragging && !_gameManager.isBoardDisabled)
        {
            MouseDrag(pos);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    { 
        _movingDirection = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        Vector3 pos = transform.position;
        if (Mathf.Abs(direction.x) > 0)
        {
            pos.x = Horizontal(Mathf.Sign(direction.x), pos);
        }
        if (Mathf.Abs(direction.y) > 0)
        {
            pos.y = Vertical(Mathf.Sign(direction.y), pos);
        }
        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.y = Mathf.Clamp(pos.y, panLimitY.x, panLimitY.y);
        transform.position = pos;
    }

    private float Vertical(float direction, Vector3 pos)
    {
        float y = pos.y;
            cinemachineVirtualCamera.Follow = null;
            y += panSpeed * Time.deltaTime * direction;
            cameraWasMoving = true;
            return y;

    }

    private float Horizontal(float direction, Vector3 pos)
    {
        float x = pos.x;
        cinemachineVirtualCamera.Follow = null;
        x += panSpeed * Time.deltaTime * direction;
        cameraWasMoving = true;
        return x;
    }

    public void MouseDrag(Vector3 pos)
    {
        Vector3 tempPos = pos;

        if (!_gameManager.isBoardDisabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                touchStartScreen = Input.mousePosition;
                hasDraggingStarted = true;
            }
            
            if (Input.GetMouseButton(0) && _gameManager.isDragAvailable && hasDraggingStarted)
            {
                Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                float directionMagnitude = direction.magnitude;
                Vector3 directionScreen = touchStartScreen - Input.mousePosition;
                float directionScreenMagnitude = directionScreen.magnitude;
                Vector3 minimalOffset = new Vector3(0.01f, 0.01f, 0f);
                float minimalOffsetMagnitude = minimalOffset.magnitude;
            
                if (directionMagnitude > minimalOffsetMagnitude && directionScreenMagnitude > minimalOffsetMagnitude)
                {
                    panning = true;
                    cinemachineVirtualCamera.Follow = null;
                    tempPos += direction;
                }
                _gameManager.canButtonsBeClicked = false;
            }
            else
            {
                hasDraggingStarted = false;
                _gameManager.canButtonsBeClicked = true;
            }
        }
        
        if (Input.GetMouseButtonUp(0) && panning)
        {
            panning = false;
            cameraWasMoving = true;
            _gameManager.canButtonsBeClicked = true;
            hasDraggingStarted = false;
        }
        
        tempPos.x = Mathf.Clamp(tempPos.x, panLimitX.x, panLimitX.y);
        tempPos.y = Mathf.Clamp(tempPos.y, panLimitY.x, panLimitY.y);
        transform.position = tempPos;
    }
    
}
