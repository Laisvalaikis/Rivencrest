using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [HideInInspector] public float time;
    private float disappearSpeed = 1f;
    private float moveSpeed = 0.005f;
    private Vector3 originalPosition;
    private Color originalColor;
    private Color color;
    private TextMeshProUGUI _textMeshProUGUI;
    private List <TextMeshProUGUI> damageTexts;
    [HideInInspector] public int damageBeingDealt;

    void Start()
    {
        originalPosition = transform.localPosition;
        //Debug.Log(originalPosition.ToString());
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //originalPosition = transform.position;
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        originalColor = _textMeshProUGUI.color;
        color = originalColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time -= Time.deltaTime;
        
       transform.position += new Vector3(0f, moveSpeed);
       color.a -= (disappearSpeed * Time.fixedDeltaTime);
        
       _textMeshProUGUI.color = color;
        if (time <= 0)
        {
            gameObject.SetActive(false);
            transform.localPosition = originalPosition;
            _textMeshProUGUI.color = originalColor;
        }
    }
}
