using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldChange : MonoBehaviour
{
    [HideInInspector] public float time;
    public float disappearSpeed = 1f;
    public float moveSpeed = 0.005f;
    private Vector3 originalPosition;
    private Color originalColor;
    private Color color;
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void OnEnable()
    {
        time = 1f;
        originalPosition = transform.localPosition;
        _text = GetComponent<TextMeshProUGUI>();
        originalColor = _text.color;
        color = originalColor;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (time <= 0)
        {
            gameObject.SetActive(false);
            transform.localPosition = originalPosition;
            _text.color = originalColor;
        }
        else
        {
            transform.position += new Vector3(0f, moveSpeed);
            color.a -= (disappearSpeed * Time.fixedDeltaTime);
            _text.color = color;
            time -= Time.deltaTime;
        }
    }
}
