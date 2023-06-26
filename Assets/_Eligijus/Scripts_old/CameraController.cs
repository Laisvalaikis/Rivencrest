using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public bool CameraDragging = true;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimitX;
    public Vector2 panLimitY;
    public GameObject DefaultToFollow;
    public Vector3 touchStart;
    public Vector3 touchStartScreen;
    private bool[] isCameraMoving = new bool[4];
    private bool cameraWasMoving = false;
    private bool panning = false;
    [HideInInspector] public bool hasDraggingStarted;
    [HideInInspector] public float time = 1f;

    void Update()
    {
        Vector3 pos = transform.position;
        if (!GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable && !panning)
        {
            if (time > 0)
                time -= Time.deltaTime;
            if(time <= 0)
            {
                if (Input.GetKey("t") || Input.mousePosition.y >= Screen.height - panBorderThickness)
                {
                    GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
                    pos.y += panSpeed * Time.deltaTime;
                    isCameraMoving[0] = true;
                }
                else
                {
                    isCameraMoving[0] = false;
                }
                if (Input.GetKey("g") || Input.mousePosition.y <= panBorderThickness)
                {
                    GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
                    pos.y -= panSpeed * Time.deltaTime;
                    isCameraMoving[1] = true;
                }
                else
                {
                    isCameraMoving[1] = false;
                }
                if (Input.GetKey("h") || Input.mousePosition.x >= Screen.width - panBorderThickness)
                {
                    GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
                    pos.x += panSpeed * Time.deltaTime;
                    isCameraMoving[2] = true;
                }
                else
                {
                    isCameraMoving[2] = false;
                }
                if (Input.GetKey("f") || Input.mousePosition.x <= panBorderThickness)
                {
                    GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
                    pos.x -= panSpeed * Time.deltaTime;
                    isCameraMoving[3] = true;
                }
                else
                {
                    isCameraMoving[3] = false;
                }
            }
        }
        else time = 1f;

        if (isAnyCameraMoving())
        {
            cameraWasMoving = true;
        }
        else if (cameraWasMoving)
        {
            //Camera.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("MainCamera").transform;
            GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
            GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
            DefaultToFollow.transform.position = transform.position + new Vector3(0f, 0f, 8f);
            GetComponent<CinemachineVirtualCamera>().Follow = DefaultToFollow.transform;
            cameraWasMoving = false;
        }
        if (CameraDragging && !GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled)
        {
            if (Input.GetMouseButtonDown(0) && !GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled)
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchStartScreen = Input.mousePosition;
                hasDraggingStarted = true;
            }
            if (Input.GetMouseButton(0) && GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable
                    && !GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled && hasDraggingStarted)
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 directionScreen = touchStartScreen - Input.mousePosition;
                if (direction.magnitude > new Vector3(0.01f, 0.01f, 0f).magnitude && directionScreen.magnitude > new Vector3(0.01f, 0.01f, 0f).magnitude)
                {
                    panning = true;
                    GetComponent<CinemachineVirtualCamera>().Follow = null;
                    pos += direction;
                }
                GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked = false;
            }
            else
            {
                hasDraggingStarted = false;
                GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (panning)
                {
                    panning = false;
                    cameraWasMoving = true;
                    GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked = true;
                    hasDraggingStarted = false;
                }
            }
        }
        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.y = Mathf.Clamp(pos.y, panLimitY.x, panLimitY.y);



        transform.position = pos;
    }
    private bool isAnyCameraMoving()
    {
        for (int i = 0; i < isCameraMoving.Length; i++)
        {
            if (isCameraMoving[i])
            {
                return true;
            }
        }
        return false;
    }
}
