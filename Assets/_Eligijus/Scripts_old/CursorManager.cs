using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite Default;
    public Sprite CharacterHover;
    public Sprite MovementHover;
    public Sprite AttackHover;
    public bool isThisVisualCursor;
    void Start()
    {
        Cursor.visible = false;
    }
    void Update() //Viskas veikia, tik milisekunde atsilieka sitas cursor nuo tikrojo
    {
        if (isThisVisualCursor)
        {
            Vector2 visualCursorPosition = Input.mousePosition;
            transform.position = visualCursorPosition + new Vector2(50f, -50f);
        }
        else
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPosition + new Vector2(0.25f, -0.5f);
        }
    }

}
