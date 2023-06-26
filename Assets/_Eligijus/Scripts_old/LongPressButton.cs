using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    public float requiredHoldTime;
    public float holdDelayTime;

    public UnityEvent onLongClick;
    public Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            pointerDown = true;
            //Debug.Log("Pointer down");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
        //Debug.Log("Pointer up");
    }

    private void Update()
    {
        if(pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if(pointerDownTimer >= requiredHoldTime + holdDelayTime)
            {
                if(onLongClick != null)
                {
                    onLongClick.Invoke();
                }
                Reset();
            }
            fillImage.fillAmount = Mathf.Clamp((pointerDownTimer - holdDelayTime) / requiredHoldTime, 0f, 1.01f);
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = 0;
    }

}
